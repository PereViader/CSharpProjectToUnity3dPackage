# CSharpProjectToUnity3dProject
This github action turns a c# project into a Unity3d project.

This is needed because unity doescan't use a c# project directly

- A pure c# project does not have meta files for files / folders
- Some files are not used by unity because it will generate them (sln, csproj)

The action will open your repository looking for a `unity3d-packageConfiguration.json` and `unity3d-packageFile.json` on the root of the project
It will output the `unity3d-packageFile.json` directly as a `package.json` expected by UPM and using the configuration will traverse the repositorty and create the necessary meta and asmdef files that unity will use.

Sample configuration json file


```
{
  "AssamblyConfigurations" : [
    { 
      "Assambly": "ManualDi.Main",
      "AssamblyDependencies" : [
      ],
      "Guid" : "2831785f1fa04efeb10b7d72b2e32628"
    }
  ],
  "IgnorePaths" : [
    "[Bb]in/",
    "[Oo]bj/",
    ".git/",
    ".vs/",
    "ManualDi.Main.Tests/",
    "ManualDi.Main/Properties/",
    ".github/"
  ]
}
```

Looking at the confguration we can see
- AssamblyConfigurations: Defines what will be the contents of the asmdef files
- IgnorePaths: Array of string paths using the gitignore format to define what paths should be ignored

## Scripts and folders
The guid of the meta files will be random

## Asmdef
Are created when a csproj files is present. 
The contents of the asmdef asset file are defined on the configuration.
The guid of the asmdef meta is defined on the configuration.  


# Running the action
The action currently has 2 parameters
- inputPath: defaults to the root of the repository `./`
- outputPath: defaults to `./OutputUnity3dPackage/`


# Example usages of this project

This project is currently beeing used here https://github.com/PereViader/ManualDi.Main
https://github.com/PereViader/ManualDi.Main/blob/main/.github/workflows/publish-unity3d-package.yml

Notice the json files at the root of the repository

The `publih-unity3d-package` workflow transforms the c# project and then uploads it to the same repository on a separate branch named `upm`.
The branch is completely unrelated to the development branch to keep it clean.
