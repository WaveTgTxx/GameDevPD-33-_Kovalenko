using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPRestoreItemController : MonoBehaviour
{
    [SerializeField] private int value;
    private void OnTriggerEnter2D(Collider2D info)
    {
        info.GetComponent<Player_controller>().ChangeMp(value);
        Destroy(gameObject);
    }
}
