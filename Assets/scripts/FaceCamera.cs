using UnityEngine;

// 吹き出し（Canvas）を常にカメラに向かせるスクリプト
public class FaceCamera : MonoBehaviour
{
    private Transform mainCamera;

    private void Start()
    {
        // メインカメラ（プレイヤーの視点）を探して保存
        mainCamera = Camera.main.transform;
    }

    private void LateUpdate()
    {
        // 常にカメラの方向を向く
        if (mainCamera != null)
        {
            transform.LookAt(transform.position + mainCamera.rotation * Vector3.forward, mainCamera.rotation * Vector3.up);
        }
    }
}