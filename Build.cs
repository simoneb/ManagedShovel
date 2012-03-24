using System;
using System.Net;
using OpenFileSystem.IO;
using OpenFileSystem.IO.FileSystem.Local;
using Pencil;
using Pencil.Attributes;
using Pencil.Tasks;

public class PencilProject : Project
{
    [DependsOn("Clean")]
    [Description("Builds the project and produces the output binaries")]
    public void Build()
    {
        new MSBuild40Task
        {
            ShowCommandLine = true,
            ProjectFile = @"src\ManagedShovel.sln",
            Properties = { { "Configuration", "Release" }, { "Platform", "Any CPU" } },
            Verbosity = MSBuildVerbosity.Quiet,
            Targets = { "Rebuild" }
        }.Run();
    }

    [Description("Cleans the artifacts generated during the build process")]
    public void Clean()
    {
        new MSBuild40Task
        {
            ShowCommandLine = true,
            ProjectFile = @"src\ManagedShovel.sln",
            Properties = { { "Configuration", "Release" }, { "Platform", "Any CPU" } },
            Verbosity = MSBuildVerbosity.Quiet,
            Targets = { "Clean" }
        }.Run();

        FileSystem.GetDirectory("dist").Delete();
    }

    [Default]
    [DependsOn("Build")]
    [Description("Copies the output to the distribution folder")]
    public void Dist()
    {
        var dist = FileSystem.GetDirectory("dist").MustExist();

        foreach (var file in FileSystem.GetDirectory(@"src\ManagedShovel\bin\Release").Files())
            file.CopyTo(dist.GetFile(file.Name));
    }
}