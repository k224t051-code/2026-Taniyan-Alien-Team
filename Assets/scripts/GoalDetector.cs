using UnityEngine;

public class GoalDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pot"))
        {
            Debug.Log("ゴールに到達！クリアです。");
            // ここでスコアを確定させる処理を入れる
        }
    }
}