using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMover : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float range;
    private Vector2 startPoint;
    private int direction = 1;
    void Start()
    {
        startPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y - startPoint.y > range && direction > 0)
            direction *= -1;
        else if (startPoint.y - transform.position.y > range && direction < 0)
            direction *= -1;

        transform.Translate(0, speed * direction * Time.deltaTime, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(0.5f, range * 2, 0));
    }
}
