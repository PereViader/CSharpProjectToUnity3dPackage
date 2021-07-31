namespace CSharpProjectToUnity3dPackage
{
    public class MetaFileForFilePathTransformation : IPathTransformation
    {
        public string Transform(string path)
        {
            var metaPath = PathUtils.GetMetaFilePathForFile(path);
            return metaPath;
        }
    }
}
