using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningZone : MonoBehaviour
{
    public Vector3 dir;

    private Collider _collider;

    public Collider Coll
    {
        get { return _collider; }
        private set { _collider = value; }
    }

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }
}
