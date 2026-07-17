using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ホログラムディスプレイに「プロジェクター投影」特有の
/// 微小な揺れや不規則な点滅（フリッカー）を加え、没入感を高めるスクリプト
/// </summary>
public class HologramEffect : MonoBehaviour
{
    [Header("点滅（フリッカー）設定")]
    [Tooltip("ベースとなる不透明度 (0.0 ～ 1.0)")]
    [Range(0f, 1f)] [SerializeField] private float baseAlpha = 0.7f;
    [Tooltip("点滅の激しさ")]
    [SerializeField] private float flickerIntensity = 0.15f;
    [Tooltip("点滅の頻度（スピード）")]
    [SerializeField] private float flickerSpeed = 20.0f;

    [Header("ノイズ・ブレ（ジッター）設定")]
    [Tooltip("プロジェクターの同期ズレを模した、微細な位置の揺れを発生させるか")]
    [SerializeField] private bool enableJitter = true;
    [Tooltip("揺れの強さ（ピクセル/ユニット単位）")]
    [SerializeField] private float jitterAmount = 0.02f;
    [Tooltip("揺れの頻度")]
    [SerializeField] private float jitterChance = 0.05f;

    [Header("対象コンポーネント")]
    [Tooltip("半透明にする対象のUI Image（未設定なら自動取得）")]
    [SerializeField] private Image targetImage;
    [Tooltip("半透明にする対象のテキスト（未設定なら自動取得）")]
    [SerializeField] private TMP_Text targetText;

    private Vector3 originalPosition;
    private Color imageOriginalColor;
    private Color textOriginalColor;

    private void Start()
    {
        // 開始時の初期位置を記録
        originalPosition = transform.localPosition;

        // 対象コンポーネントが未設定の場合は自動取得
        if (targetImage == null) targetImage = GetComponent<Image>();
        if (targetText == null) targetText = GetComponentInChildren<TMP_Text>();

        // 初期カラーを記録
        if (targetImage != null) imageOriginalColor = targetImage.color;
        if (targetText != null) textOriginalColor = targetText.color;
    }

    private void Update()
    {
        // 1. パーリンノイズを使用して、不規則かつ滑らかな点滅を計算
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);
        // 0.0～1.0のノイズをフリッカーの強さに合わせて変換
        float alphaOffset = (noise - 0.5f) * 2.0f * flickerIntensity;
        float currentAlpha = Mathf.Clamp01(baseAlpha + alphaOffset);

        // UI Image の透明度を更新
        if (targetImage != null)
        {
            Color newColor = imageOriginalColor;
            newColor.a = imageOriginalColor.a * currentAlpha;
            targetImage.color = newColor;
        }

        // テキストの透明度を更新
        if (targetText != null)
        {
            Color newColor = textOriginalColor;
            newColor.a = textOriginalColor.a * currentAlpha;
            targetText.color = newColor;
        }

        // 2. 確率的に発生する微細なブレ（同期ズレ）の再現
        if (enableJitter)
        {
            if (Random.value < jitterChance)
            {
                // ランダムな方向に一瞬だけズラす
                Vector3 jitterOffset = new Vector3(
                    Random.Range(-jitterAmount, jitterAmount),
                    Random.Range(-jitterAmount, jitterAmount),
                    Random.Range(-jitterAmount, jitterAmount)
                );
                transform.localPosition = originalPosition + jitterOffset;
            }
            else
            {
                // 通常時は元の位置に滑らかに戻す
                transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * 10f);
            }
        }
    }
}
