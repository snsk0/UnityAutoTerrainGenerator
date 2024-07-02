using UnityEngine;

namespace UniPTG
{
    public abstract class HeightmapGeneratorBase : ScriptableObject
    {
        public abstract void Generate(float[,] heightmap, int size, INoiseReader noiseReader);
    }
}
