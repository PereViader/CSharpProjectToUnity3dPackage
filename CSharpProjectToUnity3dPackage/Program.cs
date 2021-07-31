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

            var asmdefFilePathTransformation = new CompositePathTransformation(new IPathTransformation[]
            {
                new ChangeExtensionPathTransfomation("asmdef"),
                inputToOutputPathTransformation
            });

            var asmdefMetaPathTransformation = new CompositePathTransformation(new IPathTransformation[]
            {
                asmdefFilePathTransformation,
                new MetaFileForFilePathTransformation()
            });

            var unityPackageFilePathTransformer = new CompositePathTransformation(new IPathTransformation[]
            {
                new ChangeFileNamePathTransfomation("package.json"),
                inputToOutputPathTransformation
            });

            var unityPackageMetaPathTransformer = new CompositePathTransformation(new IPathTransformation[]
            {
                unityPackageFilePathTransformer,
                new MetaFileForFilePathTransformation()
            });

            var directoryMetaWritter = new TemplateFileWritter(TemplateUtils.GetTemplateAtPath(FilePaths.Folder_Meta_Liquid));
            var csharpMetaWritter = new TemplateFileWritter(TemplateUtils.GetTemplateAtPath(FilePaths.Csharp_Meta_Liquid));
            var asmdefMetaWritter = new TemplateFileWritter(TemplateUtils.GetTemplateAtPath(FilePaths.AssamblyDefinition_Meta_Liquid));
            var asmdefFileWritter = new TemplateFileWritter(TemplateUtils.GetTemplateAtPath(FilePaths.AssamblyDefinition_Asmdef_Liquid));
            var textFileMetaWritter = new TemplateFileWritter(TemplateUtils.GetTemplateAtPath(FilePaths.TextFile_Meta_Liquid));

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

            IOutputter unity3dPackageOutputter = new CompositeOutputter(new IOutputter[]
            {
                new FileCopyOutputter(unityPackageFilePathTransformer),
                new TemplateExtractorOutputter(unityPackageMetaPathTransformer,textFileMetaWritter, guidContextExtractor),
            });

            var fileOutputterMapper = new FileOutputterMapper(new List<(IPathPredicate, IOutputter)>()
            {
                (new IsFileExtensionPathPredicate("cs"), csharpOutputter),
                (new IsFileExtensionPathPredicate("csproj"), asmdefOutputter),
                (new IsFileNamePathPredicate("unity3d-package.json"), unity3dPackageOutputter)
            });

            if (!Directory.Exists(inputPath))
            {
                Console.WriteLine($"Input directory at path {inputPath} does not exist");
                return;
            }

            PathUtils.ClearAndCreateDirectory(outputPath);

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
    }
}
