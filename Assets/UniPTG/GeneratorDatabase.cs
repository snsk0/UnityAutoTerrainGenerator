#if UNITY_EDITOR
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UniPTG
{
    [InitializeOnLoad]
    internal static class GeneratorDatabase
    {
        private static Dictionary<HeightmapGeneratorBase, Editor> _heightMapGeneratorToEditor;

        static GeneratorDatabase()
        {
            AssemblyReloadEvents.beforeAssemblyReload += () =>
            {
                Debug.Log("SaveAndDispose");
                SaveAndDispose();
            };


            AssemblyReloadEvents.afterAssemblyReload += () => 
            {
                Debug.Log("Load");
                Load();
            };
        }

        private static void Load()
        {
            //MonoScriptDatabaseからデータを取得
            IReadOnlyList<Type> activeHeightmapTypes = MonoScriptDatabase.instance.GetHeightmapGeneratorTypes();

            //TypeからインスタンスとEditorを生成
            _heightMapGeneratorToEditor = activeHeightmapTypes
                .Select((type) =>
                {
                    HeightmapGeneratorBase generator = ScriptableObject.CreateInstance(type) as HeightmapGeneratorBase;

                    //UserSettingsからパラメータを読み込む
                    string json = EditorUserSettings.GetConfigValue(type.FullName);
                    if (!string.IsNullOrEmpty(json))
                    {
                        JsonUtility.FromJsonOverwrite(json, generator);
                    }

                    //永続化する
                    generator.hideFlags = HideFlags.DontSave;
                    return generator;
                })
                .ToDictionary(generator => generator, generator => Editor.CreateEditor(generator));
        }

        private static void SaveAndDispose()
        {
            IEnumerable<HeightmapGeneratorBase> generators = _heightMapGeneratorToEditor.Keys;
            IEnumerable<Editor> editors = _heightMapGeneratorToEditor.Values;

            //データを解放
            _heightMapGeneratorToEditor.Clear();
            _heightMapGeneratorToEditor = null;

            //Editorを解放する
            foreach(Editor editor in editors)
            {
                UnityEngine.Object.DestroyImmediate(editor);
            }

            //Generatorを保存してから解放する
            foreach(HeightmapGeneratorBase generator in generators)
            {
                string json = JsonUtility.ToJson(generator);
                EditorUserSettings.SetConfigValue(generator.GetType().FullName, json);

                UnityEngine.Object.Destroy(generator);
            }
        }

        internal static IReadOnlyList<HeightmapGeneratorBase> GetHeightmapGenerators()
        {
            return _heightMapGeneratorToEditor.Keys.ToList().AsReadOnly();
        }

        internal static Editor GetEditorByGenerator(HeightmapGeneratorBase generator)
        {
            return _heightMapGeneratorToEditor[generator];
        }
    }
}
#endif