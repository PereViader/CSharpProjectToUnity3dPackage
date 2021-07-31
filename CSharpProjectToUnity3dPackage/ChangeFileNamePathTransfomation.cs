using System.IO;

namespace CSharpProjectToUnity3dPackage
{
    public class ChangeFileNamePathTransfomation : IPathTransformation
    {
        private readonly string fileName;

        public ChangeFileNamePathTransfomation(string fileName)
        {
            this.fileName = fileName;
        }

        public string Transform(string path)
        {
            var directoryPath = Path.GetDirectoryName(path);
            var newPath = Path.Combine(directoryPath, fileName);
            return newPath;
        }
    }
}
