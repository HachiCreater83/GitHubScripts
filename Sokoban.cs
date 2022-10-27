using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Sokoban : MonoBehaviour
{
    // タイルの種類
    private enum TileType
    {
        /*タイルの番号振り分け
         * NONE=0,何も無い
         * GROUND=1,地面
         * TARGET=2,目的地
         * PLAYER=3,プレイヤー
         * BLOCK=4ブロック
         * PLAYER_ON_TARGET=5,プレイヤー（目的地の上にいる場合）
         * BLOCK_ON_TARGET=6 ,ブロック（目的地の上にある場合）
         */

        NONE,
        GROUND,
        TARGET,
        PLAYER,
        BLOCK,
        PLAYER_ON_TARGET,
        BLOCK_ON_TARGET,
    }

    // 方向の種類
    private enum DirectionType
    {
        UP, // 上に移動
        RIGHT, // 右に移動
        DOWN, // 下に移動
        LEFT, // 左に移動
    }

    #region//タイル情報
    public TextAsset stageFile; // ステージ構造が記述されたテキストファイル
    private int rows; // 行数
    private int columns; // 列数
    private TileType[,] tileList; // タイル情報を管理する二次元配列
    public float tileSize; // タイルのサイズ
    #endregion
    #region //スプライトの設定
    [SerializeField, Header("地面のスプライト")] private Sprite groundSprite;
    [SerializeField, Header("目的地のスプライト")] private Sprite targetSprite;
    [SerializeField, Header("ブロックのスプライト")] private Sprite blockSprite;
    [SerializeField, Header("アイテムのスプライト")] private Sprite itemSprite;
    [SerializeField, Header("プレイヤーのスプライト")] private Sprite playerSprite;
    [SerializeField, Header("プレイヤーの上向きスプライト")] private Sprite player_upSprite;
    [SerializeField, Header("プレイヤーの左向きスプライト")] private Sprite player_leftSprite;
    [SerializeField, Header("プレイヤーの下向きスプライト")] private Sprite player_downSprite;
    [SerializeField, Header("プレイヤーの右向きスプライト")] private Sprite player_rightSprite;
    #endregion

    private GameObject player = null; // プレイヤーのゲームオブジェクト
    private Vector2 middleOffset; // 中心位置
    private int blockCount = default; // ブロックの数
    private bool isClear = false; // ゲームをクリアした場合 true
    private bool isMiss = false; // ゲームをクリアした場合 true
    private SpriteRenderer playersp = null;//プレイヤーの方向に向く変数
    public Canvas CountCanvas = null;//行動回数を表すキャンバス
    public Text ActionCountText = null;//プレイヤーの行動回数を表示するテキスト
    public int NumberActions = 20;//プレイヤーの行動回数

    [SerializeField, Header("シーン切り替え時に表示されるCanvas")] private GameObject CutInCanvas = null;
    StageManager stage_manager;

    // 各位置に存在するゲームオブジェクトを管理する連想配列
    private Dictionary<GameObject, Vector2Int> gameObjectPosTable = new Dictionary<GameObject, Vector2Int>();

    // ゲーム開始時に呼び出される
    private void Start()
    {
        LoadTileData(); // タイルの情報を読み込む
        CreateStage(); // ステージを作成
        ActionCountText.text = NumberActions.ToString();//行動回数の表示
    }

    // タイルの情報を読み込む
    private void LoadTileData()
    {
        // タイルの情報を一行ごとに分割
        var lines = stageFile.text.Split
        (
            new[] { '\r', '\n' },
            StringSplitOptions.RemoveEmptyEntries
        );

        // タイルの列数を計算
        var nums = lines[0].Split(new[] { ',' });

        // タイルの列数と行数を保持
        rows = lines.Length; // 行数
        columns = nums.Length; // 列数

        // タイル情報を int 型の２次元配列で保持
        tileList = new TileType[columns, rows];
        for (int y = 0; y < rows; y++)
        {
            // 一文字ずつ取得
            var st = lines[y];
            nums = st.Split(new[] { ',' });
            for (int x = 0; x < columns; x++)
            {
                // 読み込んだ文字を数値に変換して保持
                tileList[x, y] = (TileType)int.Parse(nums[x]);
            }
        }
    }

    // ステージを作成
    private void CreateStage()
    {
        // ステージの中心位置を計算
        middleOffset.x = columns * tileSize * 0.5f - tileSize * 0.5f;
        middleOffset.y = rows * tileSize * 0.5f - tileSize * 0.5f; ;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                var val = tileList[x, y];

                // 何も無い場所は無視
                if (val == TileType.NONE) continue;

                // タイルの名前に行番号と列番号を付与
                var name = "tile" + y + "_" + x;

                // タイルのゲームオブジェクトを作成
                var tile = new GameObject(name);

                // タイルにスプライトを描画する機能を追加
                var sr = tile.AddComponent<SpriteRenderer>();

                // タイルのスプライトを設定
                sr.sprite = groundSprite;

                // タイルの位置を設定
                tile.transform.position = GetDisplayPosition(x, y);

                // 目的地の場合
                if (val == TileType.TARGET)
                {
                    // 目的地のゲームオブジェクトを作成
                    var destination = new GameObject("destination");

                    // 目的地にスプライトを描画する機能を追加
                    sr = destination.AddComponent<SpriteRenderer>();

                    // 目的地のスプライトを設定
                    sr.sprite = targetSprite;

                    // 目的地の描画順を手前にする
                    sr.sortingOrder = 1;

                    // 目的地の位置を設定
                    destination.transform.position = GetDisplayPosition(x, y);
                }
                // プレイヤーの場合
                if (val == TileType.PLAYER)
                {
                    // プレイヤーのゲームオブジェクトを作成
                    player = new GameObject("player");

                    // プレイヤーにスプライトを描画する機能を追加
                    sr = player.AddComponent<SpriteRenderer>();

                    // プレイヤーのスプライトを設定
                    sr.sprite = playerSprite;

                    // プレイヤーの描画順を手前にする
                    sr.sortingOrder = 2;

                    // プレイヤーの位置を設定
                    player.transform.position = GetDisplayPosition(x, y);

                    // プレイヤーを連想配列に追加
                    gameObjectPosTable.Add(player, new Vector2Int(x, y));
                }
                // ブロックの場合
                else if (val == TileType.BLOCK)
                {
                    // ブロックの数を増やす
                    blockCount++;

                    // ブロックのゲームオブジェクトを作成
                    var block = new GameObject("block" + blockCount);

                    // ブロックにスプライトを描画する機能を追加
                    sr = block.AddComponent<SpriteRenderer>();

                    // ブロックのスプライトを設定
                    sr.sprite = blockSprite;

                    // ブロックの描画順を手前にする
                    sr.sortingOrder = 2;

                    // ブロックの位置を設定
                    block.transform.position = GetDisplayPosition(x, y);

                    // ブロックを連想配列に追加
                    gameObjectPosTable.Add(block, new Vector2Int(x, y));
                }
            }
        }
    }

    // 指定された行番号と列番号からスプライトの表示位置を計算して返す
    private Vector2 GetDisplayPosition(int x, int y)
    {
        return new Vector2
        (
            x * tileSize - middleOffset.x,
            y * -tileSize + middleOffset.y
        );
    }

    // 指定された位置に存在するゲームオブジェクトを返します
    private GameObject GetGameObjectAtPosition(Vector2Int pos)
    {
        foreach (var pair in gameObjectPosTable)
        {
            // 指定された位置が見つかった場合
            if (pair.Value == pos)
            {
                // その位置に存在するゲームオブジェクトを返す
                return pair.Key;
            }
        }
        return null;
    }

    // 指定された位置のタイルがブロックなら true を返す
    private bool IsBlock(Vector2Int pos)
    {
        var cell = tileList[pos.x, pos.y];
        return cell == TileType.BLOCK || cell == TileType.BLOCK_ON_TARGET;
    }

    // 指定された位置がステージ内なら true を返す
    private bool IsValidPosition(Vector2Int pos)
    {
        if (0 <= pos.x && pos.x < columns && 0 <= pos.y && pos.y < rows)
        {
            return tileList[pos.x, pos.y] != TileType.NONE;
        }
        return false;
    }

    // 毎フレーム呼び出される
    private void Update()
    {
        // ゲームの終了判定中は操作できないようにする
        if (isClear || isMiss) return;

        #region //移動設定
        // 上矢印が押された場合
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            // プレイヤーが上に移動できるか検証
            TryMovePlayer(DirectionType.UP);
        }
        // 右矢印が押された場合
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            // プレイヤーが右に移動できるか検証
            TryMovePlayer(DirectionType.RIGHT);
        }
        // 下矢印が押された場合
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            // プレイヤーが下に移動できるか検証
            TryMovePlayer(DirectionType.DOWN);
        }
        // 左矢印が押された場合
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            // プレイヤーが左に移動できるか検証
            TryMovePlayer(DirectionType.LEFT);
        }

        #endregion
    }

    // 指定された方向にプレイヤーが移動できるか検証
    // 移動できる場合は指定の方向に移動する
    private void TryMovePlayer(DirectionType direction)
    {
        // プレイヤーの現在地を取得
        var currentPlayerPos = gameObjectPosTable[player];

        // プレイヤーの移動先の位置を計算
        var nextPlayerPos = GetNextPositionAlong(currentPlayerPos, direction);

        // プレイヤーの移動先がステージ内ではない場合は無視
        if (!IsValidPosition(nextPlayerPos)) return;

        // プレイヤーの移動先にブロックが存在する場合
        if (IsBlock(nextPlayerPos))
        {
            // ブロックの移動先の位置を計算
            var nextBlockPos = GetNextPositionAlong(nextPlayerPos, direction);

            //プレイヤーの行動回数を減らす
            NumberActions--;
            ActionCountText.text = NumberActions.ToString();

            // ブロックの移動先がステージ内の場合かつ
            // ブロックの移動先にブロックが存在しない場合
            if (IsValidPosition(nextBlockPos))
            {
                // 移動するブロックを取得
                var block = GetGameObjectAtPosition(nextPlayerPos);

                // プレイヤーの移動先のタイルの情報を更新
                UpdateGameObjectPosition(nextPlayerPos);

                // ブロックを移動
                block.transform.position = GetDisplayPosition(nextBlockPos.x, nextBlockPos.y);

                // ブロックの位置を更新
                gameObjectPosTable[block] = nextBlockPos;

                // ブロックの移動先の番号を更新
                if (tileList[nextBlockPos.x, nextBlockPos.y] == TileType.GROUND)
                {
                    // 移動先が地面ならブロックの番号に更新
                    tileList[nextBlockPos.x, nextBlockPos.y] = TileType.BLOCK;
                }
                else if (tileList[nextBlockPos.x, nextBlockPos.y] == TileType.TARGET)
                {
                    // 移動先が目的地ならブロック（目的地の上）の番号に更新
                    tileList[nextBlockPos.x, nextBlockPos.y] = TileType.BLOCK_ON_TARGET;
                }

                // プレイヤーの現在地のタイルの情報を更新
                UpdateGameObjectPosition(currentPlayerPos);

                // プレイヤーを移動
                player.transform.position = GetDisplayPosition(nextPlayerPos.x, nextPlayerPos.y);

                // プレイヤーの位置を更新
                gameObjectPosTable[player] = nextPlayerPos;

                // プレイヤーの移動先の番号を更新
                if (tileList[nextPlayerPos.x, nextPlayerPos.y] == TileType.GROUND)
                {
                    // 移動先が地面ならプレイヤーの番号に更新
                    tileList[nextPlayerPos.x, nextPlayerPos.y] = TileType.PLAYER;
                }
                else if (tileList[nextPlayerPos.x, nextPlayerPos.y] == TileType.TARGET)
                {
                    // 移動先が目的地ならプレイヤー（目的地の上）の番号に更新
                    tileList[nextPlayerPos.x, nextPlayerPos.y] = TileType.PLAYER_ON_TARGET;
                }
            }
        }
        // プレイヤーの移動先にブロックが存在しない場合
        else
        {
            // プレイヤーの現在地のタイルの情報を更新
            UpdateGameObjectPosition(currentPlayerPos);

            // プレイヤーを移動
            player.transform.position = GetDisplayPosition(nextPlayerPos.x, nextPlayerPos.y);

            // プレイヤーの位置を更新
            gameObjectPosTable[player] = nextPlayerPos;

            //プレイヤーの行動回数を減らす
            NumberActions--;
            ActionCountText.text = NumberActions.ToString();

            // プレイヤーの移動先の番号を更新
            // 移動先が地面ならプレイヤーの番号に更新
            if (tileList[nextPlayerPos.x, nextPlayerPos.y] == TileType.GROUND)
            {
                tileList[nextPlayerPos.x, nextPlayerPos.y] = TileType.PLAYER;
            }

            // 移動先が目的地ならプレイヤー（目的地の上）の番号に更新
            else if (tileList[nextPlayerPos.x, nextPlayerPos.y] == TileType.TARGET)
            {
                tileList[nextPlayerPos.x, nextPlayerPos.y] = TileType.PLAYER_ON_TARGET;
            }
        }
        // ゲームをクリアしたかどうか確認
        CheckCompletion();
    }

    // 指定された方向の位置を返す
    private Vector2Int GetNextPositionAlong(Vector2Int pos, DirectionType direction)
    {
        player = GameObject.Find("player");
        playersp = player.GetComponent<SpriteRenderer>();

        //まだ行動できるかどうかの確認
        if (NumberActions <= 0)
        {
            isMiss = true;
            //シーン切り替え用のコルーチン
          StartCoroutine(DelayCoroutine());
        }
        //移動後の処理
        switch (direction)
        {
            // 上
            case DirectionType.UP:
                pos.y -= 1;
                playersp.sprite = player_upSprite;
                break;

            // 右
            case DirectionType.RIGHT:
                pos.x += 1;
                playersp.sprite = player_rightSprite;
                break;

            // 下
            case DirectionType.DOWN:
                pos.y += 1;
                playersp.sprite = player_downSprite;
                break;

            // 左
            case DirectionType.LEFT:
                pos.x -= 1;
                playersp.sprite = player_leftSprite;
                break;
        }
        return pos;
    }

    // 指定された位置のタイルを更新
    private void UpdateGameObjectPosition(Vector2Int pos)
    {
        // 指定された位置のタイルの番号を取得
        var cell = tileList[pos.x, pos.y];

        // プレイヤーもしくはブロックの場合
        if (cell == TileType.PLAYER || cell == TileType.BLOCK)
        {
            // 地面に変更
            tileList[pos.x, pos.y] = TileType.GROUND;
        }
        // 目的地に乗っているプレイヤーもしくはブロックの場合
        else if (cell == TileType.PLAYER_ON_TARGET || cell == TileType.BLOCK_ON_TARGET)
        {
            // 目的地に変更
            tileList[pos.x, pos.y] = TileType.TARGET;
        }
    }

    // ゲームをクリアしたかどうか確認
    private void CheckCompletion()
    {
        // 目的地に乗っているブロックの数を計算
        int blockOnTargetCount = 0;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                if (tileList[x, y] == TileType.BLOCK_ON_TARGET)
                {
                    blockOnTargetCount++;
                }
            }
        }

        // すべてのブロックが目的地の上に乗っている場合
        if (blockOnTargetCount == blockCount)
        {
            //シーン切り替え用のキャンバスを表示
            CutInCanvas.SetActive(true);
            // ゲームクリア
            isClear = true;
            return;
        }
    }

    //コルーチン一定時間後に次の処理する
    private IEnumerator DelayCoroutine()
    {
        CutInCanvas.SetActive(true);
        // Time.timeScale の影響を受けずに実時間で2秒待つ
        yield return new WaitForSecondsRealtime(2);
        //2秒後にシーンを切り替える
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
   
}
