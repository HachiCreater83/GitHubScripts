using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <Summary>
/// ���b�V���ƃ}�e���A������������N���X�ł��B
/// </Summary>
public class MeshMaterialCombiner : MonoBehaviour
{
    // �t�B�[���h�̐e�I�u�W�F�N�g��Transform�ł�
    // �C���X�y�N�^�[�E�B���h�E����Z�b�g����悤�ɂ��܂�
    public Transform fieldParent;

    /// <Summary>
    /// �����{�^���������ꂽ���̃��\�b�h�ł�
    /// �V�[������UI����Ăяo���悤�ɂ��܂�
    /// </Summary>
    public void OnPressedCombineMaterialButton()
    {
        CombineMeshWithMaterial();
    }

    /// <Summary>
    /// ���b�V���ƃ}�e���A�����������邽�߂ɗp���܂�
    /// </Summary>
    private void CombineMeshWithMaterial()
    {
        // �n�`�I�u�W�F�N�g��MeshFilter�ւ̎Q�Ƃ�z��Ƃ��ĕێ����܂�
        // �I�u�W�F�N�g�̎q�I�u�W�F�N�g�������Ă���R���|�[�l���g��GetComponentsInChildren�Ŏ擾���܂�
        MeshFilter[] meshFilters = fieldParent.GetComponentsInChildren<MeshFilter>();
        MeshRenderer[] meshRenderers = fieldParent.GetComponentsInChildren<MeshRenderer>();

        // MeshFilter��MeshRenderer�̐��������Ă��Ȃ��ꍇ�͏����𔲂��܂��B
        if (meshFilters.Length != meshRenderers.Length)
        {
            return;
        }

        // �q�I�u�W�F�N�g�̃��b�V�����}�e���A�����ƂɃO���[�v�������܂�
        // �}�e���A�������L�[�Ƃ��āA�}�e���A����l�Ɏ������A���̃}�e���A���������Ă���MeshFilter�̃��X�g�̎��������ꂼ��쐬���Ă��܂�
        Dictionary<string, Material> matNameDict = new Dictionary<string, Material>();
        Dictionary<string, List<MeshFilter>> matFilterDict = new Dictionary<string, List<MeshFilter>>();
        for (int MaterialCount = 0; MaterialCount < meshFilters.Length; MaterialCount++)
        {
            Material material = meshRenderers[MaterialCount].material;
            string matName = material.name;

            // �����̃L�[�Ƀ}�e���A�����o�^����Ă��Ȃ��ꍇ��MeshFilter�̃��X�g��ǉ����܂�
            if (!matFilterDict.ContainsKey(matName))
            {
                List<MeshFilter> filterList = new List<MeshFilter>();
                matFilterDict.Add(matName, filterList);
                matNameDict.Add(matName, material);
            }

            matFilterDict[matName].Add(meshFilters[MaterialCount]);
        }

        // �O���[�v���������}�e���A�����ƂɃI�u�W�F�N�g���쐬���A���b�V�����������܂��B
        foreach (KeyValuePair<string, List<MeshFilter>> pair in matFilterDict)
        {
            // �����������b�V����\������Q�[���I�u�W�F�N�g���쐬���܂��B
            GameObject obj = CreateMeshObj(pair.Key);
            obj.transform.SetAsFirstSibling();

            // MeshFilter��MeshRenderer���A�^�b�`���܂��B
            MeshFilter combinedMeshFilter = CheckComponent<MeshFilter>(obj);
            MeshRenderer combinedMeshRenderer = CheckComponent<MeshRenderer>(obj);

            // �������郁�b�V���̔z����쐬���܂��B
            List<MeshFilter> filterList = pair.Value;
            CombineInstance[] combine = new CombineInstance[filterList.Count];

            // �������郁�b�V���̏���CombineInstance�ɒǉ����Ă����܂��B
            for (int MeshCount = 0; MeshCount < filterList.Count; MeshCount++)
            {
                combine[MeshCount].mesh = filterList[MeshCount].sharedMesh;
                combine[MeshCount].transform = filterList[MeshCount].transform.localToWorldMatrix;
                filterList[MeshCount].gameObject.SetActive(false);
            }

            // �����������b�V�����쐬�����Q�[���I�u�W�F�N�g�ɃZ�b�g���܂��B
            combinedMeshFilter.mesh = new Mesh();
            combinedMeshFilter.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            combinedMeshFilter.mesh.CombineMeshes(combine);

            // �����������b�V���Ƀ}�e���A�����Z�b�g���܂��B
            combinedMeshRenderer.material = matNameDict[pair.Key];

            // �����������b�V�����R���C�_�[�ɃZ�b�g���܂��B
            MeshCollider meshCol = CheckComponent<MeshCollider>(obj);
            meshCol.sharedMesh = combinedMeshFilter.mesh;

            // �e�I�u�W�F�N�g��\�����܂��B
            fieldParent.gameObject.SetActive(true);
        }
    }

    /// <Summary>
    /// �����������b�V����\������GameObject���쐬���܂��B
    /// </Summary>
    GameObject CreateMeshObj(string matName)
    {
        /*
         * �����������b�V����ێ����邽�߂̃Q�[���I�u�W�F�N�g���쐬
         * �n�`�I�u�W�F�N�g�̉��ɍ쐬
         * SetParent�ɂ́A�t�B�[���h�̐e�I�u�W�F�N�g��Transform������
         */

        GameObject obj = new GameObject();
        obj.name = $"CombinedMesh_{matName}";
        obj.transform.SetParent(fieldParent);
        obj.transform.localPosition = Vector3.zero;
        return obj;
    }

    /// <Summary>
    /// �t�B�[���h�Ƃ��ă}�e���A�����Z�b�g���郊�X�g
    /// �w�肳�ꂽ�R���|�[�l���g�ւ̎Q�Ƃ��擾���܂��B
    /// �R���|�[�l���g���Ȃ��ꍇ�̓A�^�b�`���܂��B
    /// </Summary>
    Transfrom CheckComponent<Transfrom>(GameObject obj) where Transfrom : Component
    {
        // �^�p�����[�^�Ŏw�肵���R���|�[�l���g�ւ̎Q�Ƃ��擾���܂��B
        Transfrom targetComp = obj.GetComponent<Transfrom>();
        if (targetComp == null)
        {
            targetComp = obj.AddComponent<Transfrom>();
        }
        return targetComp;
    }
}

