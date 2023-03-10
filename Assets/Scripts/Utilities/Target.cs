using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public SpawningZone[] SpawningZones;
    [SerializeField] protected float _speed;
    [SerializeField] protected float _maxDistance;
    protected Vector3 _moveDest;

    protected Vector3 _basePoint;
    protected Vector3 _destPoint;
    protected MeshRenderer _mesh;

    [SerializeField] protected Color _touchedColor;

    protected bool _isTouched = false;

    protected virtual void Start()
    {
        _mesh = GetComponent<MeshRenderer>();
        gameObject.tag = "Target";

        int index = Random.Range(0, SpawningZones.Length);

        _moveDest = SpawningZones[index].dir;
        _basePoint = GetRandomLoc(SpawningZones[index].Coll);
        transform.position = _basePoint;
        _destPoint = _basePoint + (_moveDest * _maxDistance);
    }

    private Vector3 GetRandomLoc(Collider collider)
    {
        float x = Random.Range(collider.bounds.min.x, collider.bounds.max.x);
        float y = Random.Range(collider.bounds.min.y, collider.bounds.max.y);
        float z = Random.Range(collider.bounds.min.z, collider.bounds.max.z);

        if (_moveDest.x != 0)
            x = collider.bounds.center.x;
        if (_moveDest.y != 0)
            y = collider.bounds.center.y;
        if (_moveDest.z != 0)
            z = collider.bounds.center.z;

        return new Vector3(x, y, z);
    }

    protected virtual void Update()
    {
        if (!_isTouched)
            Move();
    }

    protected virtual void Move()
    {
        if (Vector3.Distance(transform.position, _destPoint) < 0.2f)
        {
            Vector3 tmp = _destPoint;
            _destPoint = _basePoint;
            _basePoint = tmp;
        }

        transform.Translate(Time.deltaTime * _speed * (_destPoint - _basePoint).normalized);
    }

    public virtual void Hit()
    {
        if (!_isTouched)
        {
            _mesh.material.color = _touchedColor;
            _isTouched = true;
            Destroy(gameObject, 0.5f);
        }
    }
}
