using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private float speed = 2.0f;    // 往復の速さ
    [SerializeField] private float distance = 3.0f; // 片道の移動距離

    private Vector3 _startPosition;

    void Start()
    {
        // 起動時の位置を基準点として保存
        _startPosition = transform.position;
    }

    void Update()
    {
        // サイン波を使って左右（X軸）に往復移動させる
        float offset = Mathf.Sin(Time.time * speed) * distance;
        transform.position = _startPosition + new Vector3(offset, 0, 0);
    }

    // --- 乗っている物体を一緒に動かすための処理 ---

    private void OnTriggerEnter(Collider other)
    {
        // ポット（またはプレイヤーなど）が接触したら、その物体を床の子要素にする
        // これにより、床の移動に合わせて物体も移動するようになる
        if (other.CompareTag("Pot") || other.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 物体が床から離れたら、親子関係を解除する
        if (other.CompareTag("Pot") || other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}