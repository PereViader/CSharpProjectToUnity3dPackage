using DotLiquid;
using System.Linq;

namespace CSharpProjectToUnity3dPackage
{
    public class AsmdefMetaFileContextExtractor : IFileContextExtractor
    {
        private readonly AssamblyExtractorFromCsproj assamblyExtractorFromCsproj;
        private readonly Unity3dPackageConfiguration unity3DPackageConfiguration;

        public AsmdefMetaFileContextExtractor(AssamblyExtractorFromCsproj assamblyExtractorFromCsproj, Unity3dPackageConfiguration unity3DPackageConfiguration)
        {
            this.assamblyExtractorFromCsproj = assamblyExtractorFromCsproj;
            this.unity3DPackageConfiguration = unity3DPackageConfiguration;
        }

        public Hash GetHash(string filePath)
        {
            var assambly = assamblyExtractorFromCsproj.GetAssambly(filePath);

            var guid = unity3DPackageConfiguration.AssamblyConfigurations.First(x => x.Assambly.Equals(assambly)).Guid;

            return Hash.FromAnonymousObject(new
            {
                guid = guid
            });
        }
    }
}
