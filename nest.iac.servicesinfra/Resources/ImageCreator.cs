using Pulumi.Awsx.Ecr;

namespace nest.iac.servicesinfra.Resources
{
    public class ImageCreator
    {
        private readonly string name;
        private readonly Repository repository;
        private readonly string contextPath;
        private readonly string dockerFile;
        private readonly string versionImage;
        public ImageCreator(string name, Repository repository, string contextPath, string dockerFile, string versionImage) 
        {
            this.name = name;
            this.repository = repository;
            this.contextPath = contextPath;
            this.dockerFile = dockerFile;
            this.versionImage = versionImage;
        }
        public Image Build()
        {
            Image image = CreateImage(this.name, this.repository, this.contextPath, this.dockerFile, this.versionImage);
            return image;
        }
        public Image CreateImage(string name, Repository repository, string contextPath, string dockerFile, string versionImage)
        {
            return new Image(name, new ImageArgs {
                ImageName = name,
                RepositoryUrl = repository.Url,
                Context = contextPath,
                Dockerfile = dockerFile,
                ImageTag = versionImage
            });

        }
    }
}
