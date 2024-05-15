#if UNITY_EDITOR
using System.Collections;using System.Collections.Generic;using System.Linq;using System;
using UnityEngine;
using UnityEditor;

using System.Diagnostics;

public partial class BuildAllVersions
{


    public static string itch_username = "monkeywearingafezwithamop";//read in from a seperate file???
    public static string itch_project = "bulter-tests";//read in from a seperate file???


    [MenuItem("Building/Build & upload (itch.io)")]
    public static void BuildAndUpload_itchIO()
    {

        //print uploading!


        Dictionary<string, string> _output_files = BuildAll();

        //e.g. win64:path to file....




        foreach (KeyValuePair<string, string> _KVP in _output_files)
        {
            //ok so we need to generate the commands for butler to run...
            UnityEngine.Debug.Log("handling " + _KVP.Key + " at: " + _KVP.Value);


            string _build_directory = _KVP.Value;

            string _command = string.Format("butler push {0} {1}/{2}:{3}", _build_directory, itch_username, itch_project, _KVP.Key);




            RunTerminalCommands(_command);


        }



        //print uploading done????




    }






    static void RunTerminalCommands(string commands)
    {

        UnityEngine.Debug.Log("running the command: '" + commands+"'");

        ProcessStartInfo processInfo = new ProcessStartInfo("bash", "-c \"" + commands + "\"");
        processInfo.RedirectStandardOutput = true;
        processInfo.RedirectStandardError = true;
        processInfo.UseShellExecute = false;
        processInfo.CreateNoWindow = true;

        Process process = new Process();
        process.StartInfo = processInfo;
        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();


        UnityEngine.Debug.Log("command line output: " + output);
        if(error != "")
            UnityEngine.Debug.LogError("command line error: " + error);

    }









}

#endif
