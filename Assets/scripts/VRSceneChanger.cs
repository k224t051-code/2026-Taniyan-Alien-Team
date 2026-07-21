using UnityEngine;
using UnityEngine.SceneManagement;

public class VRSceneChanger : MonoBehaviour
{
    [Header("遷移先のシーン名")]
    [SerializeField] private string nextSceneName = "GameScene1";

    // コントローラーのボタンやUIからこのメソッドを呼び出す
    public void StartSceneChange()
    {
        // 非同期で指定したシーンを読み込み、そのまま遷移する
        SceneManager.LoadSceneAsync(nextSceneName);
    }
}