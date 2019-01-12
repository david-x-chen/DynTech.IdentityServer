public static class Settings
{
    public static string ProjectName => "DynTech.IdentityServer";

    public static string SonarUrl => "https://sonarqube.dynweb.biz";

    public static string SonarKey => "dyn-idsrv4";

    public static string SonarName => "dyn-idsrv4";

    public static string SonarExclude => "/d:sonar.exclusions=Program.cs";

    public static string SonarExcludeDuplications => "/d:sonar.cpd.exclusions=**/GalleryContextExtensions.cs";
}