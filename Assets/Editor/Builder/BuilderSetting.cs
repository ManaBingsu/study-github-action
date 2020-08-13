using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CI
{
    [CreateAssetMenu]
    public class BuilderSetting : ScriptableObject
    { 
        BuilderSetting()
        {
            Builder.builderSetting = this;
        }

        [Header("Custom option")]
        public BuildTarget buildTarget;

        public enum PostTestOption
        {
            CancelBuildWhenFailed,
            None
        }

        public PostTestOption postTestOption;

        [Header("Resources")]
        [Tooltip("Element 0 : Resource")]
        [SerializeField]
        List<Object> resources;

        [HideInInspector]
        public PreResources preResources;

#if UNITY_EDITOR
        [CustomEditor(typeof(BuilderSetting))]
        class Editor : UnityEditor.Editor
        {
            BuilderSetting _target;
            new BuilderSetting target => _target ?? (_target = (BuilderSetting)base.target);

            public override void OnInspectorGUI()
            {
                if (GUILayout.Button("Build for CI"))
                    target.Build();

                base.OnInspectorGUI();
            }
        }
#endif
        [MenuItem("CI/BuildWithTest")]
        public void Build()
        {
            preResources = new PreResources(resources);
            Builder.Test();
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
