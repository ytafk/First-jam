using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuQuitButtonsc : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    void Start()
    {
        if (animator != null)
            animator.enabled = false;  // Baþlangýçta animasyonu kapat
        if (image != null && defaultSprite != null)
            image.sprite = defaultSprite;
    }

    [Header("Animasyon Ayarlarý")]
    public Animator animator;          // Animator referansý
    public Image image;                // UI Image component
    public Sprite defaultSprite;       // Normal sprite
    public string hoverAnimationName;  // Oynatýlacak animasyonun adý

    // Quit Buton Fonksiyonu
    public void QuitGame()
    {
        Debug.Log("Oyun kapatýlýyor...");
        Application.Quit();
        // Not: Application.Quit() sadece build alýnca çalýþýr.
    }

    // Mouse üzerine geldiðinde animasyon baþlasýn
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (animator != null)
        {
            animator.enabled = true;
            animator.Play(hoverAnimationName, -1, 0f);
        }
    }

    // Mouse ayrýldýðýnda animasyonu kapat ve normal sprite’a dön
    public void OnPointerExit(PointerEventData eventData)
    {
        if (animator != null) animator.enabled = false;
        if (image != null && defaultSprite != null) image.sprite = defaultSprite;
    }
}
