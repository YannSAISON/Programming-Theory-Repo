using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITANCE
public class RotatingTarget : Target
{
    // POLYMORPHISM
    protected override void InstantiateMovement()
    {
        int index = Random.Range(0, SpawningZones.Length);

        _moveDest = SpawningZones[index].dir;
        _destPoint = SpawningZones[index].Coll.bounds.center;

        var vector2 = Random.insideUnitCircle.normalized * _maxDistance;

        Vector3 distanceFromCenter = new Vector3(vector2.x * (_moveDest.x == 0 ? 1 : 0),
            vector2.x * (_moveDest.x != 0 ? 1 : 0) + vector2.y * (_moveDest.y == 0 ? 1 : 0),
            vector2.y * (_moveDest.z == 0 ? 1 : 0));

        _basePoint = _destPoint + distanceFromCenter;
        Debug.Log("BasePoint = " + _basePoint);
        transform.position = _basePoint;
    }

    // POLYMORPHISM
    protected override void Move()
    {
        transform.RotateAround(_destPoint, _moveDest, Time.deltaTime * _speed);
    }
}
