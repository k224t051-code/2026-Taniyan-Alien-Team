using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Tooltip("TitleScene")]
    [SerializeField] private string titleSceneName = "TitleScene";

    // リスタートボタン用
    public void RestartGame()
    {
        // 現在のシーンの名前を取得して再読み込み
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }

    // タイトルへ戻るボタン用
    public void GoToTitle()
    {
        SceneManager.LoadScene(titleSceneName);
    }

    // 終了ボタン用
    public void QuitGame()
    {
        #if UNITY_EDITOR
            // エディタ上でのテスト時はプレイモードを解除
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // 実際のビルド（Quest実機など）ではアプリを終了
            Application.Quit();
        #endif
    }
}