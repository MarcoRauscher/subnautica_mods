var target = Argument("target", "Default");

var subnauticaDir = Argument("subnauticaDir", @"D:\Steamgames\steamapps\common\Subnautica\");



var subnauticaDataDir = Argument("subnauticaDataDir", subnauticaDir + @"Subnautica_Data\");
var subnauticaDataManagedDir = Argument("subnauticaDataManagedDir", subnauticaDataDir + @"Managed\");
var subnauticaQModsDir = Argument("subnauticaQModsDir", subnauticaDir + @"QMods\");

Task("Default")
  .Does(() =>
{
  Information("Hello World!");
  Information("Subnautica Directory: " + subnauticaDir);
  Information("Subnautica QMods Directory: " + subnauticaQModsDir);
  Information("Subnautica Managed Data Directory: " + subnauticaDataManagedDir);
  Information("Subnautica Data Directory: " + subnauticaDataDir);
});

Task("Test")
  .Does(() =>
{
	Information("Test!");
});

RunTarget(target);