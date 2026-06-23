using UnityEngine;
using UnityEngine.UI;       // UIコンポーネントの操作に必要
using System.Collections;

public class NPCReaction : MonoBehaviour
{
    [SerializeField] private float heavyImpactThreshold = 2.0f;

    [Header("オーディオ設定")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip heavySound;
    [SerializeField] private AudioClip lightSound;

    [Header("表示設定")]
    [SerializeField] private GameObject speechBubble; // 吹き出しのCanvas等の親オブジェクト
    [SerializeField] private Image reactionImage;     // ヒエラルキー上のUI Imageコンポーネント
    
    [Header("切り替える画像データ（※Texture TypeをSpriteにしてください）")]
    [SerializeField] private Sprite heavySprite;      // 強い衝撃時の画像ファイル
    [SerializeField] private Sprite lightSprite;      // 弱い衝撃時の画像ファイル

    private void Start()
    {
        // ゲーム開始時は吹き出しを非表示にしておく
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
                PlayReaction(heavySound, heavySprite, 2.0f);
            }
            else
            {
                PlayReaction(lightSound, lightSprite, 1.5f);
            }
        }
    }

    private void PlayReaction(AudioClip sound, Sprite sprite, float displayTime)
    {
        if (audioSource != null && sound != null)
        {
            audioSource.PlayOneShot(sound);
        }

        if (speechBubble != null && reactionImage != null && sprite != null)
        {
            StopAllCoroutines();

            // 画像の差し替え処理
            reactionImage.sprite = sprite;
            speechBubble.SetActive(true);

            StartCoroutine(HideBubbleAfterDelay(displayTime));
        }
    }

    private IEnumerator HideBubbleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (speechBubble != null)
        {
            speechBubble.SetActive(false);
        }
    }
}