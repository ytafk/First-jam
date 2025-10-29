using UnityEngine;
using TMPro;

public class TMPFadeIn : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public float duration = 2f;

    void Start()
    {
        textMesh.alpha = 0;
        StartCoroutine(FadeIn());
    }

    System.Collections.IEnumerator FadeIn()
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            textMesh.alpha = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }
    }
}
