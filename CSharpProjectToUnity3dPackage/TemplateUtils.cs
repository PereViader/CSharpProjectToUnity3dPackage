using DotLiquid;
using System;
using System.IO;

namespace CSharpProjectToUnity3dPackage
{
    public static class TemplateUtils
    {
        public static Template GetTemplateAtPath(string path)
        {
            string text = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path));
            Template template = Template.Parse(text);
            return template;
        }
    }
}
