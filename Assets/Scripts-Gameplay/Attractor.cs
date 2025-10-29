using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Attractor : MonoBehaviour
{
    public static readonly List<Attractor> All = new List<Attractor>();

    [Header("Etki Alanı")]
    public float R1 = 0.5f;
    public float R2 = 2f;
    public float R3 = 5f;

    [Header("Çekim Gücü")]
    [Range(0f, 2f)] public float gravityStrength = 0.5f;
    public float verticalDeadband = 0.1f;

    void OnEnable()
    {
        if (!All.Contains(this)) All.Add(this);
    }

    void OnDisable()
    {
        All.Remove(this);
    }

    public Vector2 Steer(Vector2 pos, Vector2 dir)
    {
        Vector2 toC = (Vector2)transform.position - pos;
        float d = toC.magnitude;

        if (d >= R3 || d <= Mathf.Max(0.01f, R1))
            return Vector2.zero;

        float t = Mathf.InverseLerp(R3, R1, d);

        float deltaY = pos.y - ((Vector2)transform.position).y;
        if (Mathf.Abs(deltaY) <= verticalDeadband)
            return Vector2.zero;

        int spin = (deltaY > 0f) ? +1 : -1;

        Vector2 tangent = Vector2.Perpendicular(toC).normalized * spin;
        float radialWeight = Mathf.InverseLerp(R3, R1, d);
        Vector2 radial = toC.normalized * (0.1f * radialWeight);

        Vector2 raw = tangent + radial;
        return raw.normalized * t * gravityStrength;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.6f); Gizmos.DrawWireSphere(transform.position, R1);
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.6f); Gizmos.DrawWireSphere(transform.position, R2);
        Gizmos.color = new Color(0f, 0.5f, 1f, 0.6f); Gizmos.DrawWireSphere(transform.position, R3);
    }
#endif
}

//using UnityEngine;


//public class Attractor : MonoBehaviour
//{
//    public static readonly List<Attractor> All = new List<Attractor>();

//    [Header("Etki Alanı")]
//    public float R1 = 0.5f;   // çekirdek (etkisiz)
//    public float R3 = 5.0f;   // maksimum etki yarıçapı

//    [Header("Çekim Gücü")]
//    [Range(0f, 2f)] public float gravityStrength = 0.5f; // 👈 Inspector’da ayarlanır
//    public float verticalDeadband = 0.1f;


//    [Header("Uyumluluk için eski alan")]
//    public int spin = +1; // PlanetPlacer için placeholder

//    void OnEnable()
//    {
//        if (!All.Contains(this))
//            All.Add(this);
//    }

//    void OnDisable()
//    {
//        All.Remove(this);
//    }

//    public Vector2 Steer(Vector2 pos, Vector2 dir)
//    {
//        Vector2 toC = (Vector2)transform.position - pos;
//        float d = toC.magnitude;

//        if (d >= R3 || d <= Mathf.Max(0.01f, R1))
//            return Vector2.zero;

//        float t = Mathf.InverseLerp(R3, R1, d);

//        float deltaY = pos.y - ((Vector2)transform.position).y;
//        if (Mathf.Abs(deltaY) <= verticalDeadband)
//            return Vector2.zero;

//        int dynamicSpin = (deltaY > 0f) ? +1 : -1;

//        // Tangent
//        Vector2 tangent = Vector2.Perpendicular(toC).normalized * dynamicSpin;

//        // Radial → yakında güçlü, uzakta zayıf
//        float radialWeight = Mathf.InverseLerp(R3, R1, d); // 0..1
//        radialWeight = 1f - radialWeight; // ters çevir: R3'te 0, R1'de 1
//        Vector2 radial = toC.normalized * (0.2f * radialWeight);

//        Vector2 raw = tangent + radial;

//        return raw.normalized * t * gravityStrength;
//    }




//#if UNITY_EDITOR
//    void OnDrawGizmosSelected()
//    {
//        Gizmos.color = new Color(0, 1, 0, 0.6f);
//        Gizmos.DrawWireSphere(transform.position, R1);
//        Gizmos.color = new Color(0, 0.5f, 1, 0.6f);
//        Gizmos.DrawWireSphere(transform.position, R3);
//    }
//#endif
//}


