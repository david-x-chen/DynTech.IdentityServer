//#tool nuget:?package=xunit.runner.console

#tool nuget:?package=MSBuild.SonarQube.Runner.Tool
#addin nuget:?package=Cake.Sonar

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Sonar");
var configuration = Argument("configuration", "Release");
var build_version = Argument("build_version", "0.1");
var build_version_suf = Argument("build_version_suf", "1");
var sonar_server = Argument("sonar_server", "");
var sonar_key = Argument("sonar_key", "");
var sonar_token = Argument("sonar_token", "");


//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var publishDir = Directory("./publish");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////
 
Task("Clean")
    .Does(() =>
{
    CleanDirectory(publishDir);
    DotNetCoreClean("./DynTech.IdentityServer.sln");
});

Task("SonarBegin")
    .Does(() => 
{
    SonarBegin(new SonarBeginSettings{
        //# Supported parameters
        Key = sonar_key,
        Url = sonar_server,
        Login = sonar_token,
        //Password = "admin",
        Verbose = true,
        //# Custom parameters
        ArgumentCustomization = args => args
            .Append("/v:" + build_version + "-" + build_version_suf)
            .Append("/d:sonar.cs.opencover.reportsPaths=**/*.opencover.xml")
            .Append("/d:sonar.exclusions=**/wwwroot/js/src/**")
            .Append("/d:sonar.coverage.exclusions=**Tests*.cs")
            .Append("/d:sonar.cs.vscoveragexml.reportsPaths=**/*.coveragexml")
    });
});

Task("Build")
    .Does(() =>
{
    var settings = new DotNetCoreBuildSettings
    {
        Framework = "netcoreapp2.2",
        Configuration = configuration,
        VersionSuffix = build_version_suf
    };

    DotNetCoreBuild("./src/DynTech.IdentityServer/DynTech.IdentityServer.csproj", settings);
});

/*
Task("Publish")
    .Does(() =>
{
     var settings = new DotNetCorePublishSettings
     {
         Framework = "netcoreapp2.2",
         Configuration = configuration,
         OutputDirectory = publishDir
     };

     DotNetCorePublish("./src/*", settings);
});
 */
Task("Run-Unit-Tests")
    .Does(() =>
{
     var settings = new DotNetCoreTestSettings
     {
         Configuration = configuration,
         ArgumentCustomization = args=>args.Append("/p:CollectCoverage=true")
                                           .Append("/p:CoverletOutputFormat=opencover")
     };

     var projectFiles = GetFiles("./test/**/*.csproj");
     foreach(var file in projectFiles)
     {
         DotNetCoreTest(file.FullPath, settings);
     }
});

Task("SonarEnd")
  .Does(() => {
     SonarEnd(new SonarEndSettings{
        Login = sonar_token,
        //Password = "admin"
     });
  });

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Sonar")
    .IsDependentOn("Clean")
    .IsDependentOn("SonarBegin")
    .IsDependentOn("Build")
    .IsDependentOn("Run-Unit-Tests")
    .IsDependentOn("SonarEnd");
    //.IsDependentOn("Publish");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
