using UnityEngine;

public class SceneSkipButton : MonoBehaviour
{
    public Typescriptmanagersc manager; // Inspector�dan ba�lanacak

    public void SkipScene()
    {
        if (manager != null)
        {
            manager.SkipSceneButton();
        }
    }
}
