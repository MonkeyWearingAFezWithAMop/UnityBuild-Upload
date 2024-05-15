#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

using System.Diagnostics;

public class BuildAllVersions
{

    public static string itch_username = "monkeywearingafezwithamop";
    public static string itch_project = "bulter-tests";

    /// <summary>
    /// build all versions of the game...
    /// </summary>
    [MenuItem("Building/Build All Versions")]
    
    public static Dictionary<string,string> BuildAll()
    {
        // Define your build settings for each version here
        List<BuildSettings> allBuilds = new List<BuildSettings>();


        try { allBuilds.Add(new BuildSettings("Android", BuildTarget.Android, BuildOptions.None)); }
        catch { }

        try { allBuilds.Add(new BuildSettings("iOS", BuildTarget.iOS, BuildOptions.None)); }
        catch { }

        try { allBuilds.Add(new BuildSettings("MacOS", BuildTarget.StandaloneOSX, BuildOptions.None)); }
        catch { }

        try { allBuilds.Add(new BuildSettings("Windows32", BuildTarget.StandaloneWindows, BuildOptions.None)); }
        catch { }

        try { allBuilds.Add(new BuildSettings("Windows64", BuildTarget.StandaloneWindows64, BuildOptions.None)); }
        catch { }

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
            _return_value.Add(build.buildName,_zip_file_path);

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


    


    static void RunMultipleCommands(string commands)
    {
        ProcessStartInfo processInfo = new ProcessStartInfo("bash", "-c \"" + commands + "\"");
        processInfo.RedirectStandardOutput = true;
        processInfo.UseShellExecute = false;
        processInfo.CreateNoWindow = true;

        Process process = new Process();
        process.StartInfo = processInfo;
        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        //string error = process.StandardError.ReadToEnd();
        process.WaitForExit();

    }




    [MenuItem("Building/Build & upload (itch.io)")]
    public static void BuildAndUpload_itchIO()
    {

        RunMultipleCommands("butler login");//&& for more commands...
        

        Dictionary<string,string> _output_files = BuildAll();



        foreach (KeyValuePair<string,string> _KVP in _output_files)
        {
            //compress...
            //upload to itch io...
            UnityEngine.Debug.Log("handling " + _KVP.Key + " at: "+_KVP.Value);
        }



        




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
