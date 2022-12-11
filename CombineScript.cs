using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CombineScript : MonoBehaviour
{
    // 結合したメッシュのマテリアルです。
    [SerializeField]private Material combinedMat;

    // フィールドパーツの親オブジェクトのTransformです。
    [SerializeField] private Transform fieldParent;

    private void Start()
    {
        Component[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = ((MeshFilter)meshFilters[i]).sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
            i++;
        }

        print(combine.Length);

        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        transform.gameObject.SetActive(true);


        // 親オブジェクトにMeshFilterがあるかどうか確認します。
        MeshFilter parentMeshFilter = CheckParentComponent<MeshFilter>(fieldParent.gameObject);

        // 親オブジェクトにMeshRendererがあるかどうか確認します。
        MeshRenderer parentMeshRenderer = CheckParentComponent<MeshRenderer>(fieldParent.gameObject);

        // 結合したメッシュをセットします。
        parentMeshFilter.mesh = new Mesh();
        parentMeshFilter.mesh.CombineMeshes(combine);

        // 結合したメッシュにマテリアルをセットします。
        parentMeshRenderer.material = combinedMat;

        // 結合したメッシュをコライダーにセットします。
        MeshCollider meshCol = CheckParentComponent<MeshCollider>(fieldParent.gameObject);
        meshCol.sharedMesh = parentMeshFilter.mesh;

        // 親オブジェクトを表示します。
        fieldParent.gameObject.SetActive(true);
    }

    /// <Summary>
    /// 指定されたコンポーネントへの参照を取得します。
    /// コンポーネントがない場合はアタッチします。
    /// </Summary>
    TargetComponent CheckParentComponent<TargetComponent>(GameObject obj) where TargetComponent : Component
    {
        // 型パラメータで指定したコンポーネントへの参照を取得します。
        TargetComponent targetComponent = obj.GetComponent<TargetComponent>();
        if (targetComponent == null)
        {
            targetComponent = obj.AddComponent<TargetComponent>();
        }
        return targetComponent;
    }
}