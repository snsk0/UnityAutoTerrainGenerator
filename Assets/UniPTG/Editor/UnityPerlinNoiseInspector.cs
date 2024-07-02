#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UniPTG.NoiseGenerators;

namespace UniPTG.Editors
{
    [CustomEditor(typeof(UnityPerlinNoise))]
    public class UnityPerlinNoiseInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SerializedProperty isTimeModeProperty = serializedObject.FindProperty("_isTimeMode");
            EditorGUILayout.PropertyField(isTimeModeProperty, new GUIContent("時間変化", "シード値の時間変化を有効にします"));

            if(!isTimeModeProperty.boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_seed"), new GUIContent("シード値", "シード値を設定します"));
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
