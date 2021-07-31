using System;
using System.IO;

namespace CSharpProjectToUnity3dPackage
{
    public class CreateDirectoryOutputter : IOutputter
    {
        private readonly IPathTransformation pathTransformation;

        public CreateDirectoryOutputter(IPathTransformation pathTransformation)
        {
            this.pathTransformation = pathTransformation;
        }

        public void Output(string contextPath)
        {
            var outputPath = pathTransformation.Transform(contextPath);
            Directory.CreateDirectory(outputPath);
            Console.WriteLine($"Create directory at {outputPath}");
        }
    }
}
