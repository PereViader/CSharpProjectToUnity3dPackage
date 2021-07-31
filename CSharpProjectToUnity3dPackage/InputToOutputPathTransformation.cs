using System.IO;

namespace CSharpProjectToUnity3dPackage
{
    public class InputToOutputPathTransformation : IPathTransformation
    {
        private readonly string inputPath;
        private readonly string outputPath;

        public InputToOutputPathTransformation(string inputPath, string outputPath)
        {
            this.inputPath = inputPath;
            this.outputPath = outputPath;
        }

        public string Transform(string path)
        {
            string fileWithoutIput = Path.GetRelativePath(inputPath, path);
            string fileOnOutput = Path.Combine(outputPath, fileWithoutIput);
            return fileOnOutput;
        }
    }
}
