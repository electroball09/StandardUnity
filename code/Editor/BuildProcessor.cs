using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.IO;

public class BuildProcessor : IPostprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPostprocessBuild(BuildReport report)
    {
        if (report.summary.result != BuildResult.Succeeded) return;

        string outputPath = report.summary.outputPath + "/";
        string sourcePath = Directory.GetCurrentDirectory() + "/";

        Debug.LogFormat("Copying Config file from {0} to {1}", sourcePath, outputPath);
    }
}
