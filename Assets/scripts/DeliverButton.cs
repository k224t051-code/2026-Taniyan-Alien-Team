using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// 納品判定・ボタン反応・クリア時のシーン遷移をすべて1つでこなす最強スクリプト
/// これを納品ボタン（キューブなど）に直接アタッチします。
/// </summary>
public class DeliverButton : MonoBehaviour
{
    [Header("判定エリアと正解アイテム")]
    [Tooltip("アイテムを置くエリア（DetectArea）")]
    public DetectArea area;
    [Tooltip("正解となるアイテムのリスト")]
    public GameObject[] correctAnswers;

    [Header("シーン遷移設定")]
    [Tooltip("クリア後に移動するシーン名")]
    public string nextSceneName = "GameScene";
    [Tooltip("クリア判定が出てから画面が切り替わるまでの待機時間")]
    public float delaySeconds = 1.0f;

    [Header("サウンド設定（オプション）")]
    public AudioClip submitSound;
    public AudioClip correctSound;
    public AudioClip wrongSound;

    private AudioSource audioSource;
    private bool isProcessing = false;

    private void Start()
    {
        // 音を鳴らすための準備
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    /// <summary>
    /// 方法A：物理的にVRの手やコントローラーがぶつかったら自動で反応する
    /// （Interactable Unity Event の設定が不要になります！）
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // 念のため、納品物(ThrowingItem)がぶつかっただけでは反応しないようにする
        if (!other.CompareTag("ThrowingItem"))
        {
            TrySubmit();
        }
    }

    /// <summary>
    /// 方法B：Interactable Unity Event (When Select等) から手動で呼ぶ用
    /// </summary>
    public void OnClickSubmit()
    {
        TrySubmit();
    }

    // 納品判定と処理のメイン
    private void TrySubmit()
    {
        // 連打防止：すでに処理中なら無視する
        if (isProcessing) return;

        Debug.Log("DeliverButton: 納品ボタンが押されました（触られました）！");

        if (area == null)
        {
            Debug.LogError("DeliverButton: DetectArea が設定されていません！インスペクターを確認してください。");
            return;
        }

        isProcessing = true;
        PlaySound(submitSound);

        // DetectAreaに正解チェックを依頼
        bool isCorrect = area.IsCorrect(correctAnswers);

        if (isCorrect)
        {
            Debug.Log("DeliverButton: 正解！クリア処理へ移行します。");
            PlaySound(correctSound);
            StartCoroutine(ClearSequence());
        }
        else
        {
            Debug.Log("DeliverButton: 不正解！再度やり直せます。");
            PlaySound(wrongSound);
            StartCoroutine(ResetProcess());
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private IEnumerator ResetProcess()
    {
        // 不正解の場合は1秒待ってから再度ボタンを押せるようにする
        yield return new WaitForSeconds(1.0f);
        isProcessing = false;
    }

    private IEnumerator ClearSequence()
    {
        // クリア演出用に少し待機
        yield return new WaitForSeconds(delaySeconds);

        // 非同期で次のシーンへ
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadSceneAsync(nextSceneName);
        }
        else
        {
            Debug.LogWarning("DeliverButton: 遷移先のシーン名が空っぽです！");
        }
    }
}