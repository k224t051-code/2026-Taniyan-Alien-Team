using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SimpleFadeTest : MonoBehaviour
{
    [Header("遷移先のシーン名")]
    [SerializeField] private string nextSceneName = "GameScene";

    [Header("フェード設定")]
    [Tooltip("画面を覆う真っ黒なUI Image（Alphaを0にしておくこと）")]
    [SerializeField] private Image fadeImage;
    [Tooltip("フェードアウトにかける時間（秒）")]
    [SerializeField] private float fadeDuration = 1.0f;

    private bool isFading = false;

    private void Update()
    {
        // テスト用：Spaceキーを押したらフェード＆遷移開始
        if (Input.GetKeyDown(KeyCode.Space) && !isFading)
        {
            if (fadeImage == null)
            {
                Debug.LogError("Fade Imageが設定されていません。インスペクターで割り当ててください。");
                return;
            }
            StartCoroutine(FadeAndLoadScene());
        }
    }

    private IEnumerator FadeAndLoadScene()
    {
        isFading = true;
        Debug.Log("フェードアウト開始...");

        // 1. UIのImageを有効化する
        fadeImage.gameObject.SetActive(true);

        // 2. 透明度(Alpha)を0から1へ徐々に上げるループ処理
        float timer = 0f;
        Color currentColor = fadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            // Mathf.Lerpで0(透明)から1(不透明)へ滑らかに変化させる
            currentColor.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeImage.color = currentColor;
            
            // 次のフレームまで待機
            yield return null;
        }

        // 念のため完全に不透明（Alpha=1）にしておく
        currentColor.a = 1f;
        fadeImage.color = currentColor;

        Debug.Log("フェードアウト完了。シーンをロードします。");

        // 3. 非同期でシーンをロードする
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);
        
        // ロード完了まで待機
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}