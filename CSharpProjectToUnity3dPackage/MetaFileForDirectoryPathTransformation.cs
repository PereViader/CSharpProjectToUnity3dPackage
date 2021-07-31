namespace CSharpProjectToUnity3dPackage
{
    public class MetaFileForDirectoryPathTransformation : IPathTransformation
    {
        public string Transform(string path)
        {
            var metaPath = PathUtils.GetMetaFilePathForFolder(path);
            return metaPath;
        }
    }
}
