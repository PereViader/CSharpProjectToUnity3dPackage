using System.Collections.Generic;

namespace CSharpProjectToUnity3dPackage
{
    public class CompositePathTransformation : IPathTransformation
    {
        private readonly IEnumerable<IPathTransformation> pathTransformations;

        public CompositePathTransformation(IEnumerable<IPathTransformation> pathTransformations)
        {
            this.pathTransformations = pathTransformations;
        }

        public string Transform(string path)
        {
            var result = path;
            foreach (var transformation in pathTransformations)
            {
                result = transformation.Transform(result);
            }
            return result;
        }
    }
}
