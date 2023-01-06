using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private int damage;

    private void OnTriggerEnter2D(Collider2D info)
    {
        EnemiesController enemy = info.GetComponent<EnemiesController>();
        if (enemy != null)
            enemy.TakeDamage(damage);
        Destroy(gameObject);
    }
}
