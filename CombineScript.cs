using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CombineScript : MonoBehaviour
{
    // �����������b�V���̃}�e���A���ł��B
    [SerializeField]private Material combinedMat;

    // �t�B�[���h�p�[�c�̐e�I�u�W�F�N�g��Transform�ł��B
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


        // �e�I�u�W�F�N�g��MeshFilter�����邩�ǂ����m�F���܂��B
        MeshFilter parentMeshFilter = CheckParentComponent<MeshFilter>(fieldParent.gameObject);

        // �e�I�u�W�F�N�g��MeshRenderer�����邩�ǂ����m�F���܂��B
        MeshRenderer parentMeshRenderer = CheckParentComponent<MeshRenderer>(fieldParent.gameObject);

        // �����������b�V�����Z�b�g���܂��B
        parentMeshFilter.mesh = new Mesh();
        parentMeshFilter.mesh.CombineMeshes(combine);

        // �����������b�V���Ƀ}�e���A�����Z�b�g���܂��B
        parentMeshRenderer.material = combinedMat;

        // �����������b�V�����R���C�_�[�ɃZ�b�g���܂��B
        MeshCollider meshCol = CheckParentComponent<MeshCollider>(fieldParent.gameObject);
        meshCol.sharedMesh = parentMeshFilter.mesh;

        // �e�I�u�W�F�N�g��\�����܂��B
        fieldParent.gameObject.SetActive(true);
    }

    /// <Summary>
    /// �w�肳�ꂽ�R���|�[�l���g�ւ̎Q�Ƃ��擾���܂��B
    /// �R���|�[�l���g���Ȃ��ꍇ�̓A�^�b�`���܂��B
    /// </Summary>
    TargetComponent CheckParentComponent<TargetComponent>(GameObject obj) where TargetComponent : Component
    {
        // �^�p�����[�^�Ŏw�肵���R���|�[�l���g�ւ̎Q�Ƃ��擾���܂��B
        TargetComponent targetComponent = obj.GetComponent<TargetComponent>();
        if (targetComponent == null)
        {
            targetComponent = obj.AddComponent<TargetComponent>();
        }
        return targetComponent;
    }
}