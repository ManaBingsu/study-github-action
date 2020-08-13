using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace CI
{
    public class Prebuilder : IPreprocessBuildWithReport
    {
        public int callbackOrder => 1;

        public void OnPreprocessBuild(BuildReport report)
        {
            // RunPreBuildEvent();
        }

        public void RunPreBuildEvent()
        {
            TestLogReporter.Run();
        }
    }
}
