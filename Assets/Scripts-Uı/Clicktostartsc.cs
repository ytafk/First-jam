using UnityEngine;
using TMPro;

public class SmoothBlinkingTextTMP : MonoBehaviour
{
    public TMP_Text text;
    public float blinkSpeed = 2f;

    void Update()
    {
        if (text != null)
        {
            float alpha = (Mathf.Sin(Time.time * blinkSpeed) + 1f) / 2f;
            Color c = text.color;
            c.a = alpha;
            text.color = c;
        }
    }
}
