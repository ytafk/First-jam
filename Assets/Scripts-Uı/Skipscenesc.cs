using UnityEngine;

public class SceneSkipButton : MonoBehaviour
{
    public Typescriptmanagersc manager; // Inspector’dan baðlanacak

    public void SkipScene()
    {
        if (manager != null)
        {
            manager.SkipSceneButton();
        }
    }
}
