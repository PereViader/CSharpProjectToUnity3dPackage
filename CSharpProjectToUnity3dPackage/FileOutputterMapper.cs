using System.Collections.Generic;

namespace CSharpProjectToUnity3dPackage
{
    public class FileOutputterMapper
    {
        private IEnumerable<(IPathPredicate pathPredicate, IOutputter outputter)> outputters;

        public FileOutputterMapper(IEnumerable<(IPathPredicate, IOutputter)> outputters)
        {
            this.outputters = outputters;
        }

        public bool TryGetOutputerForFile(string filePath, out IOutputter outputter)
        {
            foreach (var iOutputter in outputters)
            {
                if (iOutputter.pathPredicate.Accepts(filePath))
                {
                    outputter = iOutputter.outputter;
                    return true;
                }
            }

            outputter = default;
            return false;
        }
    }
}
