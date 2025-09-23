using Pulumi.Aws.Ec2;

namespace nest.iac.servicesinfra.Resources
{
    public class SecurityGroupCreator
    {
        //public SecurityGroup CreatePublicSg(string name)
        //{
        //    return new SecurityGroup(name, new SecurityGroupArgs
        //    {
        //        VpcId = ConfigVariables.AwsVpcId,
        //        Ingress = {
        //            new Pulumi.Aws.Ec2.Inputs.SecurityGroupIngressArgs
        //            {
        //                Protocol = "tcp",
        //                FromPort = ConfigVariables.RdsDatabasePort,
        //                ToPort   = ConfigVariables.RdsDatabasePort,
        //                CidrBlocks = { "0.0.0.0/0" },
        //            },
        //            new Pulumi.Aws.Ec2.Inputs.SecurityGroupIngressArgs
        //            {
        //                Protocol = "tcp",
        //                FromPort = ConfigVariables.RdsDatabasePort,
        //                ToPort   = ConfigVariables.RdsDatabasePort,
        //                Ipv6CidrBlocks = { "::/0" },
        //            }
        //        },
        //        Egress = {
        //            new Pulumi.Aws.Ec2.Inputs.SecurityGroupEgressArgs
        //            {
        //                Protocol = "-1",
        //                FromPort = 0,
        //                ToPort   = 0,
        //                CidrBlocks = { "0.0.0.0/0" },
        //                Ipv6CidrBlocks = { "::/0" },
        //            }
        //        },
        //        Tags = { { "Name", name } }
        //    });
        //}
    }
}
