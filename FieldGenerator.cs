using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <Summary>
/// パーリンノイズを使ってフィールドを生成するクラスです。
/// </Summary>
public class FieldGenerator : MonoBehaviour
{
    // フィールド作成のためのパーツへの参照です。
    public GameObject fieldParts;

    // フィールドパーツの親オブジェクトのTransformです。
    public Transform fieldParent;

    // フィールドに関する情報です。
    public int fieldSizeX = 50;
    public int fieldSizeZ = 50;
    public int fieldHeight = 50;

    // パーリンノイズに関する情報です。
    public float xOrigin;
    public float yOrigin;
    public float scale = 0.01f;

    // 生成するオブジェクトのマテリアルリストです。
    public List<Material> matList;


    /// <Summary>
    /// 生成ボタンが押された時のメソッドです。
    /// </Summary>
    public void OnPressedGenerateButton()
    {
        GenerateFieldParts();
    }

    /// <Summary>
    /// フィールドを生成するメソッドです。
    /// </Summary>
    void GenerateFieldParts()
    {
        for (float z = 0f; z < fieldSizeZ; z++)
        {
            for (float x = 0f; x < fieldSizeX; x++)
            {
                // パーリンノイズの座標を指定して値を取得します。
                float xValue = xOrigin + x * scale;
                float yValue = yOrigin + z * scale;
                float perlinValue = Mathf.PerlinNoise(xValue, yValue);
                float height = fieldHeight * perlinValue;

                // 位置のVector3を作成してオブジェクトをインスタンス化します。
                Vector3 pos = new Vector3(x, height, z);
                InstantiateFieldParts(pos);
            }
        }
    }

    /// <Summary>
    /// オブジェクトをインスタンス化するメソッドです。
    /// </Summary>
    void InstantiateFieldParts(Vector3 pos)
    {
        // オブジェクトをインスタンス化します。
        GameObject obj = Instantiate(fieldParts, Vector3.zero, Quaternion.identity);

        // オブジェクトのTransformを設定します。
        obj.transform.SetParent(fieldParent);
        obj.transform.localPosition = pos;

        // マテリアルをランダムにセットします。
        int matIndex = Random.Range(0, matList.Count);
        obj.GetComponent<MeshRenderer>().material = matList[matIndex];
    }

    /// <Summary>
    /// 削除ボタンが押された時のメソッドです。
    /// </Summary>
    public void OnPressedRemoveButton()
    {
        RemoveFieldParts();
    }

    /// <Summary>
    /// フィールドのパーツを削除するメソッドです。
    /// </Summary>
    void RemoveFieldParts()
    {
        foreach (Transform tran in fieldParent)
        {
            Destroy(tran.gameObject);
        }
    }
}

