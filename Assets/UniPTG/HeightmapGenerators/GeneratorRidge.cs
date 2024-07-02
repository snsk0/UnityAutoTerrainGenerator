namespace UniPTG.HeightmapGenerators
{
    internal class GeneratorRidge : DefaultHeightmapGeneratorBase
    {
        private protected override float CalculateHeight(float currentAmplitude, float value)
        {
            //符号付きの値に変換する
            value = (value - 0.5f) * 2.0f;

            //絶対値を取得
            value = UnityEngine.Mathf.Abs(value) * currentAmplitude;

            //offから判定して値を返す
            value = Mathf.RidgeOffset - value;
            value *= value;
            return value;
        }
    }
}
