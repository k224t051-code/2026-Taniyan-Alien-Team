using UnityEngine;
using System.Collections.Generic;

public class AIjen02 : MonoBehaviour
{
    public GameObject centerPrefab;
    public GameObject orbitPrefab;
    public int planetCount = 8;
    public float minRadius = 3f;
    public float maxRadius = 8f;
    public float minSize = 0.3f;
    public float maxSize = 1.2f;
    public float minHeight = 0.2f;
    public float maxHeight = 1.0f;
    public float minSpeed = 10f;
    public float maxSpeed = 60f;

    private List<GameObject> planets = new List<GameObject>();
    private List<Vector3> baseVectors = new List<Vector3>();
    private List<float> planetAngle = new List<float>();
    private List<float> planetSpeed = new List<float>();
    private List<int> planetDirection = new List<int>();
    private List<Vector3> planetAxis = new List<Vector3>();
    private GameObject centerObj;
    private float colorChangeTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 中心球（太陽）生成（親をこのGameObjectに設定）
        centerObj = Instantiate(centerPrefab, transform.position, Quaternion.identity, transform);
        // 中心にトレイル追加
        var centerTrail = centerObj.AddComponent<TrailRenderer>();
        centerTrail.time = 2f;
        centerTrail.startWidth = 0.3f;
        centerTrail.endWidth = 0.05f;
        centerTrail.material = new Material(Shader.Find("Sprites/Default"));
        centerTrail.startColor = Color.HSVToRGB(Random.value, 1f, 1f);
        centerTrail.endColor = Color.HSVToRGB(Random.value, 1f, 1f);

        // 周囲の球（惑星）生成
        for (int i = 0; i < planetCount; i++)
        {
            float angle = i * Mathf.PI * 2f / planetCount;
            float radius = Random.Range(minRadius, maxRadius);
            float y = Random.Range(minHeight, maxHeight);
            Vector3 baseVec = new Vector3(Mathf.Cos(angle) * radius, y, Mathf.Sin(angle) * radius);
            // ランダムな回転軸（正規化）
            Vector3 axis = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            float size = Random.Range(minSize, maxSize);
            float speed = Random.Range(minSpeed, maxSpeed);
            int direction = Random.value > 0.5f ? 1 : -1;
            GameObject planet = Instantiate(orbitPrefab, centerObj.transform.position + baseVec, Quaternion.identity);
            // 色をランダムに設定
            var renderer = planet.GetComponent<Renderer>();
            if (renderer != null)
            {
                float h = Random.value;
                float s = Random.Range(0.7f, 1f);
                float v = Random.Range(0.7f, 1f);
                renderer.material.color = Color.HSVToRGB(h, s, v);
            }
            // 惑星にトレイル追加
            var trail = planet.AddComponent<TrailRenderer>();
            trail.time = 2f;
            trail.startWidth = 0.15f;
            trail.endWidth = 0.03f;
            trail.material = new Material(Shader.Find("Sprites/Default"));
            trail.startColor = renderer != null ? renderer.material.color : Color.white;
            trail.endColor = Color.HSVToRGB(Random.value, 1f, 1f);
            planet.transform.localScale = Vector3.one * size;
            planets.Add(planet);
            baseVectors.Add(baseVec);
            planetAngle.Add(0f);
            planetSpeed.Add(speed);
            planetDirection.Add(direction);
            planetAxis.Add(axis);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 中心オブジェクトが消えていたら再生成
        if (centerObj == null)
        {
            centerObj = Instantiate(centerPrefab, transform.position, Quaternion.identity, transform);
        }
        // 中心オブジェクトのカラーを毎秒ランダムで変化
        colorChangeTimer += Time.deltaTime;
        if (colorChangeTimer >= 1f)
        {
            var renderer = centerObj.GetComponent<Renderer>();
            if (renderer != null)
            {
                float h = Random.value;
                float s = Random.Range(0.7f, 1f);
                float v = Random.Range(0.7f, 1f);
                renderer.material.color = Color.HSVToRGB(h, s, v);
            }
            colorChangeTimer = 0f;
        }
        for (int i = 0; i < planets.Count; i++)
        {
            planetAngle[i] += planetSpeed[i] * planetDirection[i] * Time.deltaTime;
            Quaternion rot = Quaternion.AngleAxis(planetAngle[i], planetAxis[i]);
            Vector3 pos = centerObj.transform.position + rot * baseVectors[i];
            planets[i].transform.position = pos;
        }
    }
}
