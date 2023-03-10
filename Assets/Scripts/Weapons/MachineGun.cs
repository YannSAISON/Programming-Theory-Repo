using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// INHERITANCE
public class MachineGun : Weapon
{
    [SerializeField] private float _strayFactor;

    [SerializeField] private float _maxHeat = 20;
    [SerializeField] private float _heatDecreaseTime = 0.3f;
    [SerializeField] private float _baseHeatDecrease = 1f;
    private float _heatCounter;
    private float _heating = 0;
    private float _heatDecrease;
    private bool _overHeat = false;

    [SerializeField] private Slider _heatBar;

    // POLYMORPHISM
    // Start is called before the first frame update
    new void Awake()
    {
        _cadency = 0.08f;
        _damages = 1;
        _magazineCapacity = 50;
        _rechargeTime = 3f;
        _heatBar = GameObject.Find("Player").GetComponent<PlayerUiManager>().HeatBar;
        _heatDecrease = _baseHeatDecrease;
        _heatBar.maxValue = _maxHeat;
        
        base.Awake();
    }

    // POLYMORPHISM
    private new void Update()
    {
        _heatCounter -= Time.deltaTime;

        if (_heatCounter <= 0 && _heating > 0)
        {
            _heating -= _heatDecrease;
            _heatBar.value = _heating;
            _heatCounter = _heatDecreaseTime;
        }

        base.Update();
    }

    private float RandomStray()
    {
        return Random.Range(-_strayFactor * (1 + ((float)_heating / (float)_maxHeat)), _strayFactor * (1 + ((float)_heating / (float)_maxHeat)));
    }

    private IEnumerator OverheatCoroutine()
    {
        _overHeat = true;
        _heatDecrease /= 1.5f;
        yield return new WaitForSeconds(_maxHeat * _heatDecreaseTime * 1.5f);
        _overHeat = false;
        _heatDecrease = _baseHeatDecrease;
    }

    private void IncreaseHeat()
    {
        _heating += 1;
        _heatBar.value = _heating;

        if (_heating >= _maxHeat)
        {
            StartCoroutine(nameof(OverheatCoroutine));
        }
    }

    // POLYMORPHISM
    protected override void PickedUp()
    {
        if (!_heatBar.gameObject.activeSelf)
            _heatBar.gameObject.SetActive(true);
        base.PickedUp();
    }

    // POLYMORPHISM
    public override void Shoot()
    {
        float newTime = Time.time;
        _timer -= newTime - _lastTime;

        _isShooting = true;
        StopCoroutine(nameof(RechargeRoutine));

        if (_timer <= 0 && AmountRemaining > 0 && !_overHeat)
        {
            Vector3 addRotation = new Vector3(RandomStray(), RandomStray(), RandomStray());

            Instantiate(bulletPrefab, shootPoint.position, Quaternion.FromToRotation(Vector3.forward, shootPoint.forward) * bulletPrefab.transform.rotation * Quaternion.Euler(addRotation));

            _timer = _cadency;
            UpdateAmmunitions(AmountRemaining - 1);
            IncreaseHeat();
        }
        _lastTime = newTime;
    }

    // POLYMORPHISM
    public override void Drop()
    {
        _heatBar.gameObject.SetActive(false);
        base.Drop();
    }
}
