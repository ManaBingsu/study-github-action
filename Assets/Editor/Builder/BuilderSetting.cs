using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CI
{
    [CreateAssetMenu]
    public class BuilderSetting : ScriptableObject
    { 
        [Header("Mode")]
        public BuildTarget buildTarget = BuildTarget.StandaloneWindows64;

        public enum PostTestOption
        {
            CancelBuildWhenFailed,
            None
        }

        public PostTestOption postTestOption;
        [Header("File name")]
        public string fileName;

        [Header("Path")]
        public string buildPath;
        public string testLogPath;
        public string buildLogPath;

        [Header("Resources")]
        [Tooltip("Element 0 : Resource")]
        [SerializeField]
        List<Object> resources;

        [HideInInspector]
        public PreResources preResources;

        BuilderSetting()
        {
            Builder.builderSetting = this;
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(BuilderSetting))]
        class Editor : UnityEditor.Editor
        {
            BuilderSetting _target;
            new BuilderSetting target => _target ?? (_target = (BuilderSetting)base.target);

            public override void OnInspectorGUI()
            {
                if (GUILayout.Button("Build with test"))
                    target.Build();

                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                if (GUILayout.Button("Set build path"))
                {
                    SetPath(ref target.buildPath, "Choose Location of Build Path");
                }

                if (GUILayout.Button("Set test log path"))
                {
                    SetPath(ref target.testLogPath, "Choose Location of Test Log Path");
                }

                if (GUILayout.Button("Set build log path"))
                {
                    SetPath(ref target.buildLogPath, "Choose Location of Build Log Path");
                }

                base.OnInspectorGUI();
            }

            public void SetPath(ref string path, string selecterMessage)
            {
                string selectPath = EditorUtility.SaveFolderPanel(selecterMessage, "", "");
                if (selectPath.Length == 0)
                {
                    return;
                }
                path = selectPath;
            }
        }
#endif
        public void Build()
        {
            preResources = new PreResources(resources);
            Builder.TestBeforeBuild();
        }
    }

    public class PreResources
    {
        public enum ResourceType
        {
            Resource,
            //AutoBake
        }

        public List<Object> resources;

        public PreResources(List<Object> resources)
        {
            this.resources = resources;
        }
    }
}
