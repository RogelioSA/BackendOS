using Pulumi;
using Aws = Pulumi.Aws;

namespace nest.iac.servicesinfra.Resources
{
    public class ApiGatewayCreator
    {
        private readonly string nameVpcLink;
        private readonly string nameIntegration;
        private readonly string nameRoute;
        private readonly string nameStage;
        private readonly string stageMain;
        private readonly string prefix;
        private readonly string routePath;
        private readonly string permissionName;
        private Aws.LB.Listener listener;
        private Aws.ApiGatewayV2.VpcLink vpcLink = null!;
        private Aws.ApiGatewayV2.Integration integration = null!;
        private Aws.ApiGatewayV2.Route route = null!;
        private Aws.ApiGatewayV2.Stage stage = null!;
        private Aws.Lambda.Function lambda = null!;
        private Aws.Lambda.Permission permission = null!;
        private bool deploy = false;
        public Output<string> ExecutionArn()
        {
            Output<Aws.ApiGatewayV2.GetApiResult> currentApi = Aws.ApiGatewayV2.GetApi.Invoke(new Aws.ApiGatewayV2.GetApiInvokeArgs { 
                ApiId = ConfigVariables.AwsApiId
            });
            return currentApi.Apply((x) => x.ExecutionArn);
        }
        public Output<string> EndpointUrl()
        {
            Output<Aws.ApiGatewayV2.GetApiResult> currentApi = Aws.ApiGatewayV2.GetApi.Invoke(new Aws.ApiGatewayV2.GetApiInvokeArgs
            {
                ApiId = ConfigVariables.AwsApiId
            });
            return currentApi.Apply((x) => x.ApiEndpoint);
        }
        public ApiGatewayCreator(string prefix, string stageMain, Aws.LB.Listener listener, string routePath, bool deploy)
        {
            this.prefix = prefix;
            this.nameVpcLink = $"{this.prefix}-vpcLink";
            this.nameIntegration = $"{this.prefix}-integration";
            this.nameRoute = $"{this.prefix}-route";
            this.nameStage = $"{this.prefix}-stg";
            this.listener = listener;
            this.stageMain = stageMain;
            this.routePath = routePath;
            this.deploy = deploy;
        }

        public ApiGatewayCreator(string prefix, string stageMain, Aws.Lambda.Function lambda, string routePath, bool deploy)
        {
            this.prefix = prefix;
            this.nameIntegration = $"{this.prefix}-integration";
            this.nameRoute = $"{this.prefix}-route";
            this.nameStage = $"{this.prefix}-stg";
            this.permissionName = $"{this.prefix}-permission";
            this.lambda = lambda;
            this.stageMain = stageMain;
            this.routePath = routePath;
            this.deploy = deploy;
        }

        public ApiGatewayCreator()
        {
            
        }

        public Aws.ApiGatewayV2.Route Build()
        {
            this.vpcLink = this.CreateVpcLink();
            this.integration = this.CreateIntegration();
            this.route = this.CreateRoutes();
            if(this.deploy)
                this.stage = this.CreateStageDeploy();
            return this.route;
        }

        public Aws.ApiGatewayV2.Route BuildLambda()
        {
            this.permission = this.CreatePermissionLambda();
            this.integration = this.CreateIntegrationAws();
            this.route = this.CreateRoutes();
            if (this.deploy)
                this.stage = this.CreateStageDeploy();
            return this.route;
        }
        private Aws.ApiGatewayV2.VpcLink CreateVpcLink()
        {
            return new Aws.ApiGatewayV2.VpcLink(nameVpcLink, new Aws.ApiGatewayV2.VpcLinkArgs {
                Name = nameVpcLink,
                SecurityGroupIds = ConfigVariables.AwsSecurityGroups,
                SubnetIds = ConfigVariables.AwsSubnets
            });
        }

        private Aws.ApiGatewayV2.Integration CreateIntegration()
        {
            return new Aws.ApiGatewayV2.Integration(nameIntegration, new Aws.ApiGatewayV2.IntegrationArgs {
                ApiId = ConfigVariables.AwsApiId,
                IntegrationType = "HTTP_PROXY",
                IntegrationUri = this.listener.Arn,
                IntegrationMethod = "ANY",
                ConnectionType = "VPC_LINK",
                ConnectionId = this.vpcLink.Id
            });
        }

        private Aws.ApiGatewayV2.Integration CreateIntegrationAws()
        {
            return new Aws.ApiGatewayV2.Integration(nameIntegration, new Aws.ApiGatewayV2.IntegrationArgs
            {
                ApiId = ConfigVariables.AwsApiId,
                IntegrationType = "AWS_PROXY",
                IntegrationUri = this.lambda.Arn,
                PayloadFormatVersion = "2.0"
            });
        }

        private Aws.Lambda.Permission CreatePermissionLambda()
        {
            return new Aws.Lambda.Permission(this.permissionName, new Aws.Lambda.PermissionArgs
            {
                Action = "lambda:InvokeFunction",
                Function = this.lambda.Name,
                Principal = "apigateway.amazonaws.com",
                SourceArn = Output.Format($"{this.ExecutionArn()}/*/*{this.routePath}/*")
            });
        }

        private Aws.ApiGatewayV2.Route CreateRoutes()
        {
            string mainRute = $"{nameRoute}-main";
            new Aws.ApiGatewayV2.Route(mainRute, new Aws.ApiGatewayV2.RouteArgs
            {
                ApiId = ConfigVariables.AwsApiId,
                RouteKey = $"ANY {this.routePath}",
                Target = this.integration.Id.Apply(integrationId => $"integrations/{integrationId}")
            });
            return new Aws.ApiGatewayV2.Route(nameRoute, new Aws.ApiGatewayV2.RouteArgs {
                ApiId = ConfigVariables.AwsApiId,
                RouteKey = $"ANY {this.routePath}/{{proxy+}}",
                Target = this.integration.Id.Apply(integrationId => $"integrations/{integrationId}") 
		    });
        }

        private Aws.ApiGatewayV2.Route CreateMain()
        {
            string mainRute = $"{nameRoute}-main";
            new Aws.ApiGatewayV2.Route(mainRute, new Aws.ApiGatewayV2.RouteArgs
            {
                ApiId = ConfigVariables.AwsApiId,
                RouteKey = $"ANY {this.routePath}",
                Target = this.integration.Id.Apply(integrationId => $"integrations/{integrationId}")
            });
            return new Aws.ApiGatewayV2.Route(nameRoute, new Aws.ApiGatewayV2.RouteArgs
            {
                ApiId = ConfigVariables.AwsApiId,
                RouteKey = $"ANY {this.routePath}/{{proxy+}}",
                Target = this.integration.Id.Apply(integrationId => $"integrations/{integrationId}")
            });
        }

        private Aws.ApiGatewayV2.Stage CreateStageDeploy()
        {
            return new Aws.ApiGatewayV2.Stage(this.nameStage, new Aws.ApiGatewayV2.StageArgs {
                Name = this.stageMain,
                ApiId = ConfigVariables.AwsApiId,
                AutoDeploy = true
            });
	    }

    }
}
