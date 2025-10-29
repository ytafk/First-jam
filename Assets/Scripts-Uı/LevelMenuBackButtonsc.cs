using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenuBackButtonsc : MonoBehaviour
{
    [Header("Geçilecek Sahne Adý")]
    public string sceneName;

    private Button btn;

    void Start()
    {
        // Button komponentini al
        btn = GetComponent<Button>();
        if (btn != null)
        {
            // Butona týklanýnca LoadScene fonksiyonunu çaðýr
            btn.onClick.AddListener(LoadScene);
        }
        else
        {
            Debug.LogWarning("Button komponenti bulunamadý!");
        }
    }

    void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Sahne adý boþ!");
        }
    }

}