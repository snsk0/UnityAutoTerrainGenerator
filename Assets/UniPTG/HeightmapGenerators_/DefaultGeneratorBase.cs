using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UniPTG.Parameters;

namespace UniPTG.HeightmapGenerators
{
    public abstract class DefaultGeneratorBase : HeightmapGeneratorBase
    {
        [SerializeField]
        private protected HeightmapGenerationParam _param;

        [SerializeField]
        private protected HeightmapGenerationParam _inputParam;

        private void OnEnable()
        {
            //インスタンス化
            _param = CreateInstance<HeightmapGenerationParam>();

            //専用フォルダを取得
            string path = Application.dataPath.Replace("Assets", "UserSettings/") + "UniPTG";

            //ない場合作る
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //ファイル名を追加
            path += "/" + GetType().FullName + ".json";


            //ファイルがないなら作成する
            if (!File.Exists(path))
            {
                Save(path);
            }

            //jsonを取得する
            StreamReader reader = new StreamReader(path);
            string json = reader.ReadToEnd();
            reader.Close();

            //jsonがある場合上書きする
            if (!string.IsNullOrEmpty(json))
            {
                JsonUtility.FromJsonOverwrite(json, _param);
            }

            //永続化する
            _param.hideFlags = HideFlags.DontSave;
        }

        private void OnDisable()
        {
            //セーブする
            Save(Application.dataPath.Replace("Assets", "UserSettings/") + "UniPTG/" + GetType().FullName + ".json");
        }

        private void Save(string path)
        {
            //Jsonに変換して書き込み
            string json = JsonUtility.ToJson(_param);
            StreamWriter writer = new StreamWriter(path, false);
            writer.Write(json);
            writer.Close();
        }

        public override float[,] Generate(int size)
        {
            //入力値がある場合はそちらを使用する
            HeightmapGenerationParam param = _param;
            if (_inputParam != null)
            {
                param = _inputParam;
            }

            Random.InitState(param.seed);
            float xSeed = Random.Range(0f, 256);
            float ySeed = Random.Range(0f, 256);

            float[,] heightMap = new float[size, size];

            float frequency = param.frequency;
            float amplitude = param.amplitude;

            if (param.isLinearScaling)
            {
                amplitude = Mathf.MaxTerrainHeight;
            }

            for (int i = 0; i <= param.octaves; i++)
            {
                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        float xvalue = (float)x / size * frequency + xSeed;
                        float yvalue = (float)y / size * frequency + ySeed;
                        //heightMap[x, y] += CalculateHeight(noiseReader, amplitude, xvalue, yvalue);
                    }
                }

                frequency *= Mathf.FBmFrequencyRate;
                amplitude *= Mathf.FBmPersistence;
            }

            //スケーリング
            if (param.isLinearScaling)
            {
                IEnumerable<float> heightEnum = heightMap.Cast<float>();
                float minHeight = heightEnum.Min();
                float maxHeight = heightEnum.Max();

                for (int x = 0; x < heightMap.GetLength(0); x++)
                {
                    for (int y = 0; y < heightMap.GetLength(1); y++)
                    {
                        heightMap[x, y] = Mathf.LinearScaling(heightMap[x, y], minHeight, maxHeight, param.minLinearScale, param.maxLinearScale);
                    }
                }
            }
            return heightMap;
        }

        //protected abstract float CalculateHeight(INoiseReader noiseReader,float currentAmplitude, float xvalue, float yvalue);
    }
}
