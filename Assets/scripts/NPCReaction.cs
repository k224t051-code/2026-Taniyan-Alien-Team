using UnityEngine;

public class NPCReaction : MonoBehaviour
{
    // リアクションの閾値（どれくらいの勢いで当たったら「強い」と判定するか）
    [SerializeField] private float heavyImpactThreshold = 2.0f;

    // 物理的な衝突が発生した瞬間に呼ばれる関数
    private void OnCollisionEnter(Collision collision)
    {
        // 1. ぶつかってきた相手が「投げるアイテム」かどうかをタグで判定
        if (collision.gameObject.CompareTag("ThrowingItem"))
        {
            // 2. 衝突の勢い（相対速度の大きさ）を取得
            float impactSpeed = collision.relativeVelocity.magnitude;

            // 3. 勢いに応じてリアクションを分岐
            if (impactSpeed >= heavyImpactThreshold)
            {
                // 勢いが強い場合（ダメージ、怒るなど）
                PlayHeavyReaction();
            }
            else
            {
                // 勢いが弱い場合（気づく、軽く振り返るなど）
                PlayLightReaction();
            }
        }
    }

    // 強いリアクションの処理
    private void PlayHeavyReaction()
    {
        Debug.Log("【強】痛い！オブジェクトが強く衝突しました。");
        
        // ここに実際のアニメーション再生やパーティクル生成の処理を書く
    }

    // 弱いリアクションの処理
    private void PlayLightReaction()
    {
        Debug.Log("【弱】コツン。オブジェクトが軽く触れました。");
        
        // 同じく、ここに実際のアニメーション再生やパーティクル生成の処理を書く
    }
}
