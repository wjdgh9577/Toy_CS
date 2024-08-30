using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

#if UNITY_EDITOR
using UnityEditor;

public class TestClientEditor : Editor
{
    [MenuItem("Tools/TestClient")]
    static void BuildAndRun()
    {
        var buildOptions = new BuildPlayerOptions();
        buildOptions.scenes = GetScenes();
        buildOptions.targetGroup = BuildTargetGroup.Standalone;
        buildOptions.target = BuildTarget.StandaloneWindows64;
        buildOptions.options = BuildOptions.AutoRunPlayer;
        for (int i = 0; i < 1; i++)
        {
            buildOptions.locationPathName = GetLocationPath(i);
            BuildPipeline.BuildPlayer(buildOptions);
        }
    }

    static string[] GetScenes()
    {
        string[] scenes = new string[EditorBuildSettings.scenes.Length];

        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i] = EditorBuildSettings.scenes[i].path;
        }

        return scenes;
    }

    static string GetLocationPath(int num)
    {
        string path = "Builds";

        if (Directory.Exists(path) == false)
            Directory.CreateDirectory("Builds");
        path += $"/{BuildTarget.StandaloneWindows64}";

        if (Directory.Exists(path) == false)
            Directory.CreateDirectory(path);

        path += $"/{num}/TestClient_{num}.exe";

        return path;
    }
}

#endif