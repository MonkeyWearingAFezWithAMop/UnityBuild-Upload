#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;


//code to build all availible versions....
public partial class BuildAllVersions
{

    public static string product_name { get { return PlayerSettings.productName; } }

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

   
    public static BuildOutput BuildAll()
    {
        // Define your build settings for each version here
        List<BuildSettings> allBuilds = new List<BuildSettings>();


        BuildOutput _output = new BuildOutput();




        try { allBuilds.Add(new BuildSettings("Android", BuildTarget.Android, BuildOptions.None)); }
        catch { }

        try { allBuilds.Add(new BuildSettings("iOS", BuildTarget.iOS, BuildOptions.None)); }
        catch { }

        try { allBuilds.Add(new BuildSettings("macOS", BuildTarget.StandaloneOSX, BuildOptions.None)); }
        catch { }

        try { allBuilds.Add(new BuildSettings("win32", BuildTarget.StandaloneWindows, BuildOptions.None)); }
        catch { }

        try { allBuilds.Add(new BuildSettings("win64", BuildTarget.StandaloneWindows64, BuildOptions.None)); }
        catch { }

        try { allBuilds.Add(new BuildSettings("linux", BuildTarget.StandaloneLinux64, BuildOptions.None)); }
        catch { }




        Dictionary<string,string> _standalone_dict = new Dictionary<string, string>();



        //create builds into path_to_your_project/Builds (up one level from /Assets)

        foreach (BuildSettings build in allBuilds)
        {
            string _pipeline_output_path = "Builds/" + product_name  + "_"+ build.buildName+"/";
            string _zip_file_path = "Builds/" + product_name + "-" + build.buildName + ".zip";
            _standalone_dict.Add(build.buildName,Path.Combine(project_path,_zip_file_path));

            try
            {
                string _build_folder = _pipeline_output_path + product_name + "_" + build.buildName;

                BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, _build_folder, build.buildTarget, build.buildOptions);

                if (File.Exists(_zip_file_path))
                    File.Delete(_zip_file_path);
                ZipFile.CreateFromDirectory(_pipeline_output_path, _zip_file_path);

            }
            catch
            {
                UnityEngine.Debug.LogError("error while trying to create a build for: " + build.buildName);
                
                continue;
            }

            
            
        }

        _output.standalone_locations = _standalone_dict;

        _output.webgl_path = buildForBrowser();


        return _output;


    }


    static string buildForBrowser()
    {
        string _pipeline_output_path = "Builds/" + product_name + "-browser";
        string _zip_file_path = "Builds/"+ product_name + "-browser.zip";

        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes,_pipeline_output_path, BuildTarget.WebGL, BuildOptions.None);

        if (File.Exists(_zip_file_path))
            File.Delete(_zip_file_path);
        ZipFile.CreateFromDirectory(_pipeline_output_path, _zip_file_path);


        return _zip_file_path;
    }


    public struct BuildOutput
    {

        public Dictionary<string, string> standalone_locations;

        public string webgl_path;


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
