namespace UniPTG.HeightmapGenerators
{
    internal class GeneratorTurbulence : DefaultHeightmapGeneratorBase
    {
        private protected override float CalculateHeight(float currentAmplitude, float value)
        {
            //•„†•t‚«‚Ì’l‚É•ÏŠ·‚·‚é
            value = (value - 0.5f) * 2.0f;

            //â‘Î’l‚ğæ“¾
            value = UnityEngine.Mathf.Abs(value) * currentAmplitude;
            return value;
        }
    }
}
