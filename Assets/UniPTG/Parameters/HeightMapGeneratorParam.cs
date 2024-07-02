using UnityEngine;

namespace UniPTG.Parameters
{
    internal class HeightMapGeneratorParam : ScriptableObject
    {
        //ƒmƒCƒY•Ï”
        public int noiseTypeIndex = 0;
        public int seed = 0;
        public float frequency = 0;
        public bool isLinearScaling = false;
        public float amplitude = Mathf.MaxTerrainHeight;
        public float minLinearScale = (Mathf.MinTerrainHeight + Mathf.MaxTerrainHeight) / 2;
        public float maxLinearScale = (Mathf.MinTerrainHeight + Mathf.MaxTerrainHeight) / 2;
        public int octaves = 0;
    }
}
