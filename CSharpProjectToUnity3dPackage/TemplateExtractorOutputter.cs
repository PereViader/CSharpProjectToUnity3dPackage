using System;

namespace CSharpProjectToUnity3dPackage
{
    public class TemplateExtractorOutputter : IOutputter
    {
        private readonly IPathTransformation pathTransformation;
        private readonly TemplateFileWritter templateFileOutputter;
        private readonly IFileContextExtractor fileContextExtractor;

        public TemplateExtractorOutputter(IPathTransformation pathTransformation, TemplateFileWritter templateFileOutputter, IFileContextExtractor fileContextExtractor)
        {
            this.pathTransformation = pathTransformation;
            this.templateFileOutputter = templateFileOutputter;
            this.fileContextExtractor = fileContextExtractor;
        }

        public void Output(string contextPath)
        {
            var hash = fileContextExtractor.GetHash(contextPath);
            var outputPath = pathTransformation.Transform(contextPath);
            templateFileOutputter.Output(outputPath, hash);
            Console.WriteLine($"Create template at {outputPath}");
        }
    }
}
