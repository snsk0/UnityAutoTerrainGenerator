using UnityEngine;

namespace UniPTG
{
    public static class TerrainGenerator
    {
        public static TerrainData Generate(float[,] heigtMap, Vector3 scale)
        {
            TerrainData terrainData = new TerrainData();

            terrainData.heightmapResolution = heigtMap.GetLength(0);
            terrainData.size = scale;
            terrainData.SetHeights(0, 0, heigtMap);

            Terrain.CreateTerrainGameObject(terrainData);

            return terrainData;
        }
    }
}