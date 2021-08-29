using DotLiquid;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSharpProjectToUnity3dPackage
{

    public class AsmdefContextExtractor : IFileContextExtractor
    {
        private readonly Unity3dPackageConfiguration unity3DPackageConfiguration;
        private readonly AssamblyExtractorFromCsproj assamblyExtractorFromCsproj;

        public AsmdefContextExtractor(Unity3dPackageConfiguration unity3DPackageConfiguration, AssamblyExtractorFromCsproj assamblyExtractorFromCsproj)
        {
            this.unity3DPackageConfiguration = unity3DPackageConfiguration;
            this.assamblyExtractorFromCsproj = assamblyExtractorFromCsproj;
        }

        public Hash GetHash(string filePath)
        {
            string name = Path.GetFileNameWithoutExtension(filePath);

            var assambly = assamblyExtractorFromCsproj.GetAssambly(filePath);

            var dependantAssamblies = unity3DPackageConfiguration.AssamblyConfigurations
                .FirstOrDefault(x => x.Assambly.Equals(assambly))
                ?.AssamblyDependencies ?? new List<string>();

            return Hash.FromAnonymousObject(new
            {
                name = name,
                references = dependantAssamblies
            });
        }


    }
}
