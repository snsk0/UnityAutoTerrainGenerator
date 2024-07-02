namespace UniPTG.HeightmapGenerators
{
    internal class GeneratorFbm : DefaultHeightmapGeneratorBase
    {
        private protected override float CalculateHeight(float currentAmplitude, float value)
        {
            return currentAmplitude * value;
        }
    }
}