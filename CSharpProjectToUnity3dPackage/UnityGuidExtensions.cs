using System;

namespace CSharpProjectToUnity3dPackage
{
    public static class UnityGuidExtensions
    {
        public static string ToUnity3dString(this Guid guid)
        {
            return guid.ToString("N");
        }
    }
}
