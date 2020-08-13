using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace CI
{
    public class Builder
    {
        static Dictionary<BuildTarget, string> extension = new Dictionary<BuildTarget, string>()
            {
                {BuildTarget.StandaloneWindows64, ".exe" },
                {BuildTarget.StandaloneWindows, ".exe" },
                {BuildTarget.StandaloneLinux64, ".x86_64" },
                {BuildTarget.StandaloneOSX, ".app" }
            };
        public static BuilderSetting builderSetting;
        static int testFlag;
        public static int TestFlag
        {
            get => testFlag;
            set => testFlag = value;
        }

        public static void BuildWithCommandLine()
        {
            // Update Resource
            Resource resource = (Resource)AssetDatabase.LoadAssetAtPath("Assets/Scripts/Resource.asset", typeof(Resource));
            resource.Load();
            
            // Gather values from args
            var options = ArgumentsParser.GetValidatedOptions();

            // Gather values from project
            var scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(s => s.path).ToArray();

            
            // Define BuildPlayer Options
            var buildOptions = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = options["customBuildPath"],
                target = (BuildTarget)Enum.Parse(typeof(BuildTarget), options["buildTarget"]),
            };

            // Perform build
            BuildReport buildReport = BuildPipeline.BuildPlayer(buildOptions);

            // Summary
            BuildSummary summary = buildReport.summary;
            StdOutReporter.ReportSummary(summary);

            // Result
            BuildResult result = summary.result;
            StdOutReporter.ExitWithResult(result);
        }

        public static void Test()
        {
            // Step 1 : Resource load
            Resource resource = builderSetting.preResources.resources[(int)PreResources.ResourceType.Resource] as Resource;
            resource.Load();

            // Step 2 : Test
            TestLogReporter.Run();
        }

        public static void BuildWithCustomSetting()
        {
            StringBuilder path = new StringBuilder();
            path.Append(EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", ""));

            if (path.Length == 0)
            {
                return;
            }
            path.Append($@"/EditorBuild{extension[builderSetting.buildTarget]}");

            // Gather values from project
            var scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(s => s.path).ToArray();

            // Define BuildPlayer Options
            var buildOptions = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = path.ToString(),
                target = builderSetting.buildTarget,
            };

            // Perform build
            BuildReport buildReport = BuildPipeline.BuildPlayer(buildOptions);
            /*
            // Summary
            BuildSummary summary = buildReport.summary;
            StdOutReporter.ReportSummary(summary);

            // Result
            BuildResult result = summary.result;
            StdOutReporter.ExitWithResult(result);*/
        }
    }
}

