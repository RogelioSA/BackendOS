using Awsx = Pulumi.Awsx;

namespace nest.iac.servicesinfra.Resources
{
    public class EcrCreator
    {
        private readonly string repositoryName;
        private readonly string imageName;
        private readonly string contextPath;
        private readonly string dockerFile;
        private readonly string versionImage;
        private Awsx.Ecr.Repository currentRepository = null!;
        public EcrCreator(string repositoryName, string imageName, string contextPath, string dockerFile, string versionImage)
        {
            this.repositoryName = repositoryName;
            this.imageName = imageName;
            this.contextPath = contextPath;
            this.dockerFile = dockerFile;
            this.versionImage = versionImage;
        }
        public Awsx.Ecr.Image Build()
        {
            this.currentRepository = Create();
            Awsx.Ecr.Image currentImage = CreateImage();
            return currentImage;
        }
        private Awsx.Ecr.Repository Create()
        {
            return new Awsx.Ecr.Repository(this.repositoryName, new Awsx.Ecr.RepositoryArgs
            {
                Name = this.repositoryName,
                ImageScanningConfiguration = new Pulumi.Aws.Ecr.Inputs.RepositoryImageScanningConfigurationArgs
                {
                    ScanOnPush = true,
                },
                ForceDelete = true,
                LifecyclePolicy = new Awsx.Ecr.Inputs.LifecyclePolicyArgs
                {
                    Rules = new Pulumi.InputList<Awsx.Ecr.Inputs.LifecyclePolicyRuleArgs> {
                        new Awsx.Ecr.Inputs.LifecyclePolicyRuleArgs {
                            Description = "Keep last 10 images with tag 'latest'",
                            TagStatus = Awsx.Ecr.LifecycleTagStatus.Tagged,
                            TagPrefixList = { "latest" },
                            MaximumNumberOfImages = 10
                        },
                        new Awsx.Ecr.Inputs.LifecyclePolicyRuleArgs {
                            Description = "Expire untagged images older than 7 days",
                            TagStatus = Awsx.Ecr.LifecycleTagStatus.Untagged,
                            MaximumAgeLimit = 7
                        }
                    }
                }
            });
        }

        public Awsx.Ecr.Image CreateImage()
        {
            return new Awsx.Ecr.Image(this.imageName, new Awsx.Ecr.ImageArgs
            {
                ImageName = this.imageName,
                RepositoryUrl = this.currentRepository.Url,
                Context = this.contextPath,
                Dockerfile = this.dockerFile,
                ImageTag = this.versionImage
            });
        }
    }
}
