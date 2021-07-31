using DotLiquid;
using System.IO;

namespace CSharpProjectToUnity3dPackage
{
    public class NameWithoutExtensionFileContextExtractor : IFileContextExtractor
    {
        public Hash GetHash(string filePath)
        {
            string name = Path.GetFileNameWithoutExtension(filePath);

            return Hash.FromAnonymousObject(new
            {
                name = name
            });
        }
    }
}
