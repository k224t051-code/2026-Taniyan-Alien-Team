using UnityEngine;

/// <summary>
/// UIボタン、またはVR用の3Dボタンから呼び出される「納品（クリア判定）」実行用スクリプト
/// </summary>
public class SubmitButton : MonoBehaviour
{
    [Header("必須設定")]
    [Tooltip("パズルのクリア判定を行うコントローラー（GameManagerなどを割り当て）")]
    [SerializeField] private LeverPuzzleController puzzleController;

    [Header("オプション設定")]
    [Tooltip("ボタンを押したときの効果音（なくても可）")]
    [SerializeField] private AudioClip submitSound;
    
    // 音を再生するためのコンポーネント
    private AudioSource audioSource;
    // 連打を防止するためのフラグ
    private bool isSubmitted = false;

    private void Start()
    {
        // 効果音が設定されている場合、AudioSourceを準備する
        if (submitSound != null)
        {
            audioSource = GetComponent<AudioSource>();
            // もしアタッチされていなければ自動で追加する
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
    }

    /// <summary>
    /// UIボタンの On Click () や、3Dボタンの When Select () にこの関数を設定します。
    /// </summary>
    public void OnClickSubmit()
    {
        // 既にボタンが押されて判定中なら、処理を無視する（連打防止）
        if (isSubmitted) return;

        Debug.Log("SubmitButton: 納品ボタンが押されました。");

        // 音を鳴らす
        if (audioSource != null && submitSound != null)
        {
            audioSource.PlayOneShot(submitSound);
        }

        // コントローラーにクリア判定（納品チェック）を依頼する
        if (puzzleController != null)
        {
            // 一度押したら、処理が終わるまでボタンを無効化
            isSubmitted = true;
            
            // TryClear() の結果（正解したかどうか）を受け取る
            bool isSuccess = puzzleController.TryClear();

            // 不正解（クリア条件を満たしていない）だった場合は、またボタンを押せるように元に戻す
            if (!isSuccess)
            {
                isSubmitted = false;
                Debug.Log("SubmitButton: 不正解！再度ボタンを押せるようにしました。");
            }
        }
        else
        {
            Debug.LogError("SubmitButton: LeverPuzzleController が設定されていません！インスペクターを確認してください。");
        }
    }

    /// <summary>
    /// もし「納品失敗」などで再びボタンを押せるようにしたい場合は、
    /// コントローラー側からこのメソッドを呼び出してリセットします。
    /// </summary>
    public void ResetButton()
    {
        isSubmitted = false;
    }
}