using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <Summary>
/// �p�[�����m�C�Y���g���ăt�B�[���h�𐶐�����N���X�ł��B
/// </Summary>
public class FieldGenerator : MonoBehaviour
{
    // �t�B�[���h�쐬�̂��߂̃p�[�c�ւ̎Q�Ƃł��B
    public GameObject fieldParts;

    // �t�B�[���h�p�[�c�̐e�I�u�W�F�N�g��Transform�ł��B
    public Transform fieldParent;

    // �t�B�[���h�Ɋւ�����ł��B
    public int fieldSizeX = 50;
    public int fieldSizeZ = 50;
    public int fieldHeight = 50;

    // �p�[�����m�C�Y�Ɋւ�����ł��B
    public float xOrigin;
    public float yOrigin;
    public float scale = 0.01f;

    // ��������I�u�W�F�N�g�̃}�e���A�����X�g�ł��B
    public List<Material> matList;


    /// <Summary>
    /// �����{�^���������ꂽ���̃��\�b�h�ł��B
    /// </Summary>
    public void OnPressedGenerateButton()
    {
        GenerateFieldParts();
    }

    /// <Summary>
    /// �t�B�[���h�𐶐����郁�\�b�h�ł��B
    /// </Summary>
    void GenerateFieldParts()
    {
        for (float z = 0f; z < fieldSizeZ; z++)
        {
            for (float x = 0f; x < fieldSizeX; x++)
            {
                // �p�[�����m�C�Y�̍��W���w�肵�Ēl���擾���܂��B
                float xValue = xOrigin + x * scale;
                float yValue = yOrigin + z * scale;
                float perlinValue = Mathf.PerlinNoise(xValue, yValue);
                float height = fieldHeight * perlinValue;

                // �ʒu��Vector3���쐬���ăI�u�W�F�N�g���C���X�^���X�����܂��B
                Vector3 pos = new Vector3(x, height, z);
                InstantiateFieldParts(pos);
            }
        }
    }

    /// <Summary>
    /// �I�u�W�F�N�g���C���X�^���X�����郁�\�b�h�ł��B
    /// </Summary>
    void InstantiateFieldParts(Vector3 pos)
    {
        // �I�u�W�F�N�g���C���X�^���X�����܂��B
        GameObject obj = Instantiate(fieldParts, Vector3.zero, Quaternion.identity);

        // �I�u�W�F�N�g��Transform��ݒ肵�܂��B
        obj.transform.SetParent(fieldParent);
        obj.transform.localPosition = pos;

        // �}�e���A���������_���ɃZ�b�g���܂��B
        int matIndex = Random.Range(0, matList.Count);
        obj.GetComponent<MeshRenderer>().material = matList[matIndex];
    }

    /// <Summary>
    /// �폜�{�^���������ꂽ���̃��\�b�h�ł��B
    /// </Summary>
    public void OnPressedRemoveButton()
    {
        RemoveFieldParts();
    }

    /// <Summary>
    /// �t�B�[���h�̃p�[�c���폜���郁�\�b�h�ł��B
    /// </Summary>
    void RemoveFieldParts()
    {
        foreach (Transform tran in fieldParent)
        {
            Destroy(tran.gameObject);
        }
    }
}

