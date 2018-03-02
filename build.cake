var target = Argument("target", "Default");
var Configuration = Argument("configuration", "release");

var subnauticaDir = Argument("subnauticaDir", @"D:\Steamgames\steamapps\common\Subnautica\");



var subnauticaDataDir = Argument("subnauticaDataDir", subnauticaDir + @"Subnautica_Data\");
var subnauticaDataManagedDir = Argument("subnauticaDataManagedDir", subnauticaDataDir + @"Managed\");
var subnauticaQModsDir = Argument("subnauticaQModsDir", subnauticaDir + @"QMods\");

var binaryDirectory = "./bin";
var Solution = "SubnauticaMods.sln";

var BuildVerbosity = Verbosity.Minimal;

Task("Clean binary directories")
  .Does(() =>
{
    CleanDirectories(string.Format("./bin/**/Debug"));
    CleanDirectories(string.Format("./bin/**/Release"));
});


Task("DisplayBuildInformation")
  .Does(() =>
{
  Information("Subnautica Directory: " + subnauticaDir);
  Information("Subnautica QMods Directory: " + subnauticaQModsDir);
  Information("Subnautica Managed Data Directory: " + subnauticaDataManagedDir);
  Information("Subnautica Data Directory: " + subnauticaDataDir);
});


Task("NuGet Package Restore")
    .Does(() => NuGetRestore(Solution));


Task("Build")
    .IsDependentOn("Clean binary directories")
    .IsDependentOn("DisplayBuildInformation")
    .IsDependentOn("NuGet Package Restore")
    .Does(() =>
    {
        MSBuild(Solution, cfg =>
        {
            cfg.Configuration = Configuration;
            cfg.Verbosity = BuildVerbosity;
        });
    });

Task("DeployFishOverflowDistributorLocal")
    .IsDependentOn("Build")
    .Does(() =>
    {
        CopyDirectory(binaryDirectory + "/FishOverflowDistributor/" + Configuration, subnauticaQModsDir + "FishOverflowDistributor");
    });

RunTarget(target)