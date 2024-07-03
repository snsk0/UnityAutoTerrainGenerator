using UnityEngine;

namespace UniPTG.HeightmapGenerators
{
    internal class RidgeAndLoad : GeneratorRidge
    {
        [SerializeField]
        private float _threshold;

        [SerializeField]
        private float _minloadAmplitude;

        [SerializeField]
        private float _maxloadAmplitude;

        private protected override float HeightProcessing(float minAmplitude, float maxAmplitude, float value)
        {
            //è‡’lˆÈ‰º‚Ìê‡“¹‚É‚·‚é
            if(_threshold > value)
            {
                return Mathf.LinearScaling(value, minAmplitude, maxAmplitude, _minloadAmplitude, _maxloadAmplitude);
            }
            return value;
        }
    }
}