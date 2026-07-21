using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Imageを扱うために追加

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
    [Tooltip("強い時の画像（吹き出しの中身）")]
    public Sprite heavySprite;
    
    [Header("弱い衝撃用")]
    [Tooltip("弱い時の音声")]
    public AudioClip lightSound;
    [Tooltip("弱い時の画像（吹き出しの中身）")]
    public Sprite lightSprite;

    [Header("表示時間")]
    [Tooltip("吹き出しの表示時間")]
    public float displayTime = 2.0f;
}

public class NPCReaction : MonoBehaviour
{
    [Header("アニメーション設定")]
    [SerializeField] private Animator animator;
    [Tooltip("強い衝撃時に再生するAnimatorのTriggerパラメータ名")]
    [SerializeField] private string heavyAnimTrigger = "HeavyHit";
    [Tooltip("弱い衝撃時に再生するAnimatorのTriggerパラメータ名")]
    [SerializeField] private string lightAnimTrigger = "LightHit";

    [Space(10)]
    [Header("特定のタグに対するリアクション設定")]
    [SerializeField] private TagReactionSetting[] specificTagReactions;

    [Space(10)]
    [Header("通常の投擲物(ThrowingItem)の強弱設定")]
    [SerializeField] private float heavyImpactThreshold = 2.0f;
    [SerializeField] private AudioClip heavySound;
    [SerializeField] private Sprite defaultHeavySprite;
    [SerializeField] private AudioClip lightSound;
    [SerializeField] private Sprite defaultLightSprite;

    [Header("表示設定")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject speechBubble; 
    [SerializeField] private Image reactionImage; // TextMeshProUGUIからImageへ変更

    private void Start()
    {
        if (speechBubble != null)
        {
            speechBubble.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        string hitTag = collision.gameObject.tag;
        float impactSpeed = collision.relativeVelocity.magnitude;
        
        bool isHeavy = false;

        // 1. まず、特定タグのリスト（ネジなど）に一致するかチェックする
        foreach (var reaction in specificTagReactions)
        {
            if (hitTag == reaction.targetTag)
            {
                isHeavy = impactSpeed >= reaction.heavyThreshold;
                
                if (isHeavy)
                {
                    PlayReaction(reaction.heavySound, reaction.heavySprite, reaction.displayTime, true);
                }
                else
                {
                    PlayReaction(reaction.lightSound, reaction.lightSprite, reaction.displayTime, false);
                }
                return; // 処理を終了
            }
        }

        // 2. 特定タグに当てはまらず、"ThrowingItem" だった場合は従来の強弱判定を行う
        if (hitTag == "ThrowingItem")
        {
            isHeavy = impactSpeed >= heavyImpactThreshold;
            
            if (isHeavy)
            {
                PlayReaction(heavySound, defaultHeavySprite, 2.0f, true);
            }
            else
            {
                PlayReaction(lightSound, defaultLightSprite, 1.5f, false);
            }
        }
    }

    // 音、画像、アニメーションをセットで再生する関数
    private void PlayReaction(AudioClip sound, Sprite sprite, float displayTime, bool isHeavy)
    {
        // 音を鳴らす
        if (audioSource != null && sound != null)
        {
            audioSource.PlayOneShot(sound);
        }

        // アニメーションの再生（タグの有無に関係なく、強弱のみで判定）
        if (animator != null)
        {
            if (isHeavy)
            {
                animator.SetTrigger(heavyAnimTrigger);
            }
            else
            {
                animator.SetTrigger(lightAnimTrigger);
            }
        }

        // 吹き出しと画像の処理
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