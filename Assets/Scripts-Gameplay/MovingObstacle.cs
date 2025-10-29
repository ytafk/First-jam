using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MovingObstacle : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveDistance = 2f;     // Yukarı-aşağı toplam mesafe
    public float moveSpeed = 2f;        // Hız
    public bool startUpwards = true;    // Başlangıç yönü

    [Header("Fade Settings")]
    public float fadeDuration = 1f;
    public Color fadeColor = Color.black;

    private Vector3 startPos;
    private int direction = 1;

    private GameObject fadePanel;
    private Image fadeImage;

    void Start()
    {
        // Hareket için başlangıç pozisyonu
        startPos = transform.position;
        direction = startUpwards ? +1 : -1;

        // Fade paneli oluştur
        CreateFadePanel();
    }

    void Update()
    {
        // Sinüs dalgası ile yukarı-aşağı hareket
        float offset = Mathf.Sin(Time.time * moveSpeed) * (moveDistance / 2f);
        transform.position = startPos + new Vector3(0, offset, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<RocketController>() != null)
        {
            GameManager.Instance.LoseGame();
            Debug.Log("You Died");

            // Fade + LoseMenu sahnesine geç
            StartCoroutine(FadeAndLoadLoseMenu());
        }
    }

    private void CreateFadePanel()
    {
        fadePanel = new GameObject("FadePanel");
        fadePanel.SetActive(false);

        Canvas canvas = fadePanel.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;

        fadePanel.AddComponent<CanvasScaler>();
        fadePanel.AddComponent<GraphicRaycaster>();

        GameObject panelGO = new GameObject("Panel");
        panelGO.transform.SetParent(fadePanel.transform, false);

        fadeImage = panelGO.AddComponent<Image>();
        fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0f);

        RectTransform rt = panelGO.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    private IEnumerator FadeAndLoadLoseMenu()
    {
        fadePanel.SetActive(true);
        float timer = 0f;
        Color c = fadeImage.color;
        c.a = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            c.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = 1f;
        fadeImage.color = c;

        SceneManager.LoadScene("LoseMenu"); // LoseMenu sahnesi
    }
}
