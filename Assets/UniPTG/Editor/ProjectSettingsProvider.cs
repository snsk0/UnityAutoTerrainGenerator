#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace UniPTG.Editors
{
    internal class ProjectSettingsProvider : SettingsProvider
    {
        [SettingsProvider]
        internal static SettingsProvider CreateSettingProvider()
        {
            return new ProjectSettingsProvider("Project/", SettingsScope.Project, new[] { "Procedural Terrain Generator", "UniPTG" });
        }

        internal ProjectSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
            label = "Procedural Terrain Generator";
        }

        private SerializedObject _serializedObject;

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            _serializedObject = new SerializedObject(MonoScriptDatabase.instance);
        }

        public override void OnGUI(string searchContext)
        {
            _serializedObject.Update();

            EditorGUILayout.PropertyField(_serializedObject.FindProperty("_noiseGenerators"));
            EditorGUILayout.PropertyField(_serializedObject.FindProperty("_heightmapGenerators"));

            _serializedObject.ApplyModifiedProperties();

            MonoScriptDatabase.instance.Update();
        }
    }
}
#endif
