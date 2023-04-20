using UnityEngine;

/// <Summary>
/// Terrainのハイトマップ(HeightMap)を生成するクラスです。
/// </Summary>
public class TerrainGenerator : MonoBehaviour
{
    // 高さを変更するTerrainのデータをアサインします。
    public TerrainData terrainData;

    // パーリンノイズに関する情報です。
    public float xOrigin;
    public float yOrigin;
    public float scale = 0.03f;

    // 高さの補正値です。
    public float heightMultiply = 1f;

    // パーリンノイズのオフセットで使用する乱数のシードです。
    public int seed = 0;

    // パーリンノイズを重ね合わせる回数を設定します。
    public int multipleTimes = 2;
}