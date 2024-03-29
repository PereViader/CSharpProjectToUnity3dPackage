﻿using System.IO;

namespace CSharpProjectToUnity3dPackage
{
    public class IsFileExtensionPathPredicate : IPathPredicate
    {
        private readonly string fileExtension;

        public IsFileExtensionPathPredicate(string fileExtension)
        {
            this.fileExtension = fileExtension;
        }

        public bool Accepts(string filePath)
        {
            var untreatedExtension = Path.GetExtension(filePath);
            var fileExtension = string.IsNullOrEmpty(untreatedExtension) ? string.Empty : untreatedExtension.Remove(0, 1);
            var result = fileExtension.Equals(this.fileExtension);
            return result;
        }
    }
}
