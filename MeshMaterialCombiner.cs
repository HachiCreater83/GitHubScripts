using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <Summary>
/// メッシュとマテリアルを結合するクラスです。
/// </Summary>
public class MeshMaterialCombiner : MonoBehaviour
{
    // フィールドパーツの親オブジェクトのTransformです。
    public Transform fieldParent;

    /// <Summary>
    /// 結合ボタンが押された時のメソッドです。
    /// </Summary>
    public void OnPressedCombineMaterialButton()
    {
        CombineMeshWithMaterial();
    }

    /// <Summary>
    /// メッシュとマテリアルを結合します。
    /// </Summary>
    void CombineMeshWithMaterial()
    {
        // 地形オブジェクトのMeshFilterへの参照を配列として保持します。
        MeshFilter[] meshFilters = fieldParent.GetComponentsInChildren<MeshFilter>();
        MeshRenderer[] meshRenderers = fieldParent.GetComponentsInChildren<MeshRenderer>();

        // MeshFilterとMeshRendererの数が合っていない場合は処理を抜けます。
        if (meshFilters.Length != meshRenderers.Length)
        {
            return;
        }

        // 子オブジェクトのメッシュをマテリアルごとにグループ分けします。
        Dictionary<string, Material> matNameDict = new Dictionary<string, Material>();
        Dictionary<string, List<MeshFilter>> matFilterDict = new Dictionary<string, List<MeshFilter>>();
        for (int i = 0; i < meshFilters.Length; i++)
        {
            Material mat = meshRenderers[i].material;
            string matName = mat.name;

            // 辞書のキーにマテリアルが登録されていない場合はMeshFilterのリストを追加します。
            if (!matFilterDict.ContainsKey(matName))
            {
                List<MeshFilter> filterList = new List<MeshFilter>();
                matFilterDict.Add(matName, filterList);
                matNameDict.Add(matName, mat);
            }

            matFilterDict[matName].Add(meshFilters[i]);
        }

        // グループ分けしたマテリアルごとにオブジェクトを作成し、メッシュを結合します。
        foreach (KeyValuePair<string, List<MeshFilter>> pair in matFilterDict)
        {
            // 結合したメッシュを表示するゲームオブジェクトを作成します。
            GameObject obj = CreateMeshObj(pair.Key);
            obj.transform.SetAsFirstSibling();

            // MeshFilterとMeshRendererをアタッチします。
            MeshFilter combinedMeshFilter = CheckComponent<MeshFilter>(obj);
            MeshRenderer combinedMeshRenderer = CheckComponent<MeshRenderer>(obj);

            // 結合するメッシュの配列を作成します。
            List<MeshFilter> filterList = pair.Value;
            CombineInstance[] combine = new CombineInstance[filterList.Count];

            // 結合するメッシュの情報をCombineInstanceに追加していきます。
            for (int i = 0; i < filterList.Count; i++)
            {
                combine[i].mesh = filterList[i].sharedMesh;
                combine[i].transform = filterList[i].transform.localToWorldMatrix;
                filterList[i].gameObject.SetActive(false);
            }

            // 結合したメッシュを作成したゲームオブジェクトにセットします。
            combinedMeshFilter.mesh = new Mesh();
            combinedMeshFilter.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            combinedMeshFilter.mesh.CombineMeshes(combine);

            // 結合したメッシュにマテリアルをセットします。
            combinedMeshRenderer.material = matNameDict[pair.Key];

            // 結合したメッシュをコライダーにセットします。
            MeshCollider meshCol = CheckComponent<MeshCollider>(obj);
            meshCol.sharedMesh = combinedMeshFilter.mesh;

            // 親オブジェクトを表示します。
            fieldParent.gameObject.SetActive(true);
        }
    }

    /// <Summary>
    /// 結合したメッシュを表示するGameObjectを作成します。
    /// </Summary>
    GameObject CreateMeshObj(string matName)
    {
        GameObject obj = new GameObject();
        obj.name = $"CombinedMesh_{matName}";
        obj.transform.SetParent(fieldParent);
        obj.transform.localPosition = Vector3.zero;
        return obj;
    }

    /// <Summary>
    /// 指定されたコンポーネントへの参照を取得します。
    /// コンポーネントがない場合はアタッチします。
    /// </Summary>
    Transform CheckComponent<Transform>(GameObject obj) where Transform : Component
    {
        // 型パラメータで指定したコンポーネントへの参照を取得します。
        Transform targetComp = obj.GetComponent<Transform>();
        if (targetComp == null)
        {
            targetComp = obj.AddComponent<Transform>();
        }
        return targetComp;
    }
}

