using DotLiquid;
using System;

namespace CSharpProjectToUnity3dPackage
{
    public class RandomGuidFileContextExtractor : IFileContextExtractor
    {
        public Hash GetHash(string filePath)
        {
            return Hash.FromAnonymousObject(new
            {
                guid = Guid.NewGuid().ToUnity3dString()
            });
        }
    }
}
