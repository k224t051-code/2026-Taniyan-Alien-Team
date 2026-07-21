using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class DeliverManager : MonoBehaviour
{
    [Header("判定設定")]
    [Tooltip("判定を行うエリア（DetectArea）")]
    public DetectArea area;
    [Tooltip("正解のオブジェクト群")]
    public GameObject[] correctAnswers;

    [Header("クリア演出設定")]
    [Tooltip("遷移先のシーン名")]
    public string nextSceneName = "ResultScene";
    [Tooltip("フェードアウトにかける時間（秒）")]
    public float fadeDuration = 1.0f;
    [Tooltip("画面を覆う真っ黒なUI Image（Alphaを0にしておくこと）")]
    public Image fadeImage;

    [Header("オプション")]
    [Tooltip("正解した時の効果音（なくても可）")]
    public AudioClip clearSound;
    
    private AudioSource audioSource;
    private bool isProcessing = false;

    private void Start()
    {
        if (clearSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    /// <summary>
    /// UIボタンの On Click () からこのメソッドを呼び出します。
    /// </summary>
    public void OnClickSubmit()
    {
        // すでにクリア処理中なら二重に押されないようにする
        if (isProcessing) return;

        if (area == null)
        {
            Debug.LogError("DeliverManager: DetectAreaが設定されていません！");
            return;
        }

        // 判定処理（DetectAreaに正解が入っているかチェック）
        bool isCorrect = area.IsCorrect(correctAnswers);
        
        if (isCorrect)
        {
            Debug.Log("DeliverManager: 正解！クリア処理を開始します。");
            isProcessing = true;
            
            // 正解音を鳴らす
            if (audioSource != null && clearSound != null)
            {
                audioSource.PlayOneShot(clearSound);
            }

            // フェード＆シーン遷移のコルーチンを開始
            StartCoroutine(ClearRoutine());
        }
        else
        {
            Debug.Log("DeliverManager: 不正解です。まだ条件を満たしていません。");
            // ※不正解時の音を鳴らす場合はここに追記できます
        }
    }

    private IEnumerator ClearRoutine()
    {
        // 1. フェードアウト処理
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            float timer = 0f;
            Color c = fadeImage.color;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                c.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
                fadeImage.color = c;
                yield return null;
            }
        }
        else
        {
            // フェード画像が未設定の場合は、指定秒数だけ待機する
            yield return new WaitForSeconds(fadeDuration);
        }

        // 2. シーン遷移
        SceneManager.LoadScene(nextSceneName);
    }
}