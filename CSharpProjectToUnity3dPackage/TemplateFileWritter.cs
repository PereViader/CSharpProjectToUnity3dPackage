using DotLiquid;
using System.IO;

namespace CSharpProjectToUnity3dPackage
{
    public class TemplateFileWritter
    {
        private readonly Template template;

        public TemplateFileWritter(Template template)
        {
            this.template = template;
        }

        public void Output(string path, Hash hash)
        {
            var metaFileRender = template.Render(hash);

            using (var metaFile = File.CreateText(path))
            {
                metaFile.Write(metaFileRender);
            }
        }
    }
}
