using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Gizliworldbuttonsc : MonoBehaviour
{
    public Image fadePanel;      // Siyah panel (UI Image)
    public float fadeDuration = 1f;

    private bool isTransitioning = false;

    void Start()
    {
        if (fadePanel != null)
        {
            Color c = fadePanel.color;
            c.a = 0f; // Baþlangýçta saydam
            fadePanel.color = c;
            fadePanel.gameObject.SetActive(true); // Panel aktif olmalý
        }
    }

    // Button OnClick çaðýracak
    public void LoadScene(string sceneName)
    {
        if (!isTransitioning)
            StartCoroutine(FadeAndLoad(sceneName));
    }

    IEnumerator FadeAndLoad(string sceneName)
    {
        isTransitioning = true;

        // Fade Out
        yield return StartCoroutine(Fade(0f, 1f));

        // Sahneyi yükle
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        Color c = fadePanel.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            c.a = alpha;
            fadePanel.color = c;
            yield return null;
        }

        c.a = endAlpha;
        fadePanel.color = c;
    }
}
