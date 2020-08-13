using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.TestTools.TestRunner.Api;
using System.Text;
using NUnit.Framework.Interfaces;
using System.Runtime.CompilerServices;
using System;
using UnityEditorInternal;

namespace CI
{
    [InitializeOnLoad]
    public static class TestLogReporter
    {
        static string[] testMode = new string[]
        {
            "None",
            "PlayMode",
            "EditMode"
        };

        static TestRunnerApi api;

        static TestLogReporter()
        {
            api = ScriptableObject.CreateInstance<TestRunnerApi>();
            SetupListeners(api);
        }

        public static void Run()
        {
            ExecuteTest(api, new Filter() { testMode = TestMode.PlayMode });
            ExecuteTest(api, new Filter() { testMode = TestMode.EditMode });
        }

        public static void SetupListeners(TestRunnerApi api)
        {
            api.RegisterCallbacks(new TestCallbacks());
        }
        
        public static void ExecuteTest(TestRunnerApi api, Filter filter)
        {
            api.Execute(new ExecutionSettings(filter));
        }

        private class TestCallbacks : ICallbacks
        {
            StringBuilder log;

            public void RunStarted(ITestAdaptor testsToRun)
            {
                log = new StringBuilder();
            }
            
            public void RunFinished(ITestResultAdaptor result)
            {
                Builder.TestFlag++;
                if (result.FailCount > 0)
                {
                    Debug.LogError($"Failed test count : {result.FailCount}");
                    System.IO.File.WriteAllText($@"{Builder.builderSetting.testLogPath}/{testMode[Builder.TestFlag]}test.log", log.ToString(), Encoding.UTF8);
                    if (Builder.TestFlag > 1 && Builder.builderSetting.postTestOption == BuilderSetting.PostTestOption.CancelBuildWhenFailed)
                    {
                        Debug.LogError("Build canceled");
                        return;
                    }
                }
                if (Builder.TestFlag > 1)
                {
                    Builder.TestFlag = 0;
                    Builder.BuildWithCustomSetting();
                }
            }

            public void TestStarted(ITestAdaptor test)
            {

            }

            public void TestFinished(ITestResultAdaptor result)
            {
                if (!result.HasChildren && result.ResultState.Equals("Failed"))
                {
                    log.Append(
                        $"[{result.Test.Name}] : {result.ResultState}\n{result.Message}\n");             
                }
            }

            private void CreateLogFile()
            {
                /*
                sb.Append($"Build failed: {msg}\n");
                sb.Append($"complete time: {DateTime.Now}\n");
                System.IO.File.WriteAllText(buildInfo.logPath, textValue, Encoding.UTF8);*/
            }

            private string WrapLog()
            {
                return "";
            }
        }
    }
}

