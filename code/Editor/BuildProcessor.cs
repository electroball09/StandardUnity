﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.IO;

public class BuildProcessor : IPostprocessBuildWithReport
{
    public int callbackOrder => 1;

    public void OnPostprocessBuild(BuildReport report)
    {
        //if (report.summary.result != BuildResult.Succeeded) return;

        if (report.summary.platform != BuildTarget.StandaloneWindows64)
            return;

        string outputPath = new FileInfo(report.summary.outputPath).Directory.FullName + "/Config/";
        string sourcePath = Directory.GetCurrentDirectory() + "/Config/";

        if (!Directory.Exists(sourcePath))
            Directory.CreateDirectory(sourcePath);
        if (Directory.Exists(outputPath))
            Directory.Delete(outputPath);

        Debug.LogFormat("Copying Config file from {0} to {1}", sourcePath, outputPath);

        FileUtil.CopyFileOrDirectory(sourcePath, outputPath);
    }
}
