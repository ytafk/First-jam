using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class PlanetPlacer : MonoBehaviour
{
    [Header("Placement")]
    public GameObject planetPrefab;
    public List<PlanetProfile> profiles; // 0 = small, 1 = medium, 2 = large

    [Header("Level Limits")]
    public LevelConfig levelConfig; // 👈 buraya LevelConfig asset'ini atayacaksın

    private Camera cam;
    private int currentSizeIndex = 0; // 0/1/2 = small/medium/large
    private List<GameObject> placed = new List<GameObject>();
    private GameObject previewGO;

    // Sayaçlar (level bazlı limit takibi)
    private int usedSmall = 0;
    private int usedMedium = 0;
    private int usedLarge = 0;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.State != GameState.Placement)
        {
            if (previewGO != null) Destroy(previewGO);
            return;
        }

        HandleInput();
        UpdatePreview();
    }

    void HandleInput()
    {
        // Boyut seçimi (UI'dan da çağırmak istersen public SetSizeIndex ekledim)
        if (Input.GetKeyDown(KeyCode.Alpha1)) currentSizeIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) currentSizeIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) currentSizeIndex = 2;

        // Sol tık → yerleştir
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = GetMouseWorldPosition();
            PlacePlanet(worldPos);
        }

        // Sağ tık → en yakın gezegeni sil (yakınlık eşik: 0.5)
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 worldPos = GetMouseWorldPosition();
            RemoveClosestPlanet(worldPos);
        }

        // Space → oyunu başlat
        if (Input.GetKeyDown(KeyCode.Space))
            GameManager.Instance.StartGame();
    }

    void UpdatePreview()
    {
        Vector3 worldPos = GetMouseWorldPosition();
        PlanetProfile profile = profiles[Mathf.Clamp(currentSizeIndex, 0, profiles.Count - 1)];

        if (previewGO == null)
        {
            previewGO = Instantiate(planetPrefab, worldPos, Quaternion.identity, transform);
            var sr = previewGO.GetComponentInChildren<SpriteRenderer>();
            if (sr != null) sr.color = new Color(1f, 1f, 1f, 0.3f); // şeffaf önizleme
        }

        // önizleme pozisyonu + profil uygula
        previewGO.transform.position = worldPos;

        var p = previewGO.GetComponent<Planet>();
        if (p != null)
        {
            p.profile = profile;
            p.Apply();
        }

        // (Opsiyonel) limit dolu ise önizleme KIRMIZI olsun
        var rend = previewGO.GetComponentInChildren<SpriteRenderer>();
        if (rend != null)
        {
            bool limitReached = IsLimitReached(currentSizeIndex);
            Color baseCol = new Color(1f, 1f, 1f, 0.3f);
            rend.color = limitReached ? new Color(1f, 0.3f, 0.3f, 0.5f) : baseCol;
        }
    }

    void PlacePlanet(Vector3 worldPos)
    {
        if (planetPrefab == null || profiles.Count == 0) return;

        // Limit kontrolü: LevelConfig yoksa limitsiz kabul ederiz
        if (levelConfig != null && IsLimitReached(currentSizeIndex))
        {
            // İstersen burada bir uyarı SFX/UI gösterebilirsin
            // Debug.Log("Limit reached for size " + currentSizeIndex);
            return;
        }

        PlanetProfile profile = profiles[Mathf.Clamp(currentSizeIndex, 0, profiles.Count - 1)];
        GameObject go = Instantiate(planetPrefab, worldPos, Quaternion.identity, transform);

        var p = go.GetComponent<Planet>();
        if (p != null)
        {
            p.profile = profile;
            p.Apply();
        }

        placed.Add(go);

        // Sayaç artır
        if (levelConfig != null)
        {
            if (currentSizeIndex == 0) usedSmall++;
            else if (currentSizeIndex == 1) usedMedium++;
            else if (currentSizeIndex == 2) usedLarge++;
        }
    }

    void RemoveClosestPlanet(Vector3 worldPos)
    {
        float minDist = float.MaxValue;
        GameObject closest = null;

        foreach (var go in placed)
        {
            if (go == null) continue;
            float dist = Vector2.Distance(worldPos, go.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = go;
            }
        }

        if (closest != null && minDist < 0.5f)
        {
            // Sayaç azalt (profil referansına göre)
            if (levelConfig != null)
            {
                var p = closest.GetComponent<Planet>();
                if (p != null && p.profile != null)
                {
                    if (profiles.Count > 0 && p.profile == profiles[0]) usedSmall = Mathf.Max(0, usedSmall - 1);
                    else if (profiles.Count > 1 && p.profile == profiles[1]) usedMedium = Mathf.Max(0, usedMedium - 1);
                    else if (profiles.Count > 2 && p.profile == profiles[2]) usedLarge = Mathf.Max(0, usedLarge - 1);
                }
            }

            placed.Remove(closest);
            Destroy(closest);
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -cam.transform.position.z; // kamera ortho ise z=-10 olduğundan +10 yapılır
        return cam.ScreenToWorldPoint(mousePos);
    }

    public void ResetPlacement()
    {
        foreach (var go in placed)
        {
            if (go != null) Destroy(go);
        }
        placed.Clear();

        if (previewGO != null)
        {
            Destroy(previewGO);
            previewGO = null;
        }

        // Sayaçları sıfırla
        usedSmall = usedMedium = usedLarge = 0;
    }

    // UI butonları burayı çağırabilir
    public void SetSizeIndex(int index)
    {
        currentSizeIndex = Mathf.Clamp(index, 0, Mathf.Max(0, profiles.Count - 1));
    }

    // Kalan hakları UI'da göstermek için:
    public int GetRemainingSmall() => (levelConfig == null) ? int.MaxValue : Mathf.Max(0, levelConfig.smallLimit - usedSmall);
    public int GetRemainingMedium() => (levelConfig == null) ? int.MaxValue : Mathf.Max(0, levelConfig.mediumLimit - usedMedium);
    public int GetRemainingLarge() => (levelConfig == null) ? int.MaxValue : Mathf.Max(0, levelConfig.largeLimit - usedLarge);

    private bool IsLimitReached(int sizeIndex)
    {
        if (levelConfig == null) return false; // limitsiz mod

        switch (sizeIndex)
        {
            case 0: return usedSmall >= levelConfig.smallLimit;
            case 1: return usedMedium >= levelConfig.mediumLimit;
            case 2: return usedLarge >= levelConfig.largeLimit;
            default: return true;
        }
    }
}











//public class PlanetPlacer : MonoBehaviour
//{
//    public GameObject planetPrefab;
//    public List<PlanetProfile> profiles; // 0=small,1=medium,2=large

//    private Camera cam;
//    private int currentSizeIndex = 0;
//    private Dictionary<Vector2Int, GameObject> placed = new Dictionary<Vector2Int, GameObject>();

//    private GameObject previewGO;

//    void Start()
//    {
//        cam = Camera.main;
//    }

//    void Update()
//    {
//        if (GameManager.Instance == null) return;

//        if (GameManager.Instance.State != GameState.Placement)
//        {
//            if (previewGO != null) Destroy(previewGO);
//            return;
//        }

//        HandleInput();
//        UpdatePreview();
//    }

//    void HandleInput()
//    {
//        if (Input.GetKeyDown(KeyCode.Alpha1)) currentSizeIndex = 0;
//        if (Input.GetKeyDown(KeyCode.Alpha2)) currentSizeIndex = 1;
//        if (Input.GetKeyDown(KeyCode.Alpha3)) currentSizeIndex = 2;

//        if (Input.GetMouseButtonDown(0))
//        {
//            Vector2Int gridPos = WorldToWorld(Input.mousePosition);
//            if (!placed.ContainsKey(gridPos))
//                PlacePlanet(gridPos);
//        }

//        if (Input.GetMouseButtonDown(1))
//        {
//            Vector2Int gridPos = WorldToWorld(Input.mousePosition);
//            if (placed.ContainsKey(gridPos))
//                RemovePlanet(gridPos);
//        }

//        if (Input.GetKeyDown(KeyCode.Space))
//            GameManager.Instance.StartGame();
//    }

//    void UpdatePreview()
//    {
//        Vector2Int gridPos = WorldToWorld(Input.mousePosition);
//        var profile = profiles[Mathf.Clamp(currentSizeIndex, 0, profiles.Count - 1)];

//        if (previewGO == null)
//        {
//            previewGO = Instantiate(planetPrefab, new Vector3(gridPos.x, gridPos.y, 0), Quaternion.identity, transform);
//            // şeffaf görünüm
//            var sr = previewGO.GetComponentInChildren<SpriteRenderer>();
//            if (sr != null) sr.color = new Color(1f, 1f, 1f, 0.3f);
//        }

//        previewGO.transform.position = new Vector3(gridPos.x, gridPos.y, 0);

//        var p = previewGO.GetComponent<Planet>();
//        if (p != null)
//        {
//            p.profile = profile;
//            p.Apply(); // profil değerleri + ring renkleri uygulanır
//        }
//    }

//    void PlacePlanet(Vector2Int gridPos)
//    {
//        if (planetPrefab == null || profiles.Count == 0) return;

//        PlanetProfile profile = profiles[Mathf.Clamp(currentSizeIndex, 0, profiles.Count - 1)];
//        GameObject go = Instantiate(planetPrefab, new Vector3(gridPos.x, gridPos.y, 0), Quaternion.identity, transform);

//        var p = go.GetComponent<Planet>();
//        if (p != null)
//        {
//            p.profile = profile;
//            p.Apply();
//        }

//        placed.Add(gridPos, go);
//    }

//    void RemovePlanet(Vector2Int gridPos)
//    {
//        if (placed.TryGetValue(gridPos, out GameObject go))
//        {
//            Destroy(go);
//            placed.Remove(gridPos);
//        }
//    }

//    Vector3 WorldToWorld(Vector3 mousePos)
//    {
//        return cam.ScreenToWorldPoint(mousePos);
//    }


//    public void ResetPlacement()
//    {
//        foreach (var kv in placed)
//            Destroy(kv.Value);
//        placed.Clear();

//        if (previewGO != null)
//        {
//            Destroy(previewGO);
//            previewGO = null;
//        }
//    }
//}






//using System.Collections.Generic;
//using UnityEngine;

//public class PlanetPlacer : MonoBehaviour
//{
//    [Header("Prefabs & Profiles")]
//    public Planet planetPrefab;
//    public PlanetProfile smallProfile, mediumProfile, largeProfile;

//    [Header("Level Budget (adet)")]
//    public int smallCount = 3, mediumCount = 2, largeCount = 1;

//    [Header("Yerleştirme Kuralları")]
//    public LayerMask blockerMask;       // Asteroid/duvar/goal gibi “yasak” alanlar
//    public Transform rocketStart;       // Roketin başlangıç noktası
//    public float spawnSafeRadius = 1.5f;// Rokete çok yakın koymasın
//    public float planetMinCenterGap = 0.3f; // Planet merkezleri arası min boşluk

//    [Header("Ghost Görseli (opsiyonel)")]
//    public Sprite ghostPlanetSprite;

//    List<Planet> placed = new();
//    int sel = 0; // 0: small, 1: medium, 2: large
//    int spin = +1;
//    Camera cam;

//    GameObject ghost;
//    PlanetRings ghostRings;
//    SpriteRenderer ghostSprite;

//    void Start()
//    {
//        cam = Camera.main;
//        CreateGhost();
//        UpdateGhostProfile(CurrentProfile());

//        if (GameManager.Instance != null)
//        {
//            GameManager.Instance.OnStateChanged += OnStateChanged;
//            SetActive(GameManager.Instance.State == GameState.Placement);
//        }
//        else SetActive(true); // editor test
//    }

//    void OnDestroy()
//    {
//        if (GameManager.Instance != null)
//            GameManager.Instance.OnStateChanged -= OnStateChanged;
//    }

//    void OnStateChanged(GameState s) => SetActive(s == GameState.Placement);

//    void SetActive(bool active)
//    {
//        if (ghost != null) ghost.SetActive(active);
//        enabled = active;
//    }

//    PlanetProfile CurrentProfile()
//    {
//        return sel == 0 ? smallProfile : sel == 1 ? mediumProfile : largeProfile;
//    }

//    void CreateGhost()
//    {
//        ghost = new GameObject("PlanetGhost");
//        ghostRings = ghost.AddComponent<PlanetRings>();
//        ghostSprite = ghost.AddComponent<SpriteRenderer>();
//        ghostSprite.sprite = ghostPlanetSprite; // olmasa da olur; halkalar görünür
//        ghostSprite.color = new Color(1, 1, 1, 0.30f);
//        ghostSprite.sortingOrder = -2;
//    }

//    void UpdateGhostProfile(PlanetProfile p)
//    {
//        if (p == null) return;
//        ghostRings.Set(p.R1, p.R2, p.R3, p.ringWidth);
//        // Ghost’u biraz daha silik yap
//        Color fade(Color c) => new Color(c.r, c.g, c.b, c.a * 0.85f);
//        ghostRings.SetColors(fade(p.ringColorR1), fade(p.ringColorR2), fade(p.ringColorR3));
//    }

//    void Update()
//    {
//        var p = CurrentProfile();
//        if (p == null) return;

//        Vector3 m = cam.ScreenToWorldPoint(Input.mousePosition); m.z = 0f;
//        ghost.transform.position = m;

//        // Boyut seçimi
//        if (Input.mouseScrollDelta.y > 0f) CycleSelection(+1);
//        else if (Input.mouseScrollDelta.y < 0f) CycleSelection(-1);
//        if (Input.GetKeyDown(KeyCode.Alpha1)) { sel = 0; spin = +1; UpdateGhostProfile(CurrentProfile()); }
//        if (Input.GetKeyDown(KeyCode.Alpha2)) { sel = 1; spin = +1; UpdateGhostProfile(CurrentProfile()); }
//        if (Input.GetKeyDown(KeyCode.Alpha3)) { sel = 2; spin = +1; UpdateGhostProfile(CurrentProfile()); }

//        // Spin çevir
//        if (Input.GetKeyDown(KeyCode.R)) spin = -spin;

//        bool canPlace = CanPlaceAt(m, p);
//        SetGhostValid(canPlace);

//        if (Input.GetMouseButtonDown(0) && canPlace)
//            PlacePlanet(m, p, spin);

//        if (Input.GetMouseButtonDown(1))
//            TryRemoveNearest(m);

//        // Başlat
//        if (Input.GetKeyDown(KeyCode.Space))
//            CommitAndStart();
//    }

//    void CycleSelection(int d)
//    {
//        sel = (sel + d + 3) % 3;
//        spin = +1;
//        UpdateGhostProfile(CurrentProfile());
//    }

//    void SetGhostValid(bool ok)
//    {
//        float a = ok ? 0.35f : 0.12f;
//        var c = ghostSprite.color; c.a = a; ghostSprite.color = c;
//    }

//    int Remaining(PlanetProfile p)
//    {
//        if (p == smallProfile) return smallCount;
//        if (p == mediumProfile) return mediumCount;
//        return largeCount;
//    }
//    void Decrement(PlanetProfile p)
//    {
//        if (p == smallProfile) smallCount--;
//        else if (p == mediumProfile) mediumCount--;
//        else largeCount--;
//    }
//    void Increment(PlanetProfile p)
//    {
//        if (p == smallProfile) smallCount++;
//        else if (p == mediumProfile) mediumCount++;
//        else largeCount++;
//    }

//    bool CanPlaceAt(Vector2 pos, PlanetProfile p)
//    {
//        if (Remaining(p) <= 0) return false;

//        // Yasak alanlar: asteroid/duvar/goal vs.
//        if (Physics2D.OverlapCircle(pos, p.R1, blockerMask) != null) return false;

//        // Roket başlangıcına çok yakın olmasın
//        if (rocketStart && Vector2.Distance(pos, rocketStart.position) < spawnSafeRadius + p.R1)
//            return false;

//        // Diğer gezegenlerden yeterli uzaklık
//        foreach (var other in placed)
//        {
//            if (!other) continue;
//            float min = p.R1 + other.profile.R1 + planetMinCenterGap;
//            if (Vector2.Distance(pos, other.transform.position) < min) return false;
//        }
//        return true;
//    }

//    void PlacePlanet(Vector2 pos, PlanetProfile p, int spinOverride)
//    {
//        var go = Instantiate(planetPrefab, pos, Quaternion.identity);
//        go.profile = p;
//        go.attractor.spin = spinOverride;
//        go.Apply();               // profil + halkaları uygula
//        placed.Add(go);
//        Decrement(p);
//    }

//    void TryRemoveNearest(Vector2 pos)
//    {
//        Planet best = null;
//        float bestDist = 0.6f; // imlece yakınsa sil
//        foreach (var pl in placed)
//        {
//            if (!pl) continue;
//            float d = Vector2.Distance(pos, pl.transform.position);
//            if (d < bestDist) { best = pl; bestDist = d; }
//        }
//        if (best)
//        {
//            Increment(best.profile);
//            placed.Remove(best);
//            Destroy(best.gameObject);
//        }
//    }

//    void CommitAndStart()
//    {
//        // İstersen tüm haklar kullanılmadan başlamayı engelle:
//        // if (smallCount>0 || mediumCount>0 || largeCount>0) return;

//        GameManager.Instance.StartGame();
//        SetActive(false);
//    }

//    // Basit bir HUD (UI kurmadan)
//    void OnGUI()
//    {
//        GUI.Label(new Rect(10, 10, 820, 24),
//          $"[1]Küçük:{smallCount}   [2]Orta:{mediumCount}   [3]Büyük:{largeCount}   [R]Spin   Sol Tık=Koy  Sağ Tık=Sil  [Space]=Başlat");
//    }
//}
