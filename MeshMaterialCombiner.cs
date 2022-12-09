using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <Summary>
/// メッシュとマテリアルを結合するクラスです。
/// </Summary>
public class MeshMaterialCombiner : MonoBehaviour
{
    // フィールドの親オブジェクトのTransformです
    // インスペクターウィンドウからセットするようにします
    public Transform fieldParent;

    /// <Summary>
    /// 結合ボタンが押された時のメソッドです
    /// シーン内のUIから呼び出すようにします
    /// </Summary>
    public void OnPressedCombineMaterialButton()
    {
        CombineMeshWithMaterial();
    }

    /// <Summary>
    /// メッシュとマテリアルを結合するために用います
    /// </Summary>
    private void CombineMeshWithMaterial()
    {
        // 地形オブジェクトのMeshFilterへの参照を配列として保持します
        // オブジェクトの子オブジェクトが持っているコンポーネントをGetComponentsInChildrenで取得します
        MeshFilter[] meshFilters = fieldParent.GetComponentsInChildren<MeshFilter>();
        MeshRenderer[] meshRenderers = fieldParent.GetComponentsInChildren<MeshRenderer>();

        // MeshFilterとMeshRendererの数が合っていない場合は処理を抜けます。
        if (meshFilters.Length != meshRenderers.Length)
        {
            return;
        }

        // 子オブジェクトのメッシュをマテリアルごとにグループ分けします
        // マテリアル名をキーとして、マテリアルを値に持つ辞書、そのマテリアルを持っているMeshFilterのリストの辞書をそれぞれ作成しています
        Dictionary<string, Material> matNameDict = new Dictionary<string, Material>();
        Dictionary<string, List<MeshFilter>> matFilterDict = new Dictionary<string, List<MeshFilter>>();
        for (int MaterialCount = 0; MaterialCount < meshFilters.Length; MaterialCount++)
        {
            Material material = meshRenderers[MaterialCount].material;
            string matName = material.name;

            // 辞書のキーにマテリアルが登録されていない場合はMeshFilterのリストを追加します
            if (!matFilterDict.ContainsKey(matName))
            {
                List<MeshFilter> filterList = new List<MeshFilter>();
                matFilterDict.Add(matName, filterList);
                matNameDict.Add(matName, material);
            }

            matFilterDict[matName].Add(meshFilters[MaterialCount]);
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
            for (int MeshCount = 0; MeshCount < filterList.Count; MeshCount++)
            {
                combine[MeshCount].mesh = filterList[MeshCount].sharedMesh;
                combine[MeshCount].transform = filterList[MeshCount].transform.localToWorldMatrix;
                filterList[MeshCount].gameObject.SetActive(false);
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
        /*
         * 結合したメッシュを保持するためのゲームオブジェクトを作成
         * 地形オブジェクトの下に作成
         * SetParentには、フィールドの親オブジェクトのTransformが入る
         */

        GameObject obj = new GameObject();
        obj.name = $"CombinedMesh_{matName}";
        obj.transform.SetParent(fieldParent);
        obj.transform.localPosition = Vector3.zero;
        return obj;
    }

    /// <Summary>
    /// フィールドとしてマテリアルをセットするリスト
    /// 指定されたコンポーネントへの参照を取得します。
    /// コンポーネントがない場合はアタッチします。
    /// </Summary>
    Transfrom CheckComponent<Transfrom>(GameObject obj) where Transfrom : Component
    {
        // 型パラメータで指定したコンポーネントへの参照を取得します。
        Transfrom targetComp = obj.GetComponent<Transfrom>();
        if (targetComp == null)
        {
            targetComp = obj.AddComponent<Transfrom>();
        }
        return targetComp;
    }
}

