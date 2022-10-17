using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TITLEB : MonoBehaviour
{

    [SerializeField,Header("飛ばすシーン名をいれる")]private string nextSceneName;

    public void PressStart()
    {
        //ここに次のシーンへいく命令を書く
        SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
    }

    //ゲームプレイ終了
    public void OnclickStarButtonExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    //ゲームプレイ終了
    Application.Quit();
#endif
    }
}
