using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Planet Profile")]
public class PlanetProfile : ScriptableObject
{
    [Header("Etki Alanı")]
    public float R1 = 0.5f;
    public float R2 = 2f;
    public float R3 = 5f;
    public float ringWidth = 0.5f;

    [Header("Çekim")]
    public float gravityStrength = 0.5f;
    public float verticalDeadband = 0.1f;

    [Header("Görsel")]
    public Sprite sprite;

    [Header("Ring Colors")]
    public Color ringColorR1 = new Color(0f, 1f, 0f, 0.35f);    // yeşil
    public Color ringColorR2 = new Color(1f, 0.5f, 0f, 0.35f);  // turuncu
    public Color ringColorR3 = new Color(0f, 0.5f, 1f, 0.35f);  // mavi
}



//using UnityEngine;

//[CreateAssetMenu(fileName = "PlanetProfile", menuName = "Planets/Profile")]
//public class PlanetProfile : ScriptableObject
//{
//    public string displayName = "Medium";
//    public float R1 = 0.6f, R2 = 2.5f, R3 = 4.0f;   // 3 halka yarıçapı
//    [Range(0f, 1f)] public float tangentialToRadial = 0.35f;
//    [Range(0f, 0.5f)] public float outwardBias = 0.1f;
//    public int spin = +1; // +1 saat yönü, -1 tersi

//    // Halka görselleri (silik)
//    public Color ringColorR1 = new Color(0f, 1f, 0f, 0.12f);
//    public Color ringColorR2 = new Color(1f, 0.5f, 0f, 0.18f);
//    public Color ringColorR3 = new Color(0f, 0.5f, 1f, 0.10f);
//    public float ringWidth = 0.03f;

//    // İstersen Inspector'dan tanımla; boşsa kod smoothstep kullanır
//    public AnimationCurve falloff;
//}
