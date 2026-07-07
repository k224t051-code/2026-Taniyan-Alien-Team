using UnityEngine;

public class LeverCheck : MonoBehaviour
{
    public Transform lever;
    public float triggerAngle = 60f;
    public LeverPuzzleController puzzleController;

    private bool fired = false;
    private float initialAngle = 0f;

    void Start()
    {
        // プレイ開始時のレバー角度を記録
        initialAngle = NormalizeAngle(lever.localEulerAngles.x);
    }

    void Update()
    {
        if (lever == null)
        {
            return;
        }

        float currentAngle = NormalizeAngle(lever.localEulerAngles.x);
        
        // 初期位置からの相対回転を計算
        float relativeDelta = currentAngle - initialAngle;
        
        // -180〜180の範囲内で正規化
        if (relativeDelta > 180f) relativeDelta -= 360f;
        if (relativeDelta < -180f) relativeDelta += 360f;

        Debug.Log($"Current: {currentAngle:F2}° | Initial: {initialAngle:F2}° | Delta: {relativeDelta:F2}° | Trigger: {triggerAngle}° | Fired: {fired}");

        if (!fired && relativeDelta >= triggerAngle)
        {
            Debug.Log("Lever triggered!");
            OnLeverTriggered();
            fired = true;
        }

        // もし「戻したら再発火したい」ならこれ追加
        if (relativeDelta < triggerAngle - 10f)
        {
            fired = false;
        }
    }

    private float NormalizeAngle(float angle)
    {
        // 0〜360 → -180〜180に変換
        if (angle > 180f) angle -= 360f;
        return angle;
    }

    void OnLeverTriggered()
    {
        if (puzzleController == null)
        {
            puzzleController = FindObjectOfType<LeverPuzzleController>();

            if (puzzleController == null)
            {
                Debug.LogError("LeverCheck: puzzleController is not assigned and could not be found automatically.");
                return;
            }
        }

        Debug.Log($"LeverCheck: calling TryClear on {puzzleController.name}");
        puzzleController.TryClear();
    }
}
