using nest.iac.servicesinfra.Entities;
using nest.iac.servicesinfra.Resources;
using Pulumi;

namespace nest.iac.servicesinfra
{
    class Index : Stack
    {
        public Index()
        {
            ApiGatewayCreator api = new ApiGatewayCreator();
            ServiceConfig[] services = ConfigVariables.AwsServices;
            //var (listener, loadbalancer) = new FargateCreator(Deployment.Instance.ProjectName).BuildLb();
            for (int i = 0; i < services.Length; i++)
            {
                ServiceConfig currentService = services[i];
                string prefix = $"{Deployment.Instance.ProjectName}-{currentService.serviceName}";
                string lambdaName = $"{prefix}-lambda";
                string prefixShort = currentService.serviceName;
                string ecrName = $"{prefix}-ecr";
                string imageName = $"{prefix}-image";
                string lambdaRoleName = $"{prefix}-lambda-role";
                string cwName = $"{prefix}-cw";
                //string taskRoleName = $"{prefix}-task-role";
                //string executionRoleName = $"{prefix}-exec-role";
                //string cloudwatchName = $"{prefix}-cw";
                //string ruleName = $"{prefix}-rule";
                string stageMain = "$default";
                string routePath = currentService.routepath;

                //var ecrImage = new EcrCreator(ecrName, imageName, currentService.contextDocker, currentService.pathProject, "latest").Build();
                //var (executionRole, taskRole) = new RoleCreator(executionRoleName, taskRoleName, prefix, Deployment.Instance.ProjectName).Build();
                //var cloudwatch = new CwCreator(cloudwatchName).Build();
                //var service = new FargateCreator(prefix, prefixShort, executionRole, taskRole, ecrImage, cloudwatch, listener, 8080, routePath, ruleName, i + 1).Build();
                //var route = new ApiGatewayCreator(prefix, stageMain, listener, routePath, i == 0).Build();
                var ecrImage2 = new EcrCreator(ecrName, imageName, currentService.contextDocker, currentService.pathProject, "latest").Build();
                var lambdaRole = new RoleCreator(lambdaRoleName, prefix, Deployment.Instance.ProjectName).BuildLambda();
                var lambda = new LambdaCreator(lambdaName, ecrImage2, routePath, lambdaRole, cwName, api.EndpointUrl()).Build();
                var route2 = new ApiGatewayCreator(prefix, stageMain, lambda, routePath, i == 0).BuildLambda();
            }
        }
    }
}
