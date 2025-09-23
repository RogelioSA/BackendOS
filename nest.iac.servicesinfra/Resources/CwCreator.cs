using Aws = Pulumi.Aws;

namespace nest.iac.servicesinfra.Resources
{
    public class CwCreator
    {
        private readonly string name;
        public CwCreator(string name)
        {
            this.name = name;
        }
        public Aws.CloudWatch.LogGroup Build()
        {
            return CreateCloudWatch();
        }
        public Aws.CloudWatch.LogGroup CreateCloudWatch()
        {
            return new Aws.CloudWatch.LogGroup(name, new Aws.CloudWatch.LogGroupArgs
            {
                Name = name,
                RetentionInDays = 1
            });
        }
    }
}
