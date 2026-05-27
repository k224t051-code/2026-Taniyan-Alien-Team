using UnityEngine;

public class SwitchFloorGimmick : MonoBehaviour
{
    [SerializeField] private GameObject targetFloor; // 出現・消失させる床オブジェクト
    private int _onSwitchCount = 0;                  // スイッチに乗っているオブジェクトの数

    private void OnCollisionEnter(Collision collision)
    {
        // オブジェクトが侵入したらカウントを増やす
        _onSwitchCount++;
        UpdateFloorState();
    }

    private void OnCollisionExit(Collision collision)
    {
        // オブジェクトが退出したらカウントを減らす
        _onSwitchCount--;
        UpdateFloorState();
    }

    private void UpdateFloorState()
    {
        // 1つ以上乗っていれば床を表示、0なら非表示
        if (_onSwitchCount > 0)
        {
            targetFloor.SetActive(true);
        }
        else
        {
            // カウントがマイナスにならないように念のため0で固定
            _onSwitchCount = 0;
            targetFloor.SetActive(false);
        }
    }
}