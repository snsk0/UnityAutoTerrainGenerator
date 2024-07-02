using System;
using UnityEngine;

namespace UniPTG
{
    [Serializable]
    public class TerrainParameters
    {
        public int resolutionExp = (Mathf.MinResolutionExp + Mathf.MaxResolutionExp) / 2;
        public Vector3 scale = new Vector3(1000, 600, 1000);

        public int resolution => Mathf.GetResolution(resolutionExp);
    }
}
