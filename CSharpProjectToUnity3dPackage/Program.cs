using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace CSharpProjectToUnity3dPackage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string inputPath = args[0];
            string outputPath = args[1];

            var configuration = GetConfiguration(inputPath);
            var ignore = GetIgnore(configuration);

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

            var assamblyExtractorFromCsproj = new AssamblyExtractorFromCsproj();
            var guidContextExtractor = new RandomGuidFileContextExtractor();
            var asmdefMetaFileContextExtractor = new AsmdefMetaFileContextExtractor(assamblyExtractorFromCsproj, configuration);
            var asmdefContextExtractor = new AsmdefContextExtractor(configuration, assamblyExtractorFromCsproj);

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
                new TemplateExtractorOutputter(asmdefFilePathTransformation, asmdefFileWritter, asmdefContextExtractor),
                new TemplateExtractorOutputter(asmdefMetaPathTransformation, asmdefMetaWritter, asmdefMetaFileContextExtractor)
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
                (new IsFileNamePathPredicate("unity3d-packageFile.json"), unity3dPackageOutputter)
            });

            if (!Directory.Exists(inputPath))
            {
                Console.WriteLine($"Input directory at path {inputPath} does not exist");
                return;
            }

            PathUtils.ClearAndCreateDirectory(outputPath);

            DirectoryFileTraverser.TraverseAll(inputPath, path =>
            {
                if (ignore.IsIgnored(path))
                {
                    Console.WriteLine($"Ignoring {path}");
                    return true;
                }
                return false;
            },
            directoryPath =>
            {
                folderOutputter.Output(directoryPath);
            },
            filePath =>
            {
                if (!fileOutputterMapper.TryGetOutputerForFile(filePath, out var outputter))
                {
                    Console.WriteLine($"Skipping file due to not having outputter: {filePath}");
                    return;
                }

                outputter.Output(filePath);
            });
        }

        private static Ignore.Ignore GetIgnore(Unity3dPackageConfiguration unity3DPackageConfiguration)
        {
            var ignore = new Ignore.Ignore();

            ignore.Add(unity3DPackageConfiguration.IgnorePaths);

            return ignore;
        }

        private static Unity3dPackageConfiguration GetConfiguration(string inputPath)
        {
            try
            {
                var path = Path.Combine(inputPath, FilePaths.Unity3dPackage_Configuartion_Json);
                var text = File.ReadAllText(path);
                var configuration = JsonSerializer.Deserialize<Unity3dPackageConfiguration>(text);
                if (configuration.IgnorePaths == null)
                {
                    configuration.IgnorePaths = new List<string>();
                }
                if (configuration.AssamblyConfigurations == null)
                {
                    configuration.AssamblyConfigurations = new List<AssamblyConfiguration>();
                }
                return configuration;
            }
            catch
            {
                return new Unity3dPackageConfiguration()
                {
                    AssamblyConfigurations = new List<AssamblyConfiguration>()
                };
            }
        }
    }
}
