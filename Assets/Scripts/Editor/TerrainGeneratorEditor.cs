using UnityEngine;
using UnityEditor;

/// <Summary>
/// エディタ上でTerrainのHeightMapを生成するクラスです。
/// </Summary>
[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var generator = target as TerrainGenerator;
        DrawDefaultInspector();

        EditorGUILayout.Space();
        if (GUILayout.Button("フィールドの生成"))
        {
            GenerateTerrainOnEditor(generator);
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("リセット"))
        {
            ResetHeightMap(generator);
        }
    }

    /// <Summary>
    /// 設定に応じてTerrainのHeightMapを生成します。
    /// </Summary>
    void GenerateTerrainOnEditor(TerrainGenerator generator)
    {
        // Terrainの解像度を取得します。
        int terrainSizeX = generator.terrainData.heightmapResolution;
        int terrainSizeZ = generator.terrainData.heightmapResolution;

        // Terrainにセットするハイトマップを作成します。
        float[,] newHeightMap = new float[terrainSizeX, terrainSizeZ];

        for (int z = 0; z < terrainSizeZ; z++)
        {
            for (int x = 0; x < terrainSizeX; x++)
            {
                // パーリンノイズの座標を指定して値を取得します。
                float xValue = (generator.xOrigin + x) * generator.scale;
                float yValue = (generator.yOrigin + z) * generator.scale;
                float perlinValue = GetMultiplePerlinNoise(xValue, yValue, terrainSizeX, terrainSizeZ, generator);
                float height = generator.heightMultiply * perlinValue;

                // HeightMapに値をセットします。
                newHeightMap[x, z] = height;
            }
        }

        // 生成したHeightMapをセットします。
        generator.terrainData.SetHeights(0, 0, newHeightMap);
    }

    /// <Summary>
    /// 複数のパーリンノイズをかけ合わせるメソッドです。
    /// </Summary>
    float GetMultiplePerlinNoise(float xPos, float yPos, int terrainSizeX, int terrainSizeZ, TerrainGenerator generator)
    {
        // 乱数のシードを設定します。
        Random.InitState(generator.seed);

        // パーリンノイズの値を指定回数だけ乗算します。
        float perlinValue = Mathf.PerlinNoise(xPos, yPos);
        for (int i = 0; i < generator.multipleTimes; i++)
        {
            float offsetValue = Random.value;
            float xOffset = offsetValue * terrainSizeX;
            float yOffset = offsetValue * terrainSizeZ;
            float scaleOffset = Random.Range(0.8f, 1.25f);

            float xValue = (xPos + xOffset) * scaleOffset;
            float yValue = (yPos + yOffset) * scaleOffset;
            float value = Mathf.PerlinNoise(xValue, yValue);
            perlinValue *= Mathf.Clamp01(value);
        }

        return perlinValue;
    }

    /// <Summary>
    /// TerrainのHeightMapを初期化します。
    /// </Summary>
    void ResetHeightMap(TerrainGenerator generator)
    {
        int terrainSizeX = generator.terrainData.heightmapResolution;
        int terrainSizeZ = generator.terrainData.heightmapResolution;

        float[,] newHeightMap = new float[terrainSizeX, terrainSizeZ];
        generator.terrainData.SetHeights(0, 0, newHeightMap);
    }
}