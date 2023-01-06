using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TakeCoinsItemController : MonoBehaviour
{
    [SerializeField] private int value;

    private void OnTriggerEnter2D(Collider2D info)
    {
        info.GetComponent<Player_controller>().TakeCoins(value);
        Destroy(gameObject);
    }
}
