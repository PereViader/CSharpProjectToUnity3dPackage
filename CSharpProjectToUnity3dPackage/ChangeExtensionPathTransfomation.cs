using System.IO;

namespace CSharpProjectToUnity3dPackage
{
    public class ChangeExtensionPathTransfomation : IPathTransformation
    {
        private readonly string newExtension;

        public ChangeExtensionPathTransfomation(string newExtension)
        {
            this.newExtension = newExtension;
        }

        public string Transform(string path)
        {
            var extension = Path.GetExtension(path);
            var pathWithoutExtension = path.Remove(path.Length - extension.Length, extension.Length);
            var pathNewExtension = pathWithoutExtension + "." + newExtension;
            return pathNewExtension;
        }
    }
}
