using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CombineScript : MonoBehaviour
{
    // 結合した後のオブジェクトのマテリアルです
    [SerializeField]private Material _combinedMat;

    // 親オブジェクトの位置を取得する、
    [SerializeField] private Transform _fieldParent;

    private void Start()
    {
        Component[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int meshCount = 0;
        while (meshCount < meshFilters.Length)
        {
            combine[meshCount].mesh = ((MeshFilter)meshFilters[meshCount]).sharedMesh;
            combine[meshCount].transform = meshFilters[meshCount].transform.localToWorldMatrix;
            meshFilters[meshCount].gameObject.SetActive(false);
            meshCount++;
        }

        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        transform.gameObject.SetActive(true);


        // 親オブジェクトにMeshFilterがあるかどうか確認します。
        MeshFilter parentMeshFilter = CheckParentComponent<MeshFilter>(_fieldParent.gameObject);

        // 親オブジェクトにMeshRendererがあるかどうか確認します。
        MeshRenderer parentMeshRenderer = CheckParentComponent<MeshRenderer>(_fieldParent.gameObject);

        // 結合したメッシュをセットします。
        parentMeshFilter.mesh = new Mesh();
        parentMeshFilter.mesh.CombineMeshes(combine);

        // 結合したメッシュにマテリアルをセットします。
        parentMeshRenderer.material = _combinedMat;

        // 結合したメッシュをコライダーにセットします。
        MeshCollider meshCol = CheckParentComponent<MeshCollider>(_fieldParent.gameObject);
        meshCol.sharedMesh = parentMeshFilter.mesh;

        // 親オブジェクトを表示します。
        _fieldParent.gameObject.SetActive(true);
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