using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlanetRings : MonoBehaviour
{
    private LineRenderer lrR1;
    private LineRenderer lrR2;
    private LineRenderer lrR3;

    [Header("Ring Settings")]
    public int segments = 64;
    public float lineWidth = 0.05f;
    public Material lineMaterial; // boşsa Sprites/Default kullanılır

    // SADECE sahne instance'ında çalışalım
    private bool IsInScene() => gameObject.scene.IsValid();

    private LineRenderer CreateLR(string name)
    {
        if (!IsInScene()) return null; // prefab asset'te asla oluşturma

        var go = new GameObject(name);
        go.transform.SetParent(transform, false);

        var lr = go.AddComponent<LineRenderer>();
        lr.loop = true;
        lr.useWorldSpace = false;
        lr.widthMultiplier = lineWidth;
        lr.material = lineMaterial != null
            ? lineMaterial
            : new Material(Shader.Find("Sprites/Default"));

        return lr;
    }

    private void Ensure()
    {
        if (!IsInScene()) return;
        if (lrR1 == null) lrR1 = CreateLR("Ring_R1");
        if (lrR2 == null) lrR2 = CreateLR("Ring_R2");
        if (lrR3 == null) lrR3 = CreateLR("Ring_R3");
    }

    // YENİ: 7 parametreli Set (profil renkleriyle)
    public void Set(float R1, float R2, float R3, float ringWidth, Color c1, Color c2, Color c3)
    {
        Ensure();
        DrawCircle(lrR1, R1, c1);
        DrawCircle(lrR2, R2, c2);
        DrawCircle(lrR3, R3, c3);
    }

    // OVERLOAD: Eski 4 parametreli çağrılar bozulmasın (default renkler)
    public void Set(float R1, float R2, float R3, float ringWidth)
    {
        Set(R1, R2, R3, ringWidth,
            new Color(0f, 1f, 0f, 0.35f),
            new Color(1f, 0.5f, 0f, 0.35f),
            new Color(0f, 0.5f, 1f, 0.35f));
    }

    // YENİ: PlanetPlacer eski kodu SetColors çağırıyorsa devam etsin
    public void SetColors(Color c1, Color c2, Color c3)
    {
        Ensure();
        if (lrR1 != null) { lrR1.startColor = c1; lrR1.endColor = c1; }
        if (lrR2 != null) { lrR2.startColor = c2; lrR2.endColor = c2; }
        if (lrR3 != null) { lrR3.startColor = c3; lrR3.endColor = c3; }
    }

    private void DrawCircle(LineRenderer lr, float radius, Color col)
    {
        if (lr == null) return;

        lr.positionCount = segments;
        lr.startColor = col;
        lr.endColor = col;

        float dθ = 2f * Mathf.PI / segments;
        for (int i = 0; i < segments; i++)
        {
            float θ = i * dθ;
            lr.SetPosition(i, new Vector3(Mathf.Cos(θ) * radius, Mathf.Sin(θ) * radius, 0f));
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Prefab asset içinde çalışmayalım
        if (PrefabUtility.IsPartOfPrefabAsset(this)) return;
    }
#endif
}


//using UnityEngine;

//public class PlanetRings : MonoBehaviour
//{
//    [SerializeField] private int segments = 96;
//    private LineRenderer lr1, lr2, lr3;

//    void Start() { Ensure(); }

//    LineRenderer CreateLR(string name)
//    {
//        var go = new GameObject(name);
//        go.transform.SetParent(transform, false);
//        var lr = go.AddComponent<LineRenderer>();
//        lr.useWorldSpace = false;
//        lr.loop = true;
//        lr.material = new Material(Shader.Find("Sprites/Default"));
//        lr.sortingOrder = -1;
//        lr.alignment = LineAlignment.View;
//        lr.positionCount = segments;
//        lr.widthMultiplier = 0.03f;
//        return lr;
//    }

//    void Ensure()
//    {
//        if (lr1 == null) lr1 = CreateLR("Ring_R1");
//        if (lr2 == null) lr2 = CreateLR("Ring_R2");
//        if (lr3 == null) lr3 = CreateLR("Ring_R3");
//    }

//    public void Set(float R1, float R2, float R3, float width)
//    {
//        Ensure();
//        lr1.widthMultiplier = lr2.widthMultiplier = lr3.widthMultiplier = width;
//        BuildCircle(lr1, R1); BuildCircle(lr2, R2); BuildCircle(lr3, R3);
//    }

//    public void SetColors(Color c1, Color c2, Color c3)
//    {
//        Ensure();
//        lr1.startColor = lr1.endColor = c1;
//        lr2.startColor = lr2.endColor = c2;
//        lr3.startColor = lr3.endColor = c3;
//    }

//    void BuildCircle(LineRenderer lr, float r)
//    {
//        if (r <= 0f) { lr.positionCount = 0; return; }
//        if (lr.positionCount != segments) lr.positionCount = segments;
//        for (int i = 0; i < segments; i++)
//        {
//            float t = (i / (float)segments) * Mathf.PI * 2f;
//            lr.SetPosition(i, new Vector3(Mathf.Cos(t) * r, Mathf.Sin(t) * r, 0f));
//        }
//    }
//}
