using UnityEngine;

public class GameDataReset : MonoBehaviour
{
    public void ResetGameData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs sýfýrlandý!");
    }
}