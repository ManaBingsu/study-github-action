using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CI
{
    [CreateAssetMenu]
    public class BuilderSetting : ScriptableObject
    { 
        [Header("Custom option")]
        public BuildTarget buildTarget = BuildTarget.StandaloneWindows64;

        public enum PostTestOption
        {
            CancelBuildWhenFailed,
            None
        }

        public PostTestOption postTestOption;

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

                if (GUILayout.Button("Set test log path"))
                {
                    string selectPath = EditorUtility.SaveFolderPanel("Choose Location of Test Log Path", "", "");
                    if (selectPath.Length == 0)
                    {
                        return;
                    }
                    target.testLogPath = selectPath;

                }

                if (GUILayout.Button("Set build log path"))
                {
                    string selectPath = EditorUtility.SaveFolderPanel("Choose Location of Build Log Path", "", "");
                    if (selectPath.Length == 0)
                    {
                        return;
                    }
                    target.buildLogPath = selectPath;
                }

                base.OnInspectorGUI();
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
