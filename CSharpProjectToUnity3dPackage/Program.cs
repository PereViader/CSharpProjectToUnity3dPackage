using System;
using System.Collections.Generic;
using System.IO;

namespace CSharpProjectToUnity3dPackage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string inputPath = args[0];
            string outputPath = args[1];

            var inputToOutputPathTransformation = new InputToOutputPathTransformation(inputPath, outputPath);
            var metaFilePathTransformation = new CompositePathTransformation(new IPathTransformation[]
            {
                inputToOutputPathTransformation,
                new MetaFileForFilePathTransformation()
            });
            var metaDirectoryPathTransformation = new CompositePathTransformation(new IPathTransformation[]
            {
                inputToOutputPathTransformation,
                new MetaFileForDirectoryPathTransformation()
            });

            var toAsmdefExtensionFilePathTransformer = new ChangeExtensionPathTransfomation("asmdef");

            var asmdefFilePathTransformation = new CompositePathTransformation(new IPathTransformation[]
            {
                toAsmdefExtensionFilePathTransformer,
                inputToOutputPathTransformation
            });

            var asmdefMetaPathTransformation = new CompositePathTransformation(new IPathTransformation[]
            {
                asmdefFilePathTransformation,
                new MetaFileForFilePathTransformation()
            });

            var directoryMetaWritter = new TemplateFileWritter(TemplateUtils.GetTemplateAtPath(FilePaths.Folder_Meta_Liquid));
            var csharpMetaWritter = new TemplateFileWritter(TemplateUtils.GetTemplateAtPath(FilePaths.Csharp_Meta_Liquid));
            var asmdefMetaWritter = new TemplateFileWritter(TemplateUtils.GetTemplateAtPath(FilePaths.AssamblyDefinition_Meta_Liquid));
            var asmdefFileWritter = new TemplateFileWritter(TemplateUtils.GetTemplateAtPath(FilePaths.AssamblyDefinition_Asmdef_Liquid));

            var guidContextExtractor = new GuidFileContextExtractor();
            var nameContextExtractor = new NameWithoutExtensionFileContextExtractor();

            IOutputter folderOutputter = new CompositeOutputter(new IOutputter[]
            {
                new CreateDirectoryOutputter(inputToOutputPathTransformation),
                new TemplateExtractorOutputter(metaDirectoryPathTransformation, directoryMetaWritter, guidContextExtractor)
            });

            IOutputter csharpOutputter = new CompositeOutputter(new IOutputter[]
            {
                new FileCopyOutputter(inputToOutputPathTransformation),
                new TemplateExtractorOutputter(metaFilePathTransformation, csharpMetaWritter, guidContextExtractor)
            });

            IOutputter asmdefOutputter = new CompositeOutputter(new IOutputter[]
            {
                new TemplateExtractorOutputter(asmdefFilePathTransformation, asmdefFileWritter, nameContextExtractor),
                new TemplateExtractorOutputter(asmdefMetaPathTransformation, asmdefMetaWritter, guidContextExtractor)
            });

            var fileOutputterMapper = new FileOutputterMapper(new List<(IPathPredicate, IOutputter)>()
            {
                (new IsFileExtensionPathPredicate("cs"), csharpOutputter),
                (new IsFileExtensionPathPredicate("csproj"), asmdefOutputter)
            });

            ClearAndCreateDirectory(outputPath);

            if (!Directory.Exists(inputPath))
            {
                Console.WriteLine($"Input directory at path {inputPath} does not exist");
                return;
            }

            DirectoryFileTraverser.TraverseAll(inputPath, directoryPath =>
            {
                folderOutputter.Output(directoryPath);
            },
            filePath =>
            {
                if (!fileOutputterMapper.TryGetOutputerForFile(filePath, out var outputter))
                {
                    Console.WriteLine($"Skipping {filePath}");
                    return;
                }

                outputter.Output(filePath);
            });
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
