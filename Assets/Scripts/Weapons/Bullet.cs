using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public Vector3 direction;
    public float speed = 1000;

    private Rigidbody _rb;

    private void Start()
    {
        direction = transform.up;
        _rb = GetComponent<Rigidbody>();
        _rb.AddForce(direction * speed, ForceMode.Impulse);
    }

    private void Update()
    {
        RaycastHit hit;

        Vector3 nextPosition = transform.position + _rb.velocity * Time.deltaTime;

        if (Physics.Raycast(transform.position, direction, out hit, nextPosition.magnitude))
        {
            Debug.DrawLine(transform.position, hit.point, Color.red, 2f);
            if (hit.collider.CompareTag("Target"))
                hit.collider.gameObject.GetComponent<Target>().Hit();
            Destroy(gameObject, 2f);
        }

    }
}
