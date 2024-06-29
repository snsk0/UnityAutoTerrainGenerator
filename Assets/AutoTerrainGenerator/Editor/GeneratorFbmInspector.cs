#if UNITY_EDITOR
using UnityEditor;
using AutoTerrainGenerator.HeightMapGenerators;

namespace AutoTerrainGenerator.Editors
{
    [CustomEditor(typeof(GeneratorFbm))]
    public class GeneratorFbmInspector : Editor
    {
        private void OnEnable()
        {
            SharedDefaultInspector.OnEnable(serializedObject, GetType());
        }

        private void OnDisable()
        {
            SharedDefaultInspector.OnDisable(serializedObject, GetType());
        }

        public override void OnInspectorGUI()
        {
            SharedDefaultInspector.OnInspectorGUI(serializedObject);
        }
    }
}
#endif