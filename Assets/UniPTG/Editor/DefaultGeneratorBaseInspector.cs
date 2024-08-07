#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UniPTG.HeightmapGenerators;
using UniPTG.Parameters;

namespace UniPTG.Editors
{
    [CustomEditor(typeof(DefaultHeightmapGeneratorBase), true)]
    public class DefaultGeneratorBaseInspector : Editor
    {
        public override void OnInspectorGUI ()
        {
            serializedObject.Update();

            //パラメータオブジェクトを取得
            HeightmapGenerationParameter param = (HeightmapGenerationParameter)serializedObject.FindProperty("_param").boxedValue;

            //設定値の読み込み
            SerializedProperty inputProperty = serializedObject.FindProperty("_inputParam");
            EditorGUILayout.PropertyField(inputProperty, new GUIContent("入力", "HeightMapParamを入力します"));
            if (inputProperty.objectReferenceValue != null)
            {
                GUI.enabled = false;

                //設定値の上書き
                param = (inputProperty.objectReferenceValue as HeightmapGenerationParamObject).parameter;
            }

            param.frequency = EditorGUILayout.FloatField(new GUIContent("周波数", "使用するノイズの周波数を設定します"), param.frequency);
            MessageType type = MessageType.Info;
            if (param.frequency > 256)
            {
                type = MessageType.Warning;
            }
            EditorGUILayout.HelpBox("UnityEngine.Mathf.PerlinNoiseの周期は256なため\n256以上の数値にすると同様の地形が現れる可能性があります", type);

            param.isLinearScaling = EditorGUILayout.Toggle(new GUIContent("線形スケーリング", "線形スケーリングを有効化します"), param.isLinearScaling);

            if (!param.isLinearScaling)
            {
                param.amplitude = EditorGUILayout.Slider(new GUIContent("振幅", "生成するHeightMapの振幅を設定します"),
                    param.amplitude, Mathf.MinTerrainHeight, Mathf.MaxTerrainHeight);
            }
            else
            {
                EditorGUILayout.MinMaxSlider(new GUIContent("スケール範囲", "生成するHeightMapのスケール範囲を設定します"),
                    ref param.minLinearScale, ref param.maxLinearScale, Mathf.MinTerrainHeight, Mathf.MaxTerrainHeight);

                bool guiEnableTemp = GUI.enabled;
                GUI.enabled = false;
                EditorGUILayout.FloatField(new GUIContent("最低値", "振幅の最低値を表示します"), param.minLinearScale);
                EditorGUILayout.FloatField(new GUIContent("最高値", "振幅の最高値を表示します"), param.maxLinearScale);
                EditorGUILayout.FloatField(new GUIContent("振幅", "振幅の値を表示します"), param.maxLinearScale - param.minLinearScale);
                GUI.enabled = guiEnableTemp;
            }

            if (param.octaves > 0 && param.maxLinearScale == Mathf.MaxTerrainHeight)
            {
                EditorGUILayout.HelpBox("オクターブを利用する場合、振幅を1未満に設定してください\n地形が正しく生成されません\n0.5が推奨されます", MessageType.Error);
            }

            param.octaves = EditorGUILayout.IntField(new GUIContent("オクターブ", "非整数ブラウン運動を利用してオクターブの数値の回数ノイズを重ねます"), param.octaves);

            if(inputProperty.objectReferenceValue == null)
            {
                //入力がないとき保存する
                serializedObject.FindProperty("_param").boxedValue = param;
            }
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button(new GUIContent("設定値を出力する", "設定値をアセットファイルに保存します")))
            {
                string savePath = EditorUtility.SaveFilePanelInProject("Save", "parameters", "asset", "");
                if (!string.IsNullOrEmpty(savePath))
                {
                    //値をコピーする
                    HeightmapGenerationParamObject outputParam = CreateInstance<HeightmapGenerationParamObject>();
                    outputParam.parameter = param;

                    //出力する
                    AssetDatabase.CreateAsset(outputParam, savePath);
                }
            }

            if (inputProperty.objectReferenceValue != null)
            {
                GUI.enabled = true;
            }
        }
    }
}
#endif
