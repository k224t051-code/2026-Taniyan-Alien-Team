using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// 特定のタグに対するリアクション設定
[System.Serializable]
public class TagReactionSetting
{
    [Tooltip("反応させたいタグの名前")]
    public string targetTag;
    
    [Header("強い衝撃用")]
    [Tooltip("強い衝撃とみなす速度のしきい値")]
    public float heavyThreshold = 2.0f;
    [Tooltip("強い時の音声")]
    public AudioClip heavySound;
    [Tooltip("強い時の画像")]
    public Sprite heavySprite;
    
    [Header("弱い衝撃用")]
    [Tooltip("弱い時の音声")]
    public AudioClip lightSound;
    [Tooltip("弱い時の画像")]
    public Sprite lightSprite;

    [Header("表示時間")]
    [Tooltip("吹き出しの表示時間")]
    public float displayTime = 2.0f;
}

public class NPCReaction : MonoBehaviour
{
    [Header("特定のタグに対するリアクション設定")]
    [Tooltip("ここで設定したタグが当たった時、専用のリアクションを返します")]
    [SerializeField] private TagReactionSetting[] specificTagReactions;

    [Space(10)]
    [Header("通常の投擲物(ThrowingItem)の強弱設定")]
    [SerializeField] private float heavyImpactThreshold = 2.0f;
    [SerializeField] private AudioClip heavySound;
    [SerializeField] private AudioClip lightSound;
    [SerializeField] private Sprite heavySprite;
    [SerializeField] private Sprite lightSprite;

    [Header("表示時間")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject speechBubble; 
    [SerializeField] private Image reactionImage;     

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
        string hitTag = collision.gameObject.tag;
        float impactSpeed = collision.relativeVelocity.magnitude;

        // 1. まず、特定タグのリスト（ネジなど）に一致するかチェックする
        foreach (var reaction in specificTagReactions)
        {
            if (hitTag == reaction.targetTag)
            {
                // 一致するタグが見つかったら、強弱を判定する
                if (impactSpeed >= reaction.heavyThreshold)
                {
                    // 強い時のリアクション
                    PlayReaction(reaction.heavySound, reaction.heavySprite, reaction.displayTime);
                }
                else
                {
                    // 弱い時のリアクション
                    PlayReaction(reaction.lightSound, reaction.lightSprite, reaction.displayTime);
                }
                return; // 処理を終了
            }
        }

        // 2. 特定タグに当てはまらず、"ThrowingItem" だった場合は従来の強弱判定を行う
        if (hitTag == "ThrowingItem")
        {
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

    // 音と画像表示をセットで再生する関数
    private void PlayReaction(AudioClip sound, Sprite sprite, float displayTime)
    {
        // 音を鳴らす
        if (audioSource != null && sound != null)
        {
            audioSource.PlayOneShot(sound);
        }

        // 吹き出しと画像の処理
        if (speechBubble != null && reactionImage != null && sprite != null)
        {
            // 連続で当たった時のために、一旦古いタイマーを止める
            StopAllCoroutines();

            // 画像の差し替え処理
            reactionImage.sprite = sprite;
            speechBubble.SetActive(true);

            // 指定時間後に消すタイマーをスタート
            StartCoroutine(HideBubbleAfterDelay(displayTime));
        }
    }

    // 一定時間待ってから吹き出しを消す処理（コルーチン）
    private IEnumerator HideBubbleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (speechBubble != null)
        {
            speechBubble.SetActive(false);
        }
    }
}