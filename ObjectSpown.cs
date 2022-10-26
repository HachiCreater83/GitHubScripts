using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpown : MonoBehaviour
{
    /*
     * 生成するオブジェクトを設定
     * 出現する時間を設定
     */
    [SerializeField, Header("生成するオブジェクトをいれる")] private GameObject Obj;
    [SerializeField, Header("生成するまでの時間")] private float time;

    //スポーンするまでの時間加算用の変数
    private float delta = 0;

    //オブジェクトを保持する空のオブジェクトの設定(子オブジェクトを参照するためTransformを使用)
    Transform bullets;

    private void Start()
    {
        //弾を保持する空のオブジェクトを生成
        bullets = new GameObject("SpownPointenemys").transform;
    }
    private void Update()
    {
        //スポーン感覚に合わせてオブジェクトを生成
        delta += Time.deltaTime;
        if (this.delta > this.time)
        {
            //オブジェクト生成関数を呼び出し
            InstBullet(transform.position, transform.rotation);
             // スポーン感覚をリセット
            delta = 0;
        }
    }
    /// <summary>
    /// オブジェクト生成関数
    /// </summary>
    /// <param name="pos">生成位置</param>
    /// <param name="rotation">生成時の回転</param>
    public void InstBullet(Vector3 pos, Quaternion rotation)
    {
        //アクティブでないオブジェクトをbulletsの中から探索
        foreach (Transform transform in bullets)
        {
            if (!transform.gameObject.activeSelf)
            {
                //非アクティブなオブジェクトの位置と回転を設定
                transform.SetPositionAndRotation(pos, rotation);
                //アクティブにする
                transform.gameObject.SetActive(true);
                return;
            }
        }
        //非アクティブなオブジェクトがない場合新規生成
        //生成時にbulletsの子オブジェクトにする
        Instantiate(Obj, pos, rotation, bullets);
    }
}
