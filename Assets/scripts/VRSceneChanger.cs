using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class VRSceneChanger : MonoBehaviour
{
    [Header("遷移先のシーン名")]
    [SerializeField] private string nextSceneName = "GameScene";

    // コントローラーのボタンやUIからこのメソッドを呼び出す
    public void StartSceneChange()
    {
        StartCoroutine(LoadSceneSequence());
    }

    private IEnumerator LoadSceneSequence()
    {
        // ==========================================
        // 1. ここでフェードアウト処理を実行する
        // （例：目の前に黒いUIパネルを出して徐々に不透明にする等）
        // ==========================================
        Debug.Log("フェードアウト開始...");
        yield return new WaitForSeconds(1.0f); // フェード完了を待つ擬似的な待機時間

        // ==========================================
        // 2. 非同期で次のシーンを読み込む
        // ==========================================
        Debug.Log("非同期ロード開始...");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);

        // シーンがアクティブになるのを一時的に防ぐ（ロード完了後も画面を切り替えない）
        asyncLoad.allowSceneActivation = false;

        // ロードの進行度を監視（0.9でロード完了となる仕様）
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        // ==========================================
        // 3. ロード完了後、シーンを切り替える
        // ==========================================
        Debug.Log("ロード完了。シーンを切り替えます。");
        asyncLoad.allowSceneActivation = true;

        // ※切り替え先のシーンの開始時（Start関数など）でフェードイン処理を行う
    }
}