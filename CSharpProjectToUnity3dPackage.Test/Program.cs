using System.IO;

namespace CSharpProjectToUnity3dPackage.Test
{
    class Program
    {
        public static void Main(string[] args)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string inputPath = Path.Combine(currentDirectory, "Input");
            string outputPath = Path.Combine(currentDirectory, "Output");
            EntryPoint.Run(new[] { inputPath, outputPath });
        }
    }
}
