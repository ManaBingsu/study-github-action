using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;

namespace CI
{
    public static class Builder
    {
        static string EOL = Environment.NewLine;
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

        // This function is testing, Don't use now
        public static void BuildWithCommandLine()
        {
            // Update Resource
            Resource resource = AssetDatabase.LoadAssetAtPath("Assets/Resources/Resource.asset", typeof(Resource)) as Resource;
            if (resource != null && resource is Resource)
            {
                Console.WriteLine(
                $"{EOL}" +
                $"###########################{EOL}" +
                $"#     Resource loaded     #{EOL}" +
                $"###########################{EOL}" +
                $"{EOL}"
                );
                resource.Load();
            }

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

        public static void TestBeforeBuild()
        {

            // Step 1 : Resource load
            Resource resource = builderSetting.preResources.resources[(int)PreResources.ResourceType.Resource] as Resource;
            resource.Load();

            // Step 2 : Test
            TestLogReporter.Run();
        }

        public static void BuildWithCustomSetting()
        {
            // Step 0 : Set path
            StringBuilder path = new StringBuilder();
            path.Append(builderSetting.buildPath);
            if (path.Length == 0)
            {
                return;
            }

            string exStr = extension.ContainsKey(builderSetting.buildTarget) ?
                extension[builderSetting.buildTarget] : "";
            path.Append($@"/{builderSetting.fileName}{exStr}");

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
            BuildReport report = BuildPipeline.BuildPlayer(buildOptions);

            string log = report.summary.result == BuildResult.Succeeded ? MakeBuildLog(true, report) : MakeBuildLog(false, report);

            System.IO.File.WriteAllText($"{path}.log", log, Encoding.UTF8);
            /*
            // Summary
            BuildSummary summary = buildReport.summary;
            StdOutReporter.ReportSummary(summary);

            // Result
            BuildResult result = summary.result;
            StdOutReporter.ExitWithResult(result);*/
        }

        public static string MakeBuildLog(bool isSuceeded, BuildReport report)
        {
            StringBuilder sb = new StringBuilder();
            if (isSuceeded)
            {
                sb.Append($"Build succeeded: \n");
                sb.Append($"start time: {report.summary.buildStartedAt}\n");
                sb.Append($"end time: {report.summary.buildEndedAt}\n");
                sb.Append($"total time: {report.summary.totalTime}\n");
            }
            else
            {
                sb.Append($"Build failed:\n");
                sb.Append($"start time: {report.summary.buildStartedAt}\n");
                sb.Append($"end time: {report.summary.buildEndedAt}\n");
            }
            for (int i = 0; i < report.steps.Length; i++)
            {
                for (int j = 0; j < report.steps[i].messages.Length; j++)
                {
                    sb.Append($"{report.steps[i].messages[j].content}\n");
                }
            }
            return sb.ToString();
        }
    
        [PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            Debug.Log($"[{target}] Build with test is finished");
            EditorUtility.RevealInFinder(pathToBuiltProject);
        }
    }
}

