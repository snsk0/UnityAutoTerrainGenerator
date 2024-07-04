using System;
using UnityEngine;

namespace UniPTG.Parameters
{
    [Serializable]
    internal struct HeightmapGenerationParameter
    {
        public float frequency;
        public bool isLinearScaling;
        public float amplitude;
        public float minLinearScale;
        public float maxLinearScale;
        public int octaves;
    }

    internal class HeightmapGenerationParamObject : ScriptableObject
    {
        [SerializeField]
        internal HeightmapGenerationParameter parameter;
    }
}
