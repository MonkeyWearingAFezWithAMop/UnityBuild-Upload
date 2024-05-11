#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

public class BuildAllVersions
{
    [MenuItem("Building/Build All Versions")]
    public static void BuildAll()
    {
        // Define your build settings for each version here
        List<BuildSettings> allBuilds = new List<BuildSettings>();


        try { allBuilds.Add(new BuildSettings("Android", BuildTarget.Android, BuildOptions.None)); }
        catch { }

        try { allBuilds.Add(new BuildSettings("iOS", BuildTarget.iOS, BuildOptions.None)); }
        catch { }

        try { allBuilds.Add(new BuildSettings("MacOS", BuildTarget.StandaloneOSX, BuildOptions.None)); }
        catch { }

        try { allBuilds.Add(new BuildSettings("Windows", BuildTarget.StandaloneWindows, BuildOptions.None)); }
        catch { }

        try { allBuilds.Add(new BuildSettings("Windows64", BuildTarget.StandaloneWindows64, BuildOptions.None)); }
        catch { }

        try { allBuilds.Add(new BuildSettings("Linux", BuildTarget.StandaloneLinux64, BuildOptions.None)); }
        catch { }

        try { allBuilds.Add(new BuildSettings("WebGL", BuildTarget.WebGL, BuildOptions.None)); }
        catch { }




        //read in the settings from the settings file?

        //the settings should say things like "build for windows:true" "build for mac:true"

        //



        string _product_name = PlayerSettings.productName;

        foreach (BuildSettings build in allBuilds)
        {
            string outputPath = "Builds/" + _product_name  + "_"+ build.buildName+"/";
            string _zip_file_path = "Builds/" + _product_name + "-" + build.buildName + "_zip.zip";
            try
            {
                BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, outputPath + _product_name + "_" + build.buildName, build.buildTarget, build.buildOptions);
                //ZipFile.CreateFromDirectory(outputPath, _zip_file_path); //itch.io wants a .zip file?


                //upload to itch.io using butler?

                //upload to steam 



            }
            catch
            {
                Debug.LogError("error while trying to create a build for: " + build.buildName);
                continue;
            }

            
            
        }






        Debug.Log("All versions have been built successfully!");
    }

    [MenuItem("Building/Build & upload")]
    public static void BuildAndUpload()
    {
        Debug.LogWarning("WARNING - unimplemented...");
    }

    private class BuildSettings
    {
        public string buildName;
        public BuildTarget buildTarget;
        public BuildOptions buildOptions;

        public BuildSettings(string name, BuildTarget target, BuildOptions options)
        {
            buildName = name;
            buildTarget = target;
            buildOptions = options;
        }
    }
}
#endif
