using DotLiquid;

namespace CSharpProjectToUnity3dPackage
{
    public interface IFileContextExtractor
    {
        Hash GetHash(string filePath);
    }
}
