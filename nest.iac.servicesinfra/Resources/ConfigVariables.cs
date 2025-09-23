using nest.iac.servicesinfra.Entities;
using Pulumi;

namespace nest.iac.servicesinfra.Resources
{
    public class ConfigVariables
    {
        private static Config ConfigAws = new Config("cld");
        public static string ProjectName { get { return Deployment.Instance.ProjectName; } }
        public static string Region { get { return Pulumi.Aws.Config.Region ?? "us-east-1"; } }
        public static string AwsVpcId { get { return ConfigAws.Require("vpcId"); } }
        public static string[] AwsSubnets { get { return ConfigAws.RequireObject<string[]>("subnets"); } }
        public static string[] AwsSecurityGroups { get { return ConfigAws.RequireObject<string[]>("securityGroups"); } }
        public static string AwsClusterArn { get { return ConfigAws.Require("clusterArn"); } }
        public static string AwsApiId { get { return ConfigAws.Require("apiId"); } }
        public static string AwsClusterName { get { return AwsClusterArn.Split('/')[1]; } }
        public static ServiceConfig[] AwsServices { get { return ConfigAws.RequireObject<ServiceConfig[]>("services"); } }
    }
}
