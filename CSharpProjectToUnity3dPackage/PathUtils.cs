using System.IO;

namespace CSharpProjectToUnity3dPackage
{
    public static class PathUtils
    {
        public static string GetParentPath(string path)
        {
            return Path.Combine(path, "..");
        }

        public static string GetMetaFilePathForFolder(string path)
        {
            var name = Path.GetFileName(Path.TrimEndingDirectorySeparator(path));
            var parentOutputPath = PathUtils.GetParentPath(path);
            var metaFilePath = Path.Combine(parentOutputPath, name + ".meta");
            return metaFilePath;
        }

        public static string GetMetaFilePathForFile(string path)
        {
            return path + ".meta";
        }

        public static void ClearAndCreateDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            Directory.CreateDirectory(path);
        }
    }
}
