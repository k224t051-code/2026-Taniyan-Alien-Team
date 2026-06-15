using UnityEngine;

// MeshFilterとMeshRendererが自動的に追加されるようにする
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TriangleCreator : MonoBehaviour
{
    private void Start()
    {
        // 新しいメッシュ（形状データ）を作成
        Mesh triangleMesh = new Mesh();

        // 1. 頂点（3つの点の位置）を定義
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0, 1, 0),   // 頂点0：上
            new Vector3(1, -1, 0),  // 頂点1：右下
            new Vector3(-1, -1, 0)  // 頂点2：左下
        };

        // 2. 頂点を結ぶ順番（時計回りに結ぶと「表」の面になる）
        int[] triangles = new int[]
        {
            0, 1, 2
        };

        // メッシュにデータを流し込む
        triangleMesh.vertices = vertices;
        triangleMesh.triangles = triangles;

        // 光の当たり方（法線）を自動計算して綺麗に表示する
        triangleMesh.RecalculateNormals();

        // 自分自身のMeshFilterに作成した三角形をセットする
        GetComponent<MeshFilter>().mesh = triangleMesh;
    }
}