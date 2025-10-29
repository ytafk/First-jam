using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float boostSpeed = 12f;   // X ekseninde gideceði hýz
    public float duration = 1.0f;    // Kaç saniye

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var rocket = other.GetComponent<RocketController>();
            if (rocket != null)
            {
                rocket.ActivateXBoost(boostSpeed, duration);
            }
            Destroy(gameObject);
        }
    }
}
