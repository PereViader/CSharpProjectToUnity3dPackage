using System;
using System.Collections.Generic;
using System.IO;

namespace CSharpProjectToUnity3dPackage
{
    public static class DirectoryFileTraverser
    {
        public static void TraverseAll(string rootDirectoryPath, Predicate<string> ignorePredicate, Action<string> traverseDirectory, Action<string> traverseFile)
        {
            var folderPaths = new Stack<string>();
            var filePaths = new Stack<string>();

            TraverseDirectoryFiles(ignorePredicate, traverseFile, filePaths, rootDirectoryPath);
            PushSubdirectories(folderPaths, rootDirectoryPath);

            while (folderPaths.Count > 0)
            {
                var folderPath = folderPaths.Pop();
                if (ignorePredicate.Invoke(folderPath))
                {
                    continue;
                }
                PushSubdirectories(folderPaths, folderPath);
                traverseDirectory.Invoke(folderPath);

                TraverseDirectoryFiles(ignorePredicate, traverseFile, filePaths, folderPath);
            }
        }

        private static void TraverseDirectoryFiles(Predicate<string> ignorePredicate, Action<string> traverseFile, Stack<string> filePaths, string path)
        {
            PushFiles(filePaths, path);

            while (filePaths.Count > 0)
            {
                var filePath = filePaths.Pop();
                if (ignorePredicate.Invoke(filePath))
                {
                    continue;
                }
                traverseFile.Invoke(filePath);
            }
        }

        private static void PushSubdirectories(Stack<string> paths, string inputPath)
        {
            foreach (var path in Directory.GetDirectories(inputPath))
            {
                paths.Push(path + "/");
            }
        }

        private static void PushFiles(Stack<string> paths, string inputPath)
        {
            foreach (var path in Directory.GetFiles(inputPath))
            {
                paths.Push(path);
            }
        }
    }
}
