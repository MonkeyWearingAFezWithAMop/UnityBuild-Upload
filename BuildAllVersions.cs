#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;


//code to build all availible versions....
public partial class BuildAllVersions
{

    public static string project_path
    {
        get
        {
            // Get the path to the Assets folder
            string assetsPath = Application.dataPath;

            // Get the path to the project root folder
            string projectPath = System.IO.Path.GetDirectoryName(assetsPath);
            return projectPath;
        }
    }


    /// <summary>
    /// build all versions of the game and returns a dictionary {"channel":absolute path}
    /// </summary>
    [MenuItem("Building/Build All Versions")]

   
    public static Dictionary<string,string> BuildAll()
    {
        // Define your build settings for each version here
        List<BuildSettings> allBuilds = new List<BuildSettings>();


        //try { allBuilds.Add(new BuildSettings("Android", BuildTarget.Android, BuildOptions.None)); }
        //catch { }

        //try { allBuilds.Add(new BuildSettings("iOS", BuildTarget.iOS, BuildOptions.None)); }
        //catch { }

        try { allBuilds.Add(new BuildSettings("MacOS", BuildTarget.StandaloneOSX, BuildOptions.None)); }
        catch { }

        try { allBuilds.Add(new BuildSettings("win32", BuildTarget.StandaloneWindows, BuildOptions.None)); }
        catch { }

        //try { allBuilds.Add(new BuildSettings("win64", BuildTarget.StandaloneWindows64, BuildOptions.None)); }
        //catch { }

        try { allBuilds.Add(new BuildSettings("Linux", BuildTarget.StandaloneLinux64, BuildOptions.None)); }
        catch { }

        //try { allBuilds.Add(new BuildSettings("WebGL", BuildTarget.WebGL, BuildOptions.None)); }
        //catch { }


        Dictionary<string,string> _return_value = new Dictionary<string, string>();








        string _product_name = PlayerSettings.productName;

        foreach (BuildSettings build in allBuilds)
        {
            string outputPath = "Builds/" + _product_name  + "_"+ build.buildName+"/";
            string _zip_file_path = "Builds/" + _product_name + "-" + build.buildName + ".zip";
            _return_value.Add(build.buildName,Path.Combine(project_path,_zip_file_path));

            try
            {
                string _build_folder = outputPath + _product_name + "_" + build.buildName;

                BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, _build_folder, build.buildTarget, build.buildOptions);
                ZipFile.CreateFromDirectory(outputPath, _zip_file_path);



                





            }
            catch
            {
                UnityEngine.Debug.LogError("error while trying to create a build for: " + build.buildName);
                
                continue;
            }

            
            
        }


        return _return_value;


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
