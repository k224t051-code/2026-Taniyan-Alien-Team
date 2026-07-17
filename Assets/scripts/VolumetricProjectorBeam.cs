using UnityEngine;

/// <summary>
/// プロジェクターのレンズからUIキャンバスの4隅に向かって、
/// 動的に伸縮する美しい「光のビーム（ボリュメトリックライト）」の3Dメッシュを生成するスクリプト。
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VolumetricProjectorBeam : MonoBehaviour
{
    [Header("接続対象の設定")]
    [Tooltip("光の発信源となる宇宙船の丸いレンズ（空のオブジェクトでも可）")]
    [SerializeField] private Transform projectorLens;
    
    [Tooltip("光の投影先となるUIのCanvas（RectTransform）")]
    [SerializeField] private RectTransform targetCanvas;

    [Header("ビームの見た目設定")]
    [Tooltip("発信源（レンズ）の光の口径サイズ")]
    [SerializeField] private float lensSize = 0.15f;

    private Mesh filterMesh;
    private Vector3[] worldCorners = new Vector3[4];
    private Vector3[] vertices = new Vector3[8];
    private int[] triangles;
    private Vector2[] uvs;

    private void Start()
    {
        // 制御用のメッシュを作成してMeshFilterに割り当て
        filterMesh = new Mesh();
        filterMesh.name = "ProjectorBeamMesh";
        GetComponent<MeshFilter>().mesh = filterMesh;

        // VR用に内側からも外側からも光が見えるよう、両面（ダブルサイド）のポリゴンを定義
        InitializeMeshStructure();
    }

    private void InitializeMeshStructure()
    {
        // 頂点数: レンズ側4点 + キャンバス側4点 = 8点
        uvs = new Vector2[8];

        // UV座標の設定 (V方向を0から1にすることで、根本から先端にかけてのグラデーション表現を可能にします)
        // レンズ側 (V = 0)
        uvs[0] = new Vector2(0f, 0f);
        uvs[1] = new Vector2(0f, 1f);
        uvs[2] = new Vector2(1f, 1f);
        uvs[3] = new Vector2(1f, 0f);
        // キャンバス側 (V = 1)
        uvs[4] = new Vector2(0f, 0f);
        uvs[5] = new Vector2(0f, 1f);
        uvs[6] = new Vector2(1f, 1f);
        uvs[7] = new Vector2(1f, 0f);

        // 重なりによる斜めの線（ノイズ）を防ぐため、内側の面の描画を削除
        // ※裏面表示はマテリアルの設定（Cull Off / Render Face: Both）で行います
        triangles = new int[]
        {
            // --- 外側を向く面 (Clockwise) ---
            0, 4, 5,  0, 5, 1, // 左面
            1, 5, 6,  1, 6, 2, // 上面
            2, 6, 7,  2, 7, 3, // 右面
            3, 7, 4,  3, 4, 0  // 下面
        };
    }

    private void LateUpdate()
    {
        if (projectorLens == null || targetCanvas == null) return;

        // 1. キャンバスの現在のワールド空間における4隅の座標を取得
        targetCanvas.GetWorldCorners(worldCorners);

        // 2. レンズの向き（Up, Right）に基づいて、レンズ側の矩形4隅を計算
        Vector3 lensCenter = projectorLens.position;
        Vector3 upOffset = projectorLens.up * (lensSize * 0.5f);
        Vector3 rightOffset = projectorLens.right * (lensSize * 0.5f);

        Vector3[] lensCorners = new Vector3[4];
        lensCorners[0] = lensCenter - rightOffset - upOffset; // 左下
        lensCorners[1] = lensCenter - rightOffset + upOffset; // 左上
        lensCorners[2] = lensCenter + rightOffset + upOffset; // 右上
        lensCorners[3] = lensCenter + rightOffset - upOffset; // 右下

        // 3. すべての座標をこのスクリプトがついているオブジェクトのローカル座標に変換して格納
        for (int i = 0; i < 4; i++)
        {
            vertices[i] = transform.InverseTransformPoint(lensCorners[i]);      // 0~3: レンズ側
            vertices[i + 4] = transform.InverseTransformPoint(worldCorners[i]); // 4~7: キャンバス側
        }

        // 4. メッシュを更新して再描画
        filterMesh.vertices = vertices;
        filterMesh.triangles = triangles;
        filterMesh.uv = uvs;
        filterMesh.RecalculateBounds();
        filterMesh.RecalculateNormals();
    }
}