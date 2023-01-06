using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPRestoreItemController : MonoBehaviour
{
    [SerializeField] private int healValue;
    private void OnTriggerEnter2D(Collider2D info)
    {
        info.GetComponent<Player_controller>().RestoreHP(healValue);
        Destroy(gameObject);
    }
}
