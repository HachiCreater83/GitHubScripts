using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EXPCounter : MonoBehaviour
{
    /*変数
     *反映される経験値の初期値
     *経験値を表示するテキストの取得
     */
    public int ExpPoint;
    public Text ExpText;
    public int skillcount;

    //レベル表示
    PlayerController script_L;
    public int level;

    //経験値加算処理
    public void AddExp()
    {
        ++ExpPoint;
        ++skillcount;
    }
    //レベルを反映する
    public void Update()
    {
        script_L = FindObjectOfType<PlayerController>();
        level = script_L.player_level;
        ExpText.text = "Level;" + level.ToString();
    }
}
