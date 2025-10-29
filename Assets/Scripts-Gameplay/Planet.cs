using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Attractor))]
public class Planet : MonoBehaviour
{
    public PlanetProfile profile;

    [HideInInspector] public Attractor attractor;
    private SpriteRenderer spriteRenderer;
    private PlanetRings rings;

    void Awake()
    {
        attractor = GetComponent<Attractor>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rings = GetComponentInChildren<PlanetRings>();
    }

    void OnValidate()
    {
#if UNITY_EDITOR
        if (PrefabUtility.IsPartOfPrefabAsset(this)) return;
#endif
        Apply();
    }

    public void Apply()
    {
        if (profile == null) return;

        if (attractor == null) attractor = GetComponent<Attractor>();
        attractor.R1 = profile.R1;
        attractor.R2 = profile.R2;
        attractor.R3 = profile.R3;
        attractor.gravityStrength = profile.gravityStrength;
        attractor.verticalDeadband = profile.verticalDeadband;

        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null && profile.sprite != null)
            spriteRenderer.sprite = profile.sprite;

        if (rings == null) rings = GetComponentInChildren<PlanetRings>();
        if (rings != null)
        {
            rings.Set(
                profile.R1, profile.R2, profile.R3, profile.ringWidth,
                profile.ringColorR1, profile.ringColorR2, profile.ringColorR3
            );
        }
    }
}



//using UnityEngine;

//[RequireComponent(typeof(Attractor))]
//[RequireComponent(typeof(PlanetRings))]
//public class Planet : MonoBehaviour
//{
//    public PlanetProfile profile;
//    [HideInInspector] public Attractor attractor;
//    [HideInInspector] public PlanetRings rings;

//    private void Reset()
//    {
//        attractor = GetComponent<Attractor>();
//        rings = GetComponent<PlanetRings>();
//    }

//    private void Awake()
//    {
//        Cache();
//        Apply();
//    }

//    //private void OnValidate()
//    //{
//    //    Cache();
//    //    Apply();
//    //}

//    private void Cache()
//    {
//        if (!attractor) attractor = GetComponent<Attractor>();
//        if (!rings) rings = GetComponent<PlanetRings>();
//    }

//    public void Apply()
//    {
//        if (profile == null || attractor == null || rings == null) return;

//        // Davranış parametreleri
//        attractor.R1 = profile.R1;
//        attractor.R3 = profile.R3;



//        // Görsel halkalar
//        rings.Set(profile.R1, profile.R2, profile.R3, profile.ringWidth);
//        rings.SetColors(profile.ringColorR1, profile.ringColorR2, profile.ringColorR3);
//    }
//}
