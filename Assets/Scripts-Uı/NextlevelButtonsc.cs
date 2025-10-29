using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class BlackHoleSceneTransition : MonoBehaviour
{
    [Header("Harfler")]
    public Transform lettersParent;   // Harflerin parent GameObject'i
    public Transform blackHole;       // Karadeliðin Transform’u

    [Header("Efekt Ayarlarý")]
    public float absorbSpeed = 3f;        // merkeze çekilme hýzý
    public float rotationSpeed = 360f;    // spiral dönüþ hýzý
    public float destroyDistance = 0.2f;  // yok olma mesafesi

    [Header("Sahne Geçiþi")]
    public string nextSceneName = "Level2";
    public float fadeDuration = 1f;   // Fade süresi

    private int lettersRemaining;
    private Image fadeImage;

    public void StartAbsorption()
    {
        lettersRemaining = lettersParent.childCount;

        foreach (Transform child in lettersParent)
        {
            var tmp = child.GetComponent<TextMeshProUGUI>();
            if (tmp != null)
            {
                StartCoroutine(MoveToBlackHole(child));
            }
        }
    }

    private IEnumerator MoveToBlackHole(Transform letter)
    {
        Vector3 offset = letter.position - blackHole.position;

        while (letter != null && offset.magnitude > destroyDistance)
        {
            // Merkeze yaklaþma
            offset = Vector3.Lerp(offset, Vector3.zero, Time.deltaTime * absorbSpeed);

            // Spiral dönüþ
            offset = Quaternion.Euler(0, 0, rotationSpeed * Time.deltaTime) * offset;

            // Pozisyonu güncelle
            letter.position = blackHole.position + offset;

            // Küçülme efekti
            letter.localScale = Vector3.Lerp(letter.localScale, Vector3.zero, Time.deltaTime * 2f);

            yield return null;
        }

        if (letter != null)
            Destroy(letter.gameObject);

        lettersRemaining--;

        // Tüm harfler yok olduðunda fade baþlat
        if (lettersRemaining <= 0)
            StartCoroutine(FadeAndLoadScene());
    }

    private IEnumerator FadeAndLoadScene()
    {
        CreateFadePanel();

        // Fade-in (ekraný karart)
        fadeImage.canvasRenderer.SetAlpha(0f);
        fadeImage.CrossFadeAlpha(1f, fadeDuration, false);

        yield return new WaitForSeconds(fadeDuration);

        // Asenkron sahne yükle
        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneName);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
            yield return null;

        op.allowSceneActivation = true;

        // Yeni sahne açýldýðýnda fade-out yap
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // event temizle

        if (fadeImage != null)
        {
            fadeImage.CrossFadeAlpha(0f, fadeDuration, false);
            Destroy(fadeImage.transform.parent.gameObject, fadeDuration + 0.1f);
        }
    }

    private void CreateFadePanel()
    {
        if (fadeImage != null) return;

        // Canvas oluþtur
        GameObject canvasGO = new GameObject("FadeCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999; // her þeyin üstünde

        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        // Panel oluþtur
        GameObject panelGO = new GameObject("FadePanel");
        panelGO.transform.SetParent(canvasGO.transform, false);

        fadeImage = panelGO.AddComponent<Image>();
        fadeImage.color = Color.black;

        RectTransform rt = panelGO.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }
}
