using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Menuplaybuttonsc : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Ge�ilecek Sahne Ad�")]
    public string sceneName;

    [Header("Animasyon Ayarlar�")]
    public Animator animator;          // Animator referans�
    public Image image;                // Dura�an sprite i�in UI Image
    public Sprite defaultSprite;       // Normal sprite
    public string hoverAnimationName;  // Oynat�lacak animasyonun ad�

    private Button btn;

    void Start()
    {
        // Button komponentini al
        btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(LoadScene);
        }
        else
        {
            Debug.LogWarning("Button komponenti bulunamad�!");
        }

        // Ba�lang��ta animasyonu kapat�p default sprite g�ster
        if (animator != null) animator.enabled = false;
        if (image != null && defaultSprite != null) image.sprite = defaultSprite;
    }

    void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Sahne ad� bo�!");
        }
    }

    // Mouse �zerine geldi�inde
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (animator != null)
        {
            animator.enabled = true;
            animator.Play(hoverAnimationName, -1, 0f);
        }
    }

    // Mouse ayr�ld���nda
    public void OnPointerExit(PointerEventData eventData)
    {
        if (animator != null) animator.enabled = false;
        if (image != null && defaultSprite != null) image.sprite = defaultSprite;
    }
}
