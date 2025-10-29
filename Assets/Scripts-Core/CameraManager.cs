using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [Header("General")]
    public Camera cam;
    public float moveSpeed = 5f;        // placement modunda kayma hızı

    [Header("Placement Settings")]
    public float edgeSize = 50f;        // ekran kenarından kaç px içinde kaymaya başlasın
    public float xMin = -20f;           // sol sınır
    public float xMax = 20f;            // sağ sınır

    [Header("Running Settings")]
    public Transform rocket;            // roket referansı
    private float fixedY;               // Y sabitlenecek
    private float fixedZ;               // Z sabitlenecek

    // Zıplamayı engellemek için
    private float lastKnownRocketX = 0f;
    private bool hasEverRun = false;    // en az bir kez RUNNING görüldü mü?

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (cam == null) cam = Camera.main;
    }

    private void Start()
    {
        fixedY = cam.transform.position.y;
        fixedZ = cam.transform.position.z;

        if (GameManager.Instance != null)
            GameManager.Instance.OnStateChanged += OnStateChanged;
    }

    private void Update()
    {
        if (GameManager.Instance == null) return;

        var state = GameManager.Instance.State;

        // Roket hayattayken son X'i sürekli güncelle
        if (rocket != null)
            lastKnownRocketX = rocket.position.x;

        if (state == GameState.Running)
        {
            hasEverRun = true;   // bir kez bile running gördüysek placement'ı bir daha kullanmayacağız
            Follow(rocket != null ? rocket.position.x : lastKnownRocketX);
        }
        else
        {
            // Oyun hiç başlamadıysa: klasik placement kaydırması
            if (!hasEverRun)
            {
                HandlePlacementCamera();
            }
            else
            {
                // Oyun en az bir kez başladıysa: placement'a ASLA dönme
                // Roket yoksa bile son X'te sabit kal
                Follow(lastKnownRocketX);
            }
        }
    }

    private void Follow(float targetX)
    {
        var pos = cam.transform.position;
        pos.x = targetX;   // sadece X ekseni takip/sabit
        pos.y = fixedY;    // Y sabit
        pos.z = fixedZ;    // Z sabit
        cam.transform.position = pos;
    }

    private void HandlePlacementCamera()
    {
        Vector3 pos = cam.transform.position;
        Vector3 mousePos = Input.mousePosition;

        if (mousePos.x <= edgeSize)
            pos.x -= moveSpeed * Time.deltaTime;

        if (mousePos.x >= Screen.width - edgeSize)
            pos.x += moveSpeed * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, xMin, xMax);

        cam.transform.position = pos;
    }

    private void OnStateChanged(GameState state)
    {
        // RUNNING'e girerken bir kere hizala
        if (state == GameState.Running)
        {
            float targetX = (rocket != null) ? rocket.position.x : lastKnownRocketX;
            Follow(targetX);
        }
    }
}


//public class CameraManager : MonoBehaviour
//{
//    public static CameraManager Instance;

//    [Header("General")]
//    public Camera cam;
//    public float moveSpeed = 5f;        // placement modunda kayma hızı

//    [Header("Placement Settings")]
//    public float edgeSize = 50f;        // ekran kenarından kaç px içinde kaymaya başlasın
//    public float xMin = -20f;           // sol sınır
//    public float xMax = 20f;           // sağ sınır

//    [Header("Running Settings")]
//    public Transform rocket;            // roket referansı
//    private float fixedY;               // Y sabitlenecek
//    private float fixedZ;               // Z sabitlenecek
//    private float lastKnownRocketX = 0f;
//    // Ölüm sonrası kamera kilidi
//    private bool freezeOnDeath = false;
//    public float frozenX = 0f;
//    // geçişleri yakalamak için
//    private GameState prevState;
//    private bool prevRocketExists;
//    private bool hasEverRun = false;

//    private void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);

//        if (cam == null) cam = Camera.main;
//    }

//    private void Start()
//    {
//        fixedY = cam.transform.position.y;
//        fixedZ = cam.transform.position.z;

//        // GameManager state değişimlerine abone ol
//        if (GameManager.Instance != null)
//            GameManager.Instance.OnStateChanged += OnStateChanged;
//        prevState = (GameManager.Instance != null) ? GameManager.Instance.State : GameState.Placement;
//        prevRocketExists = (rocket != null);
//    }

//    private void Update()
//    {
//        if (GameManager.Instance == null) return;

//        var state = GameManager.Instance.State;

//        if (state == GameState.Running)
//        {
//            HandleRunningOrFrozenCamera(runMode: true);
//            hasEverRun = true; // artık placement'a dönmeyeceğiz
//        }
//        else
//        {
//            // Hiç RUNNING görmediysek placement; gördüysek SON X'te kal
//            if (!hasEverRun)
//                HandlePlacementCamera();
//            else
//                HandleRunningOrFrozenCamera(runMode: false); // son X'e sabit
//        }
//    }




//    private void HandlePlacementCamera()
//    {
//        Vector3 pos = cam.transform.position;
//        Vector3 mousePos = Input.mousePosition;

//        // sola kay
//        if (mousePos.x <= edgeSize)
//            pos.x -= moveSpeed * Time.deltaTime;

//        // sağa kay
//        if (mousePos.x >= Screen.width - edgeSize)
//            pos.x += moveSpeed * Time.deltaTime;

//        // sınırla
//        pos.x = Mathf.Clamp(pos.x, xMin, xMax);

//        cam.transform.position = pos;
//    }

//    private void HandleRunningOrFrozenCamera(bool runMode)
//    {
//        // Roket hayattaysa X'i güncelle
//        if (rocket != null)
//            lastKnownRocketX = rocket.position.x;

//        float targetX = (rocket != null) ? rocket.position.x : lastKnownRocketX;

//        Vector3 pos = cam.transform.position;
//        pos.x = targetX;
//        pos.y = fixedY;
//        pos.z = fixedZ;

//        cam.transform.position = pos;
//    }



//    private void OnStateChanged(GameState state)
//    {
//        if (state == GameState.Running)
//        {
//            Vector3 pos = cam.transform.position;
//            float targetX = (rocket != null) ? rocket.position.x : lastKnownRocketX;
//            pos.x = targetX; pos.y = fixedY; pos.z = fixedZ;
//            cam.transform.position = pos;
//        }
//    }





//}


//using UnityEngine;

//public class CameraManager : MonoBehaviour
//{
//    public static CameraManager Instance;

//    [Header("General")]
//    public Camera cam;
//    public float moveSpeed = 5f;        // placement modunda kayma hızı

//    [Header("Placement Settings")]
//    public float edgeSize = 50f;        // ekran kenarından kaç px içinde kaymaya başlasın
//    public float xMin = -20f;           // sol sınır
//    public float xMax = 20f;           // sağ sınır

//    [Header("Running Settings")]
//    public Transform rocket;            // roket referansı
//    private float fixedY;               // Y sabitlenecek
//    private float fixedZ;               // Z sabitlenecek

//    [Header("Shake")]
//    [SerializeField] private float defaultShakeAmp = 0.5f;
//    [SerializeField] private float defaultShakeDuration = 0.3f;
//    private float shakeTimer = 0f;
//    private float shakeDuration = 0f;
//    private float shakeAmp = 0f;
//    private Vector3 shakeOffset = Vector3.zero;

//    // Takip yedeği ve kilit
//    private float lastKnownRocketX = 0f;
//    private bool followFrozen = false;
//    private float frozenX = 0f;


//    private void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);

//        if (cam == null) cam = Camera.main;
//    }

//    private void Start()
//    {
//        fixedY = cam.transform.position.y;
//        fixedZ = cam.transform.position.z;

//        // GameManager state değişimlerine abone ol
//        if (GameManager.Instance != null)
//            GameManager.Instance.OnStateChanged += OnStateChanged;
//    }

//    private void Update()
//    {
//        if (GameManager.Instance == null) return;

//        if (GameManager.Instance.State == GameState.Running && shakeTimer > 0f)
//        {
//            shakeTimer -= Time.deltaTime;
//            float t = 1f - (shakeTimer / shakeDuration); // 0→1
//            float damper = (1f - t); damper *= damper;   // ease-out

//            Vector2 rnd = Random.insideUnitCircle * shakeAmp * damper;
//            shakeOffset = new Vector3(rnd.x, rnd.y, 0f);
//        }
//        else
//        {
//            shakeOffset = Vector3.zero;
//        }
//        if (GameManager.Instance.State == GameState.Placement)
//            HandlePlacementCamera();
//        else if (GameManager.Instance.State == GameState.Running && rocket != null)
//            HandleRunningCamera();
//    }

//    private void HandlePlacementCamera()
//    {
//        Vector3 pos = cam.transform.position;
//        Vector3 mousePos = Input.mousePosition;

//        // sola kay
//        if (mousePos.x <= edgeSize)
//            pos.x -= moveSpeed * Time.deltaTime;

//        // sağa kay
//        if (mousePos.x >= Screen.width - edgeSize)
//            pos.x += moveSpeed * Time.deltaTime;

//        // sınırla
//        pos.x = Mathf.Clamp(pos.x, xMin, xMax);

//        cam.transform.position = pos;
//    }

//    private void HandleRunningCamera()
//    {
//        // Roket varsa son X'i güncelle
//        if (rocket != null)
//            lastKnownRocketX = rocket.position.x;

//        // Hedef X: kilitliyse frozenX, değilse roket varsa roketX, yoksa son bilinen X
//        float targetX = followFrozen
//            ? frozenX
//            : (rocket != null ? rocket.position.x : lastKnownRocketX);

//        Vector3 pos = cam.transform.position;
//        pos.x = targetX;
//        pos.y = fixedY;
//        pos.z = fixedZ;

//        cam.transform.position = pos + shakeOffset; // shake ekle
//    }


//    private void OnStateChanged(GameState state)
//    {
//        if (state == GameState.Running && rocket != null)
//        {
//            Vector3 pos = cam.transform.position;
//            pos.x = rocket.position.x;
//            pos.y = fixedY;
//            pos.z = fixedZ;
//            cam.transform.position = pos;

//            // Shake & kilit sıfırla
//            shakeTimer = 0f;
//            shakeOffset = Vector3.zero;
//            UnfreezeFollow();
//            lastKnownRocketX = rocket.position.x;
//        }
//    }


//    public void Shake(float amplitude, float duration)
//    {
//        shakeAmp = amplitude;
//        shakeDuration = Mathf.Max(0.0001f, duration);
//        shakeTimer = shakeDuration;
//    }
//    public void Shake() => Shake(defaultShakeAmp, defaultShakeDuration);

//    public void FreezeFollowAtCurrentX()
//    {
//        followFrozen = true;
//        frozenX = (rocket != null) ? rocket.position.x : cam.transform.position.x;
//        lastKnownRocketX = frozenX; // emniyet
//    }

//    public void UnfreezeFollow()
//    {
//        followFrozen = false;
//    }


//}
