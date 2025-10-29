//using UnityEngine;
//using TMPro;
//using System.Collections;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;
//using System.Collections.Generic;

//public class Typescriptmanagersc : MonoBehaviour
//{
//    [System.Serializable]
//    public class TextEntry
//    {
//        public TMP_Text textComponent;
//        public string fullText;
//        public float typeDelay = 0.05f;
//        public float displayDuration = 2f;
//        public float fadeDuration = 1f;
//    }

//    [System.Serializable]
//    public class ImageEntry
//    {
//        public Image imageComponent;
//        public float fadeDuration = 1f;
//    }

//    [Header("Typewriter Settings")]
//    public float defaultTypeDelay = 0.05f;
//    public float defaultDisplayDuration = 2f;
//    public float defaultFadeDuration = 1f;
//    public float delayBeforeNextScene = 2f;

//    [Header("Fade Panel Settings")]
//    public Color fadeColor = Color.black;
//    public float fadeDuration = 1f;

//    [Header("Audio Settings")]
//    public AudioSource audioSource;
//    public AudioClip typeSound;
//    [Range(0f, 0.5f)]
//    public float pitchRandomness = 0.1f;

//    [Header("Scene Settings")]
//    public string nextSceneName;

//    [Header("Backgrounds")]
//    public Image generalBackground;
//    private List<ImageEntry> images = new List<ImageEntry>();

//    private TextEntry[] texts;
//    private GameObject fadePanel;
//    private UnityEngine.UI.Image fadeImage;

//    private void Start()
//    {
//        Transform[] children = GetComponentsInChildren<Transform>(true);
//        List<TextEntry> textList = new List<TextEntry>();

//        foreach (Transform child in children)
//        {
//            TMP_Text tmp = child.GetComponent<TMP_Text>();
//            if (tmp != null)
//            {
//                TextEntry entry = new TextEntry
//                {
//                    textComponent = tmp,
//                    fullText = tmp.text,
//                    typeDelay = defaultTypeDelay,
//                    displayDuration = defaultDisplayDuration,
//                    fadeDuration = defaultFadeDuration
//                };
//                textList.Add(entry);
//                tmp.text = "";
//            }

//            Image img = child.GetComponent<Image>();
//            if (img != null && img != generalBackground)
//            {
//                ImageEntry entry = new ImageEntry
//                {
//                    imageComponent = img,
//                    fadeDuration = defaultFadeDuration
//                };
//                images.Add(entry);

//                Color c = img.color;
//                c.a = 0f;
//                img.color = c;
//            }
//        }

//        texts = textList.ToArray();

//        CreateFadePanel();

//        fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 1f);
//        StartCoroutine(FadeInScreen());
//    }

//    private void CreateFadePanel()
//    {
//        fadePanel = new GameObject("FadePanel");
//        Canvas canvas = fadePanel.AddComponent<Canvas>();
//        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
//        canvas.sortingOrder = 999;

//        fadePanel.AddComponent<CanvasScaler>();
//        fadePanel.AddComponent<GraphicRaycaster>();

//        GameObject panelGO = new GameObject("Panel");
//        panelGO.transform.SetParent(fadePanel.transform, false);

//        fadeImage = panelGO.AddComponent<UnityEngine.UI.Image>();
//        fadeImage.color = fadeColor;

//        RectTransform rt = panelGO.GetComponent<RectTransform>();
//        rt.anchorMin = Vector2.zero;
//        rt.anchorMax = Vector2.one;
//        rt.offsetMin = Vector2.zero;
//        rt.offsetMax = Vector2.zero;
//    }

//    private IEnumerator FadeInScreen()
//    {
//        float timer = 0f;
//        Color c = fadeImage.color;
//        while (timer < fadeDuration)
//        {
//            timer += Time.unscaledDeltaTime;
//            c.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
//            fadeImage.color = c;
//            yield return null;
//        }
//        c.a = 0f;
//        fadeImage.color = c;

//        StartCoroutine(RunSequence());
//    }

//    private IEnumerator RunSequence()
//    {
//        int textIndex = 0;
//        int imageIndex = 0;

//        while (textIndex < texts.Length || imageIndex < images.Count)
//        {
//            if (textIndex < texts.Length)
//            {
//                yield return StartCoroutine(ShowTextWithAudioAndFade(texts[textIndex]));
//                textIndex++;
//            }

//            if (imageIndex < images.Count)
//            {
//                yield return StartCoroutine(ShowImage(images[imageIndex]));
//                yield return new WaitForSeconds(defaultDisplayDuration);
//                yield return StartCoroutine(HideImage(images[imageIndex]));
//                imageIndex++;
//            }
//        }

//        yield return new WaitForSeconds(delayBeforeNextScene);

//        yield return StartCoroutine(FadeOutScreen());
//        SceneManager.LoadScene(nextSceneName);
//    }

//    private IEnumerator ShowTextWithAudioAndFade(TextEntry entry)
//    {
//        entry.textComponent.color = new Color(entry.textComponent.color.r,
//                                              entry.textComponent.color.g,
//                                              entry.textComponent.color.b, 1f);

//        entry.textComponent.text = "";

//        for (int i = 0; i < entry.fullText.Length; i++)
//        {
//            entry.textComponent.text += entry.fullText[i];

//            if (audioSource != null && typeSound != null)
//            {
//                audioSource.pitch = 1f + Random.Range(-pitchRandomness, pitchRandomness);
//                audioSource.PlayOneShot(typeSound);
//            }

//            yield return new WaitForSeconds(entry.typeDelay);
//        }

//        yield return new WaitForSeconds(entry.displayDuration);

//        float timer = 0f;
//        Color original = entry.textComponent.color;
//        while (timer < entry.fadeDuration)
//        {
//            timer += Time.unscaledDeltaTime;
//            float alpha = Mathf.Lerp(1f, 0f, timer / entry.fadeDuration);
//            entry.textComponent.color = new Color(original.r, original.g, original.b, alpha);
//            yield return null;
//        }
//        entry.textComponent.color = new Color(original.r, original.g, original.b, 0f);
//    }

//    private IEnumerator ShowImage(ImageEntry entry)
//    {
//        float timer = 0f;
//        Color original = entry.imageComponent.color;
//        while (timer < entry.fadeDuration)
//        {
//            timer += Time.unscaledDeltaTime;
//            float alpha = Mathf.Lerp(0f, 1f, timer / entry.fadeDuration);
//            entry.imageComponent.color = new Color(original.r, original.g, original.b, alpha);
//            yield return null;
//        }
//        entry.imageComponent.color = new Color(original.r, original.g, original.b, 1f);
//    }

//    private IEnumerator HideImage(ImageEntry entry)
//    {
//        float timer = 0f;
//        Color original = entry.imageComponent.color;
//        while (timer < entry.fadeDuration)
//        {
//            timer += Time.unscaledDeltaTime;
//            float alpha = Mathf.Lerp(1f, 0f, timer / entry.fadeDuration);
//            entry.imageComponent.color = new Color(original.r, original.g, original.b, alpha);
//            yield return null;
//        }
//        entry.imageComponent.color = new Color(original.r, original.g, original.b, 0f);
//    }

//    private IEnumerator FadeOutScreen()
//    {
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
//    }
//}


using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class Typescriptmanagersc : MonoBehaviour
{
    [System.Serializable]
    public class TextEntry
    {
        public TMP_Text textComponent;
        public string fullText;
        public float typeDelay = 0.05f;
        public float displayDuration = 2f;
        public float fadeDuration = 1f;
    }

    [System.Serializable]
    public class ImageEntry
    {
        public Image imageComponent;
        public float fadeDuration = 1f;
    }

    [Header("Typewriter Settings")]
    public float defaultTypeDelay = 0.05f;
    public float defaultDisplayDuration = 2f;
    public float defaultFadeDuration = 1f;
    public float delayBeforeNextScene = 2f;

    [Header("Fade Panel Settings")]
    public Color fadeColor = Color.black;
    public float fadeDuration = 1f;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip typeSound;
    [Range(0f, 0.5f)]
    public float pitchRandomness = 0.1f;

    [Header("Scene Settings")]
    public string nextSceneName;

    [Header("Backgrounds")]
    public Image generalBackground;
    private List<ImageEntry> images = new List<ImageEntry>();
    private TextEntry[] texts;
    private GameObject fadePanel;
    private UnityEngine.UI.Image fadeImage;

    private bool skipScene = false; // Button tıklaması için flag

    private void Start()
    {
        Transform[] children = GetComponentsInChildren<Transform>(true);
        List<TextEntry> textList = new List<TextEntry>();

        foreach (Transform child in children)
        {
            TMP_Text tmp = child.GetComponent<TMP_Text>();
            if (tmp != null)
            {
                TextEntry entry = new TextEntry
                {
                    textComponent = tmp,
                    fullText = tmp.text,
                    typeDelay = defaultTypeDelay,
                    displayDuration = defaultDisplayDuration,
                    fadeDuration = defaultFadeDuration
                };
                textList.Add(entry);
                tmp.text = "";
            }

            Image img = child.GetComponent<Image>();
            if (img != null && img != generalBackground)
            {
                ImageEntry entry = new ImageEntry
                {
                    imageComponent = img,
                    fadeDuration = defaultFadeDuration
                };
                images.Add(entry);

                Color c = img.color;
                c.a = 0f;
                img.color = c;
            }
        }

        texts = textList.ToArray();

        CreateFadePanel();

        fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 1f);
        StartCoroutine(FadeInScreen());
    }

    private void CreateFadePanel()
    {
        fadePanel = new GameObject("FadePanel");
        Canvas canvas = fadePanel.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;

        fadePanel.AddComponent<CanvasScaler>();
        fadePanel.AddComponent<GraphicRaycaster>();

        GameObject panelGO = new GameObject("Panel");
        panelGO.transform.SetParent(fadePanel.transform, false);

        fadeImage = panelGO.AddComponent<UnityEngine.UI.Image>();
        fadeImage.color = fadeColor;

        // 🎯 Raycast’i kapat ki butonlara tıklamayı engellemesin
        fadeImage.raycastTarget = false;

        RectTransform rt = panelGO.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }


    private IEnumerator FadeInScreen()
    {
        float timer = 0f;
        Color c = fadeImage.color;
        while (timer < fadeDuration)
        {
            if (skipScene) break;
            timer += Time.unscaledDeltaTime;
            c.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }
        c.a = 0f;
        fadeImage.color = c;

        StartCoroutine(RunSequence());
    }

    private IEnumerator RunSequence()
    {
        int textIndex = 0;
        int imageIndex = 0;

        while ((textIndex < texts.Length || imageIndex < images.Count) && !skipScene)
        {
            if (textIndex < texts.Length)
            {
                yield return StartCoroutine(ShowTextWithAudioAndFade(texts[textIndex]));
                textIndex++;
            }

            if (imageIndex < images.Count)
            {
                yield return StartCoroutine(ShowImage(images[imageIndex]));
                yield return new WaitForSeconds(defaultDisplayDuration);
                yield return StartCoroutine(HideImage(images[imageIndex]));
                imageIndex++;
            }
        }

        if (!skipScene)
        {
            yield return new WaitForSeconds(delayBeforeNextScene);
        }

        yield return StartCoroutine(FadeOutScreen());
        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator ShowTextWithAudioAndFade(TextEntry entry)
    {
        entry.textComponent.color = new Color(entry.textComponent.color.r,
                                              entry.textComponent.color.g,
                                              entry.textComponent.color.b, 1f);

        entry.textComponent.text = "";

        for (int i = 0; i < entry.fullText.Length; i++)
        {
            if (skipScene)
            {
                entry.textComponent.text = entry.fullText;
                break;
            }

            entry.textComponent.text += entry.fullText[i];

            if (audioSource != null && typeSound != null)
            {
                audioSource.pitch = 1f + Random.Range(-pitchRandomness, pitchRandomness);
                audioSource.PlayOneShot(typeSound);
            }

            float timer = 0f;
            while (timer < entry.typeDelay)
            {
                if (skipScene)
                {
                    entry.textComponent.text = entry.fullText;
                    break;
                }
                timer += Time.deltaTime;
                yield return null;
            }
        }

        float fadeTimer = 0f;
        Color original = entry.textComponent.color;
        while (fadeTimer < entry.fadeDuration && !skipScene)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, fadeTimer / entry.fadeDuration);
            entry.textComponent.color = new Color(original.r, original.g, original.b, alpha);
            yield return null;
        }
        entry.textComponent.color = new Color(original.r, original.g, original.b, 0f);
    }

    private IEnumerator ShowImage(ImageEntry entry)
    {
        float timer = 0f;
        Color original = entry.imageComponent.color;
        while (timer < entry.fadeDuration && !skipScene)
        {
            timer += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / entry.fadeDuration);
            entry.imageComponent.color = new Color(original.r, original.g, original.b, alpha);
            yield return null;
        }
        entry.imageComponent.color = new Color(original.r, original.g, original.b, 1f);
    }

    private IEnumerator HideImage(ImageEntry entry)
    {
        float timer = 0f;
        Color original = entry.imageComponent.color;
        while (timer < entry.fadeDuration && !skipScene)
        {
            timer += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / entry.fadeDuration);
            entry.imageComponent.color = new Color(original.r, original.g, original.b, alpha);
            yield return null;
        }
        entry.imageComponent.color = new Color(original.r, original.g, original.b, 0f);
    }

    private IEnumerator FadeOutScreen()
    {
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
    }

    // --------------------------
    // Button tıklaması ile sahne geçirme
    // --------------------------
    public void SkipSceneButton()
    {
        skipScene = true;
        SceneManager.LoadScene(nextSceneName);
    }
}
