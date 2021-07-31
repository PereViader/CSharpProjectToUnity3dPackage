using System.IO;

namespace CSharpProjectToUnity3dPackage
{
    public class IsFileNamePathPredicate : IPathPredicate
    {
        private readonly string fileName;

        public IsFileNamePathPredicate(string fileName)
        {
            this.fileName = fileName;
        }

        public bool Accepts(string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            var result = fileName.Equals(this.fileName);
            return result;
        }
    }
}
