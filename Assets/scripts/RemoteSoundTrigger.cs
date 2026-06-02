using UnityEngine;
using UnityEngine.InputSystem;

public class RemoteSoundTrigger : MonoBehaviour
{
    [Header("ーーー オーディオ設定 ーーー")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip soundEffect;

    [Header("ーーー 入力設定 (新Input System用) ーーー")]
    // 右コントローラーのAボタンの入力を割り当てる変数
    [SerializeField] private InputActionReference rightControllerAButton;

    private void OnEnable()
    {
        // スクリプトが有効になったら入力を監視し、ボタンが押された（Started）時にメソッドを呼ぶ
        if (rightControllerAButton != null)
        {
            rightControllerAButton.action.Enable();
            rightControllerAButton.action.started += OnAButtonPressed;
        }
    }

    private void OnDisable()
    {
        // スクリプトが無効になったら監視を解除する（メモリリーク防止）
        if (rightControllerAButton != null)
        {
            rightControllerAButton.action.started -= OnAButtonPressed;
            rightControllerAButton.action.Disable();
        }
    }

    // ボタンが押された瞬間に実行される処理
    private void OnAButtonPressed(InputAction.CallbackContext context)
    {
        if (audioSource != null && soundEffect != null)
        {
            audioSource.PlayOneShot(soundEffect);
            Debug.Log("右コントローラーのAボタンが押されました。音を鳴らします。");
        }
    }
}