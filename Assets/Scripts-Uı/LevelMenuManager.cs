using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelMenuManager : MonoBehaviour
{
    public static LevelMenuManager instance; // Singleton

    [Header("Level Buttons")]
    public Button[] levelButtons;
    public string[] levelSceneNames;

    [Header("Fade Panel")]
    public Color fadeColor = Color.black;
    public float fadeDuration = 1f;

    private GameObject fadePanel;
    private Image fadeImage;

    void Awake()
    {
        // Singleton kontrolü, DontDestroyOnLoad kaldýrýldý
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Duplicate varsa yok et
        }
    }

    void Start()
    {
        // Eðer levelButtons sahnede yoksa parent GameObject'ten otomatik bul
        if (levelButtons == null || levelButtons.Length == 0)
        {
            levelButtons = GameObject.Find("LevelButtonsParent").GetComponentsInChildren<Button>();
        }

        CreateFadePanel();
        UpdateButtons();

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int capturedIndex = i;
            levelButtons[i].onClick.AddListener(() =>
            {
                bool unlocked = PlayerPrefs.GetInt("Level" + (capturedIndex + 1) + "_Unlocked", (capturedIndex == 0 ? 1 : 0)) == 1;
                if (unlocked)
                    StartCoroutine(FadeAndLoadScene(levelSceneNames[capturedIndex]));
            });
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

    private IEnumerator FadeAndLoadScene(string sceneName)
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

        SceneManager.LoadScene(sceneName);
    }

    public void CompleteLevel(int levelNumber)
    {
        PlayerPrefs.SetInt("Level" + levelNumber + "_Unlocked", 1);

        if (levelNumber < levelButtons.Length)
            PlayerPrefs.SetInt("Level" + (levelNumber + 1) + "_Unlocked", 1);

        PlayerPrefs.Save();
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        Color openColor = Color.white;
        Color lockedColor = new Color(1f, 1f, 1f, 0.5f);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i + 1;
            bool unlocked = PlayerPrefs.GetInt("Level" + levelIndex + "_Unlocked", (i == 0 ? 1 : 0)) == 1;

            levelButtons[i].interactable = unlocked;
            levelButtons[i].image.color = unlocked ? openColor : lockedColor;
        }
    }
}
