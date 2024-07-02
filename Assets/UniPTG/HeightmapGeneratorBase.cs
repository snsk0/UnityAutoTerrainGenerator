using UnityEngine;

namespace UniPTG
{
    public abstract class HeightmapGeneratorBase : ScriptableObject
    {
        public abstract float[,] Generate(int size);
    }
}
