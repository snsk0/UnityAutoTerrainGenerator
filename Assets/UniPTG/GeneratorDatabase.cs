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
        private static Dictionary<NoiseGeneratorBase, Editor> _noiseGeneratorToEditor;
        private static Dictionary<HeightmapGeneratorBase, Editor> _heightMapGeneratorToEditor;

        static GeneratorDatabase()
        {
            MonoScriptDatabase.instance.OnUpdateDatabase += () =>
            {
                Debug.Log("SaveAndLoad");
                SaveAndDispose();
                Load();
            };

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
            IReadOnlyList<Type> noiseGeneratorTypes = MonoScriptDatabase.instance.GetNoiseGeneratorTypes();
            IReadOnlyList<Type> heightmapGeneratorTypes = MonoScriptDatabase.instance.GetHeightmapGeneratorTypes();

            //TypeからインスタンスとEditorを生成
            _noiseGeneratorToEditor = noiseGeneratorTypes
                .Select((type) =>
                {
                    NoiseGeneratorBase generator = ScriptableObject.CreateInstance(type) as NoiseGeneratorBase;

                    //UserSettingsからパラメータを読み込む
                    string json = EditorUserSettings.GetConfigValue(type.FullName);
                    if (!string.IsNullOrEmpty(json))
                    {
                        EditorJsonUtility.FromJsonOverwrite(json, generator);
                    }

                    //永続化する
                    generator.hideFlags = HideFlags.DontSave;
                    return generator;
                })
                .ToDictionary(generator => generator, generator => Editor.CreateEditor(generator));

            _heightMapGeneratorToEditor = heightmapGeneratorTypes
                .Select((type) =>
                {
                    HeightmapGeneratorBase generator = ScriptableObject.CreateInstance(type) as HeightmapGeneratorBase;

                    //UserSettingsからパラメータを読み込む
                    string json = EditorUserSettings.GetConfigValue(type.FullName);
                    if (!string.IsNullOrEmpty(json))
                    {
                        EditorJsonUtility.FromJsonOverwrite(json, generator);
                    }

                    //永続化する
                    generator.hideFlags = HideFlags.DontSave;
                    return generator;
                })
                .ToDictionary(generator => generator, generator => Editor.CreateEditor(generator));
        }

        private static void SaveAndDispose()
        {
            IEnumerable<NoiseGeneratorBase> noiseGenerators = _noiseGeneratorToEditor.Keys;
            IEnumerable<Editor> noiseEditors = _noiseGeneratorToEditor.Values;
            IEnumerable<HeightmapGeneratorBase> heightmapGenerators = _heightMapGeneratorToEditor.Keys;
            IEnumerable<Editor> heightmapEditors = _heightMapGeneratorToEditor.Values;

            //Editorを解放する
            foreach (Editor editor in noiseEditors)
            {
                UnityEngine.Object.DestroyImmediate(editor);
            }
            foreach (Editor editor in heightmapEditors)
            {
                UnityEngine.Object.DestroyImmediate(editor);
            }

            //Generatorを保存してから解放する
            foreach(NoiseGeneratorBase generator in noiseGenerators)
            {
                string json = EditorJsonUtility.ToJson(generator);
                EditorUserSettings.SetConfigValue(generator.GetType().FullName, json);

                UnityEngine.Object.DestroyImmediate(generator);
            }
            foreach(HeightmapGeneratorBase generator in heightmapGenerators)
            {
                string json = EditorJsonUtility.ToJson(generator);
                EditorUserSettings.SetConfigValue(generator.GetType().FullName, json);

                UnityEngine.Object.DestroyImmediate(generator);
            }

            //データを解放
            _noiseGeneratorToEditor.Clear();
            _noiseGeneratorToEditor = null;
            _heightMapGeneratorToEditor.Clear();
            _heightMapGeneratorToEditor = null;
        }

        internal static IReadOnlyList<HeightmapGeneratorBase> GetHeightmapGenerators()
        {
            return _heightMapGeneratorToEditor.Keys.ToList().AsReadOnly();
        }

        internal static IReadOnlyList<NoiseGeneratorBase> GetNoiseGenerators()
        {
            return _noiseGeneratorToEditor.Keys.ToList().AsReadOnly();
        }

        internal static Editor GetNoiseEditor(NoiseGeneratorBase generator)
        {
            return _noiseGeneratorToEditor[generator];
        }

        internal static Editor GetHeightmapEditor(HeightmapGeneratorBase generator)
        {
            return _heightMapGeneratorToEditor[generator];
        }
    }
}
#endif