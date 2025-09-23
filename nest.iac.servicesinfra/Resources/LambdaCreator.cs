using Pulumi;
using Aws = Pulumi.Aws;
using Awsx = Pulumi.Awsx;

namespace nest.iac.servicesinfra.Resources
{
    public class LambdaCreator
    {
        public string lambdaName { get; set; }
        public string cwName { get; set; }
        public Awsx.Ecr.Image image { get; set; }
        private string basePath { get; set; }
        private Aws.Iam.Role role = null!;
        private Aws.Lambda.Function function = null!;
        private Output<string> endpointUrl { get; set; }
        public LambdaCreator(string lambdaName, Awsx.Ecr.Image image, string basePath, Aws.Iam.Role role, string cwName, Output<string> endpointUrl)
        {
            this.lambdaName = lambdaName;
            this.image = image;
            this.basePath = basePath;
            this.role = role;
            this.cwName = cwName;
            this.endpointUrl = endpointUrl;
        }
        public Aws.Lambda.Function Build()
        {
            this.function = this.Create();
            CreateCW();
            return function;
        }
        private Aws.Lambda.Function Create()
        {
            return new Aws.Lambda.Function(this.lambdaName, new Aws.Lambda.FunctionArgs
            {
                Name = this.lambdaName,
                PackageType = "Image",
                ImageUri = this.image.ImageUri,
                MemorySize = 512,
                Timeout = 30,
                Role = this.role.Arn,
                Environment = new Aws.Lambda.Inputs.FunctionEnvironmentArgs
                {
                    Variables = 
                    {
                        { "ASPNETCORE_ENVIRONMENT", "Development" },
                        { "ENGINE", "Npgsql" },
                        { "Connections__Npgsql", "Host=nest-generalinfra-instance.cibyifu5bsuf.us-east-1.rds.amazonaws.com;Port=5432;Database=nest;Username=lucia;Password=123Lucia01*;Application Name=Nest;" },
                        { "BASE_URL", this.basePath },
                        { "IS_LAMBDA", "True" },
                        { "URL_ENDPOINT", this.endpointUrl },
                    }
                },
                VpcConfig = new Aws.Lambda.Inputs.FunctionVpcConfigArgs
                {
                    SubnetIds = ConfigVariables.AwsSubnets,
                    SecurityGroupIds = ConfigVariables.AwsSecurityGroups
                },
            });
        }

        private void CreateCW()
        {
            var lg = new Aws.CloudWatch.LogGroup(this.cwName, new Aws.CloudWatch.LogGroupArgs
            {
                Name = this.function.Name.Apply(name => $"/aws/lambda/{name}"),
                RetentionInDays = 1
            });
        }
    }
}
