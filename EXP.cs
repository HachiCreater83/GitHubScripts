using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXP : MonoBehaviour
{
    /*
     * 経験値加算する際の入れ物
     * オブジェクトを消すまでの時間
     */
    [SerializeField] private float timer ;
    GameObject expObject;

    private void Start()
    {
        /*
         * ヒエラルキービューにあるオブジェクトを取得
         * 設定時間後にオブジェクトを消す時間
         */
        expObject = GameObject.Find("CounterScripts");
        Destroy(gameObject, timer);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 当たった相手が特定のタグを持っていたら
        if (other.gameObject.tag == "Player")
        {
            /*
             * スコア加算して
             * このオブジェクトを消す
             */
            expObject.GetComponent<EXPCounter>().AddExp();
           gameObject.SetActive(false);
        }
    }
}
