using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RocketController : MonoBehaviour
{
    public float speed = 6f;
    public float maxTurnDegPerSec = 180f;
    public float turnGain = 1f;

    public GameObject rocketFire; // 🚀 Ateş GameObject’i
    //public GameObject explosionPrefab;

    private Rigidbody2D rb;
    private Vector2 dir;
    private bool running = false;
    private bool xBoostActive = false;
    private float xBoostEndTime = 0f;
    private float xBoostStartSpeed = 0f;
    private float xBoostDuration = 0f;
    private bool hasDied = false;
    [SerializeField] private string[] hazardTags = new string[] { "MobingObstacle", "Obstacle" };
    [SerializeField] private GameObject explosionPrefab;
    private bool isArmed = false; // patlama için hazır mı?


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        dir = transform.up.normalized;
        if (rocketFire != null) rocketFire.SetActive(false); // oyun başlarken kapalı
    }

    // === State kontrol ===
    public void Begin()
    {
        running = true;
        isArmed = true; // <- eklendi
        if (rocketFire != null) rocketFire.SetActive(true);
    }

    public void Run()
    {
        running = true;
        isArmed = true; // <- eklendi
        if (rocketFire != null) rocketFire.SetActive(true);
    }


    public void Stop()
    {
        running = false;
        rb.velocity = Vector2.zero;
        if (rocketFire != null) rocketFire.SetActive(false); // ateşi kapat

        
    }

    public void Die()
    {
        if (hasDied) return;
        hasDied = true;

        if (rocketFire != null) rocketFire.SetActive(false);

        // Patlama efekti
        if (explosionPrefab != null)
        {
            var vfx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            var systems = vfx.GetComponentsInChildren<ParticleSystem>(true);
            foreach (var s in systems) s.Play();
            if (systems.Length > 0)
            {
                float maxLife = 0f;
                foreach (var s in systems)
                {
                    var m = s.main;
                    maxLife = Mathf.Max(maxLife, m.duration + m.startLifetime.constantMax);
                }
                var cm = CameraManager.Instance;
                var cam = Camera.main;
                if (cam != null)
                {
                    var shaker = cam.GetComponent<CameraShake2D>();
                    if (shaker != null)
                        shaker.Shake(0.5f, 0.3f); // (şiddet, süre)
                }


                Destroy(vfx, maxLife + 0.1f);

            }
        }

        

        Destroy(gameObject);
    }

    void Update()
    {
        // Space'e basılırsa kendini kur
        if (!isArmed && Input.GetKeyDown(KeyCode.Space))
            isArmed = true;

        // Hareket başladıysa da kur (ör. başka yerden hız verildi)
        if (!isArmed && rb != null && rb.velocity.sqrMagnitude > 0.01f)
            isArmed = true;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleHit(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleHit(collision.collider.gameObject);
    }
    private void HandleHit(GameObject other)
    {
        //Debug.Log($"HandleHit: other={other.name}, tag={other.tag}, running={running}, isArmed={isArmed}");
        if (!isArmed) return; // yalnızca kuruluysa öl

        if (other.CompareTag("Obstacle"))
        {
            Die();
        }
    }







    public void ActivateXBoost(float boostSpeed, float duration)
    {
        xBoostActive = true;
        xBoostStartSpeed = boostSpeed;
        xBoostDuration = duration;
        xBoostEndTime = Time.time + duration;

        rb.velocity = Vector2.zero; // hız sıfırla
    }


    void FixedUpdate()
    {
        if (!running) return;

        if (xBoostActive)
        {
            // Geçen süre oranını hesapla (0 → 1 arası)
            float t = 1f - ((xBoostEndTime - Time.time) / xBoostDuration);
            t = Mathf.Clamp01(t);

            // Hız lineer olarak azalsın (başta hızlı, sona doğru yavaş)
            float currentSpeed = Mathf.Lerp(xBoostStartSpeed, 3.2f, t);

            // X yönünde hareket (y sabit)
            rb.MovePosition(rb.position + Vector2.right * currentSpeed * Time.fixedDeltaTime);

            // ====== BOOST BİTİŞ ANINDA YÖNÜ SIFIRLA ======
            if (Time.time >= xBoostEndTime)
            {
                xBoostActive = false;

                // Yönü DÜMDÜZ +X'e kilitle (önceki eğrisel yönü unut)
                dir = Vector2.right;

                // Olası fizik artıklarını temizle (özellikle hızlı sahnelerde)
                rb.velocity = Vector2.zero;
            }

            return; // boost varken normal steer akışını atla
        }

        Vector2 steer = Vector2.zero;
        foreach (var a in Attractor.All)
            steer += a.Steer(rb.position, dir);

        float desiredAngle = Vector2.SignedAngle(dir, dir + steer);
        float maxStep = maxTurnDegPerSec * Time.fixedDeltaTime;
        float step = Mathf.Clamp(desiredAngle * turnGain, -maxStep, maxStep);

        dir = (Vector2)(Quaternion.Euler(0, 0, step) * dir);
        dir.Normalize();

        if (dir.x < 0f)
            dir.x = 0f;
        dir.Normalize();

        rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);

        // İlerle
        rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);

        // 🔸 Eğer power-up aktifse, x yönüne ekstra itme uygula
        

    }
}

//using System.Collections;
//using UnityEngine;
//using System.Collections.Generic;





//[RequireComponent(typeof(Rigidbody2D))]
//public class RocketController : MonoBehaviour
//{
//    public float speed = 6f;
//    public float maxTurnDegPerSec = 180f;
//    public float turnGain = 1f;

//    private Rigidbody2D rb;
//    private Vector2 dir;
//    private bool running = false;

//    void Awake()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        dir = transform.up.normalized; // sağa bakacak şekilde başlat
//    }

//    // === State kontrol ===
//    public void Begin()  // eski isim
//    {
//        running = true;
//    }

//    public void Run()    // GameManager için uyumlu isim
//    {
//        running = true;
//    }

//    public void Stop()
//    {
//        running = false;
//        rb.velocity = Vector2.zero;
//    }

//    void FixedUpdate()
//    {
//        if (!running) return;

//        Vector2 steer = Vector2.zero;
//        foreach (var a in Attractor.All)
//            steer += a.Steer(rb.position, dir);

//        // Yönü güncelle
//        float desiredAngle = Vector2.SignedAngle(dir, dir + steer);
//        float maxStep = maxTurnDegPerSec * Time.fixedDeltaTime;
//        float step = Mathf.Clamp(desiredAngle * turnGain, -maxStep, maxStep);

//        dir = (Vector2)(Quaternion.Euler(0, 0, step) * dir);
//        dir.Normalize();

//        // 🚀 X ekseninde geri gitmeyi engelle
//        if (dir.x < 0f)
//            dir.x = 0f;
//        dir.Normalize();

//        // İlerle
//        rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);
//    }
//}
