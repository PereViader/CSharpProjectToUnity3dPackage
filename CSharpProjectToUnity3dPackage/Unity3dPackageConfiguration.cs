using System.Collections.Generic;

namespace CSharpProjectToUnity3dPackage
{
    public class Unity3dPackageConfiguration
    {
        public List<AssamblyConfiguration> AssamblyConfigurations { get; set; }
        public List<string> IgnorePaths { get; set; }
    }
}
