using UnityEngine;
using TMPro;           // TextMeshProを操作するために追加
using System.Collections; // 時間待ち（コルーチン）を使うために追加

public class NPCReaction : MonoBehaviour
{
    [SerializeField] private float heavyImpactThreshold = 2.0f;

    [Header("オーディオ設定")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip heavySound;
    [SerializeField] private AudioClip lightSound;

    [Header("吹き出し設定")]
    [SerializeField] private GameObject speechBubble; // Canvasをここに入れる
    [SerializeField] private TextMeshProUGUI speechText;  // TextMeshProをここに入れる

    private void Start()
    {
        // ゲーム開始時は吹き出しを非表示（オフ）にしておく
        if (speechBubble != null)
        {
            speechBubble.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ThrowingItem"))
        {
            float impactSpeed = collision.relativeVelocity.magnitude;

            if (impactSpeed >= heavyImpactThreshold)
            {
                // 強い時：強い音、痛そうなテキスト、2秒間表示
                PlayReaction(heavySound, "痛っ！！！", 2.0f);
            }
            else
            {
                // 弱い時：弱い音、ちょっとしたテキスト、1.5秒間表示
                PlayReaction(lightSound, "いてっ", 1.5f);
            }
        }
    }

    // 音と吹き出しをセットで再生する関数
    private void PlayReaction(AudioClip sound, string message, float displayTime)
    {
        // 音を鳴らす
        if (audioSource != null && sound != null) audioSource.PlayOneShot(sound);

        if (speechBubble != null && speechText != null)
        {
            // 連続で当たった時のために、一旦古い「消すタイマー」を止める
            StopAllCoroutines();

            // テキストを書き換えて、吹き出しを表示
            speechText.text = message;
            speechBubble.SetActive(true);

            // 指定時間後に吹き出しを消すタイマーをスタート
            StartCoroutine(HideBubbleAfterDelay(displayTime));
        }
    }

    // 一定時間待ってから吹き出しを消す処理（コルーチン）
    private IEnumerator HideBubbleAfterDelay(float delay)
    {
        // delay秒だけ待機
        yield return new WaitForSeconds(delay);
        
        // 吹き出しを非表示にする
        if (speechBubble != null)
        {
            speechBubble.SetActive(false);
        }
    }
}