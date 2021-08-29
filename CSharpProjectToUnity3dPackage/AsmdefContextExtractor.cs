using DotLiquid;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CSharpProjectToUnity3dPackage
{
    public class AsmdefContextExtractor : IFileContextExtractor
    {
        private readonly Unity3dPackageConfiguration unity3DPackageConfiguration;
        private readonly Regex assamblyRegex;

        public AsmdefContextExtractor(Unity3dPackageConfiguration unity3DPackageConfiguration)
        {
            this.unity3DPackageConfiguration = unity3DPackageConfiguration;
            this.assamblyRegex = new Regex("<AssemblyName>(.*?)</AssemblyName>", RegexOptions.Singleline);
        }

        public Hash GetHash(string filePath)
        {
            string name = Path.GetFileNameWithoutExtension(filePath);

            var fileContent = File.ReadAllText(filePath);
            var matches = assamblyRegex.Match(fileContent);
            var assambly = matches.Groups[1].Value;

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
