using System;
using System.IO;

namespace CSharpProjectToUnity3dPackage
{
    public class FileCopyOutputter : IOutputter
    {
        private readonly IPathTransformation pathTransformation;

        public FileCopyOutputter(IPathTransformation pathTransformation)
        {
            this.pathTransformation = pathTransformation;
        }

        public void Output(string contextPath)
        {
            var outputPath = pathTransformation.Transform(contextPath);
            File.Copy(contextPath, outputPath);
            Console.WriteLine($"Copy file from {contextPath} to {outputPath}");
        }
    }
}
