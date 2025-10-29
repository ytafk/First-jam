using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuQuitButtonsc : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    void Start()
    {
        if (animator != null)
            animator.enabled = false;  // Ba�lang��ta animasyonu kapat
        if (image != null && defaultSprite != null)
            image.sprite = defaultSprite;
    }

    [Header("Animasyon Ayarlar�")]
    public Animator animator;          // Animator referans�
    public Image image;                // UI Image component
    public Sprite defaultSprite;       // Normal sprite
    public string hoverAnimationName;  // Oynat�lacak animasyonun ad�

    // Quit Buton Fonksiyonu
    public void QuitGame()
    {
        Debug.Log("Oyun kapat�l�yor...");
        Application.Quit();
        // Not: Application.Quit() sadece build al�nca �al���r.
    }

    // Mouse �zerine geldi�inde animasyon ba�las�n
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (animator != null)
        {
            animator.enabled = true;
            animator.Play(hoverAnimationName, -1, 0f);
        }
    }

    // Mouse ayr�ld���nda animasyonu kapat ve normal sprite�a d�n
    public void OnPointerExit(PointerEventData eventData)
    {
        if (animator != null) animator.enabled = false;
        if (image != null && defaultSprite != null) image.sprite = defaultSprite;
    }
}
