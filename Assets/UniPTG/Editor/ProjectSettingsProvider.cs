#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UniPTG.Utility;

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

        private class TempDisplayObject : ScriptableObject
        {
            public List<MonoScript> _noiseGenerators;
            public List<MonoScript> _heightmapGenerators;
        }

        private TempDisplayObject _tempDisplayObject;
        private SerializedObject _displaySerializedObject;
        private SerializedObject _dataBaseSerializedObject;

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            _tempDisplayObject = ScriptableObject.CreateInstance<TempDisplayObject>();
            _tempDisplayObject.hideFlags = HideFlags.DontSave;

            _displaySerializedObject = new SerializedObject(_tempDisplayObject);
            _dataBaseSerializedObject = new SerializedObject(MonoScriptDatabase.instance);

            //detabaseからコピー
            EditorUtil.OverrideSerializedArray(_dataBaseSerializedObject.FindProperty("_noiseGenerators"), _displaySerializedObject.FindProperty("_noiseGenerators"));
            EditorUtil.OverrideSerializedArray(_dataBaseSerializedObject.FindProperty("_heightmapGenerators"), _displaySerializedObject.FindProperty("_heightmapGenerators"));
        }

        public override void OnDeactivate()
        {
            Object.DestroyImmediate(_tempDisplayObject);
            _tempDisplayObject = null;
        }

        public override void OnGUI(string searchContext)
        {
            _displaySerializedObject.Update();

            SerializedProperty noisesProperty = _displaySerializedObject.FindProperty("_noiseGenerators");
            SerializedProperty heightmapsProperty = _displaySerializedObject.FindProperty("_heightmapGenerators");

            EditorGUILayout.PropertyField(noisesProperty);
            EditorGUILayout.PropertyField(heightmapsProperty);

            //指定サブクラス以外Nullに置き換え
            ReplaceNullExceptSub<NoiseGeneratorBase>(noisesProperty);
            ReplaceNullExceptSub<HeightmapGeneratorBase>(heightmapsProperty);

            _displaySerializedObject.ApplyModifiedProperties();

            //適用ボタン
            if (GUILayout.Button(new GUIContent("適用")))
            {
                //databaseに対してコピー
                EditorUtil.OverrideSerializedArray(noisesProperty, _dataBaseSerializedObject.FindProperty("_noiseGenerators"));
                EditorUtil.OverrideSerializedArray(heightmapsProperty, _dataBaseSerializedObject.FindProperty("_heightmapGenerators"));

                //データベースを更新
                MonoScriptDatabase.instance.Update();

                //Dialogを表示
                EditorUtility.DisplayDialog("UniPTG", "設定値を適用しました", "OK");
            }
        }

        /// <summary>
        /// 指定したサブクラスでないPropertyの場合Nullにする
        /// </summary>
        private void ReplaceNullExceptSub<T>(SerializedProperty property) where T : class
        {
            for (int i = 0; i < property.arraySize; i++)
            {
                MonoScript script = property.GetArrayElementAtIndex(i).objectReferenceValue as MonoScript;

                if(script == null)
                {
                    continue;
                }

                if (!script.GetClass().IsSubclassOf(typeof(T)))
                {
                    property.GetArrayElementAtIndex(i).objectReferenceValue = null;
                }
            }
        }
    }
}
#endif
