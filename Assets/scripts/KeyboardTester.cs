using UnityEngine;

/// <summary>
/// VRゴーグルがない時に、パソコンのキーボードやマウスでボタン処理をテストするためのスクリプト
/// </summary>
public class KeyboardTester : MonoBehaviour
{
    [Tooltip("テストしたいDeliverManagerをここに割り当てます")]
    public DeliverManager deliverManager;

    void Update()
    {
        // パソコンの「スペースキー」が押された瞬間を検知
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TestSubmit();
        }
    }

    // インスペクターから手動で実行できるようにする魔法のコード
    [ContextMenu("納品テストを実行する (Test Submit)")]
    public void TestSubmit()
    {
        if (deliverManager != null)
        {
            Debug.Log("⌨️ 納品ボタンをテスト実行します！");
            
            // VRでボタンをレーザーで撃ったのと同じように、直接メソッドを呼び出す
            deliverManager.OnClickSubmit();
        }
        else
        {
            Debug.LogWarning("KeyboardTesterにDeliverManagerが割り当てられていません！インスペクターを確認してください。");
        }
    }

    // ★新しく追加：判定を完全に無視して、強制的にクリア演出とシーン移動を起こす
    [ContextMenu("🚀 判定を無視して強制クリアする (Force Clear)")]
    public void ForceClear()
    {
        if (deliverManager != null)
        {
            Debug.Log("🚀 判定を無視して強制クリア処理を起動します！");
            // DeliverManagerの中にあるクリア処理を、無理やり直接呼び出します
            deliverManager.StartCoroutine("ClearRoutine");
        }
        else
        {
            Debug.LogWarning("KeyboardTesterにDeliverManagerが割り当てられていません！");
        }
    }
}