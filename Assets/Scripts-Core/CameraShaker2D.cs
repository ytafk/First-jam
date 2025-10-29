using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class CameraShake2D : MonoBehaviour
{
    [SerializeField] float defaultAmplitude = 4f;
    [SerializeField] float defaultDuration = 1f;

    Vector3 originalLocalPos;
    Coroutine runningShake;

    // Awake'te alma! O anki konum deðiþebilir.
    void Awake() { /* boþ býrak */ }

    public void Shake(float amplitude, float duration)
    {
        // Yeni shake baþlamadan, o anda kameranýn local merkezini referans al
        originalLocalPos = transform.localPosition;

        if (runningShake != null)
            StopCoroutine(runningShake);

        runningShake = StartCoroutine(DoShake(amplitude, duration));
    }

    public void Shake() => Shake(defaultAmplitude, defaultDuration);

    IEnumerator DoShake(float amplitude, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            float damper = 1f - (t / duration);
            //damper *= damper; // ease-out

            Vector2 offs = Random.insideUnitCircle * amplitude /** damper*/;
            transform.localPosition = originalLocalPos + new Vector3(offs.x, offs.y, 0f);

            t += Time.deltaTime;
            yield return null;
        }

        // Bitince o anda yakaladýðýmýz merkeze tam geri dön
        transform.localPosition = originalLocalPos;
        runningShake = null;
    }

    void OnDisable()
    {
        // Her ihtimale karþý stabilize et
        transform.localPosition = originalLocalPos;
        runningShake = null;
    }
}


