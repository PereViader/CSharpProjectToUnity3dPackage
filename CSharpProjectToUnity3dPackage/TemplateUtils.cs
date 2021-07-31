using DotLiquid;
using System.IO;

namespace CSharpProjectToUnity3dPackage
{
    public static class TemplateUtils
    {
        public static Template GetTemplateAtPath(string path)
        {
            string text = File.ReadAllText(path);
            Template template = Template.Parse(text);
            return template;
        }
    }
}
