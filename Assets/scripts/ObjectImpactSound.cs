using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ObjectImpactSound : MonoBehaviour
{
    [Header("サウンド設定")]
    [Tooltip("小さくぶつかった時の音")]
    [SerializeField] private AudioClip lightImpactSound;
    [Tooltip("強くぶつかった時の音")]
    [SerializeField] private AudioClip heavyImpactSound;

    [Header("衝撃の強さ設定")]
    [Tooltip("この速度以下なら音を鳴らさない")]
    [SerializeField] private float minImpactSpeed = 0.5f;
    [Tooltip("この速度以上なら「強い音」を鳴らす")]
    [SerializeField] private float heavyImpactThreshold = 3.0f;

    [Header("音量設定")]
    [Tooltip("衝撃の強さに応じて音量を自動調整するか")]
    [SerializeField] private bool scaleVolumeWithSpeed = true;
    [Tooltip("音量が最大(1.0)になる時の衝突速度")]
    [SerializeField] private float maxVolumeSpeed = 5.0f;

    private AudioSource audioSource;
    private float lastPlayTime = 0f;
    private readonly float cooldownTime = 0.1f; // 連続再生防止のクールダウン（0.1秒）

    private void Awake()
    {
        // アタッチされているAudioSourceを取得
        audioSource = GetComponent<AudioSource>();
        
        // VR空間での没入感を高めるため、音を完全に3Dサウンド（距離や方向がわかる音）に強制設定
        audioSource.spatialBlend = 1.0f; 
        audioSource.playOnAwake = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 1. クールダウン判定（複数の接点が同時にぶつかった際の多重再生を防ぐ）
        if (Time.time - lastPlayTime < cooldownTime) return;

        // 2. 衝突した相対速度（衝撃の強さ）を取得
        float impactSpeed = collision.relativeVelocity.magnitude;

        // 3. 微細な衝突（床の上を滑っている時など）は無視
        if (impactSpeed < minImpactSpeed) return;

        // 4. 速度に応じて鳴らすクリップ（強・弱）を決定
        AudioClip clipToPlay = (impactSpeed >= heavyImpactThreshold) ? heavyImpactSound : lightImpactSound;

        if (clipToPlay != null)
        {
            float volume = 1.0f;

            // 5. 衝撃の強さに応じて音量を計算（よりリアルな物理挙動の表現）
            if (scaleVolumeWithSpeed)
            {
                // 速度を基準に 0.0 ～ 1.0 の割合を算出
                volume = Mathf.Clamp01(impactSpeed / maxVolumeSpeed);
                
                // 音が小さすぎると聞こえないため、下限を20%（0.2）に設定
                volume = Mathf.Max(volume, 0.2f);
            }

            // 音を再生
            audioSource.PlayOneShot(clipToPlay, volume);
            
            // 最後に再生した時間を記録
            lastPlayTime = Time.time;
        }
    }
}