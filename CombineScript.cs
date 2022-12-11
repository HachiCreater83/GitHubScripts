using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CombineScript : MonoBehaviour
{
    // ����������̃I�u�W�F�N�g�̃}�e���A���ł�
    [SerializeField]private Material _combinedMat;

    // �e�I�u�W�F�N�g�̈ʒu���擾����A
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


        // �e�I�u�W�F�N�g��MeshFilter�����邩�ǂ����m�F���܂��B
        MeshFilter parentMeshFilter = CheckParentComponent<MeshFilter>(_fieldParent.gameObject);

        // �e�I�u�W�F�N�g��MeshRenderer�����邩�ǂ����m�F���܂��B
        MeshRenderer parentMeshRenderer = CheckParentComponent<MeshRenderer>(_fieldParent.gameObject);

        // �����������b�V�����Z�b�g���܂��B
        parentMeshFilter.mesh = new Mesh();
        parentMeshFilter.mesh.CombineMeshes(combine);

        // �����������b�V���Ƀ}�e���A�����Z�b�g���܂��B
        parentMeshRenderer.material = _combinedMat;

        // �����������b�V�����R���C�_�[�ɃZ�b�g���܂��B
        MeshCollider meshCol = CheckParentComponent<MeshCollider>(_fieldParent.gameObject);
        meshCol.sharedMesh = parentMeshFilter.mesh;

        // �e�I�u�W�F�N�g��\�����܂��B
        _fieldParent.gameObject.SetActive(true);
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