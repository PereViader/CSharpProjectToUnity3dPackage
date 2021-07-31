using System.Collections.Generic;

namespace CSharpProjectToUnity3dPackage
{
    public class CompositeOutputter : IOutputter
    {
        private readonly IEnumerable<IOutputter> outputters;

        public CompositeOutputter(IEnumerable<IOutputter> outputters)
        {
            this.outputters = outputters;
        }

        public void Output(string contextPath)
        {
            foreach (var outputter in outputters)
            {
                outputter.Output(contextPath);
            }
        }
    }
}
