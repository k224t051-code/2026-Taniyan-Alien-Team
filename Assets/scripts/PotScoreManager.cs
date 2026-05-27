using UnityEngine;
using TMPro; // TextMeshProを使用

public class PotScoreManager : MonoBehaviour
{
    public float score = 1000f;       // 初期スコア
    public float damageMultiplier = 10f; // 衝撃をスコア減少に変換する倍率
    public float minImpact = 1.0f;    // 減点対象となる最小の衝撃

    public TextMeshProUGUI scoreText; // 画面または空間に配置したテキスト

    void Start()
    {
        UpdateUI();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 衝突の強さを取得
        float impactForce = collision.relativeVelocity.magnitude;

        if (impactForce > minImpact)
        {
            // 衝撃に応じて減点
            float penalty = impactForce * damageMultiplier;
            score -= penalty;
            
            // スコアが0以下にならないように固定
            score = Mathf.Max(0, score);
            
            UpdateUI();
            Debug.Log($"衝突！ 衝撃: {impactForce} / 減点: {penalty}");
        }
    }

    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + Mathf.CeilToInt(score).ToString();
        }
    }
}