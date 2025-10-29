//using System.Collections;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

//public class Goal : MonoBehaviour
//{
//    [Header("Fade Settings")]
//    public float delayBeforeMenu = 2f;   // Level tamamlandıktan sonra bekleme süresi
//    public Color fadeColor = Color.black;
//    public float fadeDuration = 1f;

//    private GameObject fadePanel;
//    private Image fadeImage;

//    void Start()
//    {
//        CreateFadePanel();
//    }

//    void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.GetComponent<RocketController>() != null)
//        {
//            Debug.Log("Goal hit!");
//            GameManager.Instance.WinGame();

//            // 1️⃣ Level numarasını al
//            string sceneName = SceneManager.GetActiveScene().name;
//            int levelNumber = ParseLevelNumber(sceneName);
//            Debug.Log("Current Level: " + levelNumber);

//            // 2️⃣ LevelMenuManager singleton üzerinden tamamla
//            if (LevelMenuManager.instance != null)
//            {
//                LevelMenuManager.instance.CompleteLevel(levelNumber);
//            }
//            else
//            {
//                Debug.LogWarning("LevelMenuManager instance bulunamadı!");
//            }

//            // 3️⃣ Fade + LevelMenu sahnesine dönüş
//            StartCoroutine(ReturnToMenuAfterDelay());
//        }
//    }

//    private int ParseLevelNumber(string sceneName)
//    {
//        // Sahne adları "Level1", "Level2", ... şeklinde olmalı
//        if (sceneName.StartsWith("Level"))
//        {
//            string numberStr = sceneName.Substring(5); // "Level".Length = 5
//            int levelNum = 1;
//            int.TryParse(numberStr, out levelNum);
//            return levelNum;
//        }
//        return 1; // default
//    }

//    private void CreateFadePanel()
//    {
//        fadePanel = new GameObject("FadePanel");
//        fadePanel.SetActive(false); // Başta kapalı

//        Canvas canvas = fadePanel.AddComponent<Canvas>();
//        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
//        canvas.sortingOrder = 999; // En üstte

//        fadePanel.AddComponent<CanvasScaler>();
//        fadePanel.AddComponent<GraphicRaycaster>();

//        GameObject panelGO = new GameObject("Panel");
//        panelGO.transform.SetParent(fadePanel.transform, false);

//        fadeImage = panelGO.AddComponent<Image>();
//        fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0f);

//        RectTransform rt = panelGO.GetComponent<RectTransform>();
//        rt.anchorMin = Vector2.zero;
//        rt.anchorMax = Vector2.one;
//        rt.offsetMin = Vector2.zero;
//        rt.offsetMax = Vector2.zero;
//    }

//    private IEnumerator ReturnToMenuAfterDelay()
//    {
//        yield return new WaitForSeconds(delayBeforeMenu);

//        // Fade panel aç
//        fadePanel.SetActive(true);
//        float timer = 0f;
//        Color c = fadeImage.color;
//        c.a = 0f;
//        while (timer < fadeDuration)
//        {
//            timer += Time.unscaledDeltaTime;
//            c.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
//            fadeImage.color = c;
//            yield return null;
//        }
//        c.a = 1f;
//        fadeImage.color = c;

//        // LevelMenu sahnesine dön
//        SceneManager.LoadScene("WinMenuUP"); // LevelMenu sahne adı
//    }
//}
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    [Header("Fade Settings")]
    public float delayBeforeMenu = 2f;   // Level tamamlandıktan sonra bekleme süresi
    public Color fadeColor = Color.black;
    public float fadeDuration = 1f;

    [Header("Scene Settings")]
    public string nextSceneName = "WinMenuUP";  // Inspector'den sahne adını girebilirsin

    private GameObject fadePanel;
    private Image fadeImage;

    void Start()
    {
        CreateFadePanel();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<RocketController>() != null)
        {
            //Debug.Log("Goal hit!");
            GameManager.Instance.WinGame();

            // 1️⃣ Level numarasını al
            string sceneName = SceneManager.GetActiveScene().name;
            int levelNumber = ParseLevelNumber(sceneName);
            //Debug.Log("Current Level: " + levelNumber);

            // 2️⃣ LevelMenuManager singleton üzerinden tamamla
            if (LevelMenuManager.instance != null)
            {
                LevelMenuManager.instance.CompleteLevel(levelNumber);
            }
            else
            {
                Debug.LogWarning("LevelMenuManager instance bulunamadı!");
            }

            // 3️⃣ Fade + Inspector’dan girilen sahneye geç
            StartCoroutine(ReturnToMenuAfterDelay());
        }
    }

    private int ParseLevelNumber(string sceneName)
    {
        // Sahne adları "Level1", "Level2", ... şeklinde olmalı
        if (sceneName.StartsWith("Level"))
        {
            string numberStr = sceneName.Substring(5); // "Level".Length = 5
            int levelNum = 1;
            int.TryParse(numberStr, out levelNum);
            return levelNum;
        }
        return 1; // default
    }

    private void CreateFadePanel()
    {
        fadePanel = new GameObject("FadePanel");
        fadePanel.SetActive(false); // Başta kapalı

        Canvas canvas = fadePanel.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999; // En üstte

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

    private IEnumerator ReturnToMenuAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeMenu);

        // Fade panel aç
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

        // Inspector’dan girilen sahneye dön
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("Next scene name boş bırakılmış!");
        }
    }
}

