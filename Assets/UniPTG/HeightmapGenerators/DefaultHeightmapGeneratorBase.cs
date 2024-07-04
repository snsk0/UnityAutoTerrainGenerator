using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniPTG.Parameters;

namespace UniPTG.HeightmapGenerators
{
    internal abstract class DefaultHeightmapGeneratorBase : HeightmapGeneratorBase
    {
        [SerializeField]
        private protected HeightmapGenerationParameter _param;

        [SerializeField]
        private protected HeightmapGenerationParamObject _inputParam;

        public override void Generate(float[,] heightmap, int size, INoiseReader noiseReader)
        {
            //入力値がある場合はそちらを使用する
            HeightmapGenerationParameter param = _param;
            if (_inputParam != null)
            {
                param = _inputParam.parameter;
            }

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
                        float xvalue = (float)x / size * frequency;
                        float yvalue = (float)y / size * frequency;
                        heightmap[x, y] += CalculateHeight(amplitude, noiseReader.GetValue(xvalue, yvalue));
                    }
                }

                frequency *= Mathf.FBmFrequencyRate;
                amplitude *= Mathf.FBmPersistence;

                //ノイズ状態の更新
                noiseReader.UpdateState();
            }

            //高さの最大値と最小値を取得する
            IEnumerable<float> heightEnum = heightmap.Cast<float>();
            float minHeight = heightEnum.Min();
            float maxHeight = heightEnum.Max();

            //スケーリング
            if (param.isLinearScaling)
            {
                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        heightmap[x, y] = Mathf.LinearScaling(heightmap[x, y], minHeight, maxHeight, param.minLinearScale, param.maxLinearScale);
                    }
                }
            }

            for(int x = 0; x < size; x++)
            {
                for(int y = 0; y < size; y++)
                {
                    if (param.isLinearScaling)
                    {
                        heightmap[x, y] = HeightProcessing(param.minLinearScale, param.maxLinearScale, heightmap[x, y]);
                    }
                    else
                    {
                        heightmap[x, y] = HeightProcessing(minHeight, maxHeight, heightmap[x, y]);
                    }
                }
            }
        }

        private protected abstract float CalculateHeight(float currentAmplitude, float value);

        private protected virtual float HeightProcessing(float minAmplitude, float maxAmplitude, float value)
        {
            return value;
        }
    }
}
