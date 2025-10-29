using UnityEngine;
using TMPro;

public class PlanetUI : MonoBehaviour
{
    [Header("References")]
    public PlanetPlacer placer;
    public GameManager gameManager; // Inspector’dan sahnedeki GameManager objesini ata

    [Header("UI Groups (Parent GameObjects)")]
    public GameObject smallGroup;
    public GameObject mediumGroup;
    public GameObject largeGroup;

    [Header("Text Components")]
    public TMP_Text smallText;
    public TMP_Text mediumText;
    public TMP_Text largeText;

    [Header("Simulating Text")]
    public GameObject simulatingText;

    void OnEnable()
    {
        if (gameManager != null)
            gameManager.OnStateChanged += HandleStateChanged;
    }

    void OnDisable()
    {
        if (gameManager != null)
            gameManager.OnStateChanged -= HandleStateChanged;
    }

    void Update()
    {
        if (placer == null || gameManager == null) return;

        // Placement modunda kalan hakları güncelle
        if (gameManager.State == GameState.Placement)
        {
            if (smallText != null) smallText.text = placer.GetRemainingSmall() + "x";
            if (mediumText != null) mediumText.text = placer.GetRemainingMedium() + "x";
            if (largeText != null) largeText.text = placer.GetRemainingLarge() + "x";
        }
    }

    private void HandleStateChanged(GameState state)
    {
        bool showUI = (state == GameState.Placement);
        bool showSimulating = (state == GameState.Running);

        if (smallGroup != null) smallGroup.SetActive(showUI);
        if (mediumGroup != null) mediumGroup.SetActive(showUI);
        if (largeGroup != null) largeGroup.SetActive(showUI);

        if (simulatingText != null) simulatingText.SetActive(showSimulating);
    }
}
