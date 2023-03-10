using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITANCE
public class Shotgun : Weapon
{
    [SerializeField] private float _strayFactor = 10;
    [SerializeField] private int _nbBullet = 9;

    // POLYMORPHISM
    // Start is called before the first frame update
    new void Awake()
    {
        _cadency = 0.8f;
        _damages = 5;
        _magazineCapacity = 5;
        _rechargeTime = 5f;
        base.Awake();
    }

    private float RandomStray()
    {
        return Random.Range(-_strayFactor, _strayFactor);
    }

    private void FireBullet()
    {
        Vector3 addRotation = new Vector3(RandomStray(), RandomStray(), RandomStray());

        Instantiate(bulletPrefab, shootPoint.position, Quaternion.FromToRotation(Vector3.forward, shootPoint.forward) * bulletPrefab.transform.rotation * Quaternion.Euler(addRotation));
    }

    // POLYMORPHISM
    public override void Shoot()
    {
        float newTime = Time.time;
        _timer -= newTime - _lastTime;
        _isShooting = true;
        StopCoroutine(nameof(RechargeRoutine));

        if (_timer <= 0 && AmountRemaining > 0)
        {
            for (int i = 0; i < _nbBullet; i++)
                FireBullet();

            _timer = _cadency;
            UpdateAmmunitions(AmountRemaining - 1);
        }
        _lastTime = newTime;
    }

    // POLYMORPHISM
    protected override IEnumerator RechargeRoutine()
    {
        yield return new WaitForSeconds(_rechargeTime / _magazineCapacity);
        UpdateAmmunitions(AmountRemaining + 1);
        if (AmountRemaining < _magazineCapacity && !_isShooting)
            StartCoroutine(nameof(RechargeRoutine));
    }
}
