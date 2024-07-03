#if UNITY_EDITOR
using UniPTG.HeightmapGenerators;
using UnityEditor;

namespace UniPTG.Editors
{
    [CustomEditor(typeof(RidgeAndLoad))]
    public class RidgeOnLoadInspector : DefaultGeneratorBaseInspector
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            base.OnInspectorGUI();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_threshold"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_minloadAmplitude"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_maxloadAmplitude"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
