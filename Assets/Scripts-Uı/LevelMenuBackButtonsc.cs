using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenuBackButtonsc : MonoBehaviour
{
    [Header("Ge�ilecek Sahne Ad�")]
    public string sceneName;

    private Button btn;

    void Start()
    {
        // Button komponentini al
        btn = GetComponent<Button>();
        if (btn != null)
        {
            // Butona t�klan�nca LoadScene fonksiyonunu �a��r
            btn.onClick.AddListener(LoadScene);
        }
        else
        {
            Debug.LogWarning("Button komponenti bulunamad�!");
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
            Debug.LogWarning("Sahne ad� bo�!");
        }
    }

}