using UnityEngine;
using UnityEngine.UI;

public class Volumeslidersc : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
        if (slider != null && MusicManager.instance != null)
        {
            // Scrollbar ba�lang�� de�erini g�ncel volume�ye e�itle
            slider.value = MusicManager.instance.audioSource.volume;
            // De�i�im oldu�unda m�zik sesini g�ncelle
            slider.onValueChanged.AddListener(OnScrollValueChanged);
        }
    }

    private void OnScrollValueChanged(float value)
    {
        if (MusicManager.instance != null)
        {
            // Scrollbar 0�1 aras� �al���yor, sen 0�100 istiyorsan burada �arpabilirsin
            MusicManager.instance.SetVolume(value);
        }
    }
}
