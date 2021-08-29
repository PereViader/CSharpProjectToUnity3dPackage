using System.IO;
using System.Text.RegularExpressions;

namespace CSharpProjectToUnity3dPackage
{
    public class AssamblyExtractorFromCsproj
    {
        private readonly Regex assamblyRegex;

        public AssamblyExtractorFromCsproj()
        {
            this.assamblyRegex = new Regex("<AssemblyName>(.*?)</AssemblyName>", RegexOptions.Singleline);
        }

        public string GetAssambly(string filePath)
        {
            var fileContent = File.ReadAllText(filePath);
            var matches = assamblyRegex.Match(fileContent);
            var assambly = matches.Groups[1].Value;
            return assambly;
        }
    }
}
