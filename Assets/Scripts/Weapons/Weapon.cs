using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weapon : Interactable
{
    protected int AmountRemaining { get; set; }
    protected float _timer;

    protected float _lastTime;
    protected bool _isShooting;


    [SerializeField] protected TextMeshProUGUI ammunitionsText;
    [SerializeField] protected GameObject bulletPrefab;

    protected Transform shootPoint;
    public Transform ShootPoint
    {
        get => shootPoint;
        set
        {
            shootPoint = value;
        }
    }

    protected float _damages;
    public float Damages
    {
        get => _damages;
        protected set
        {
            if (value <= 0)
                Debug.LogError("Damage Value should be more than 0.");
            _damages = value;
        }
    }

    protected int _magazineCapacity;
    public int MagazineCapacity
    {
        get => _magazineCapacity;
        protected set
        {
            if (value <= 0)
                Debug.LogError("Magazine capacity should be more than 0.");
            _magazineCapacity = value;
        }
    }

    protected float _cadency;
    public float Cadency
    {
        get => _cadency;
        protected set
        {
            if (value <= 0)
                Debug.LogError("Cadency should be more than 0.");
            _cadency = value;
        }
    }

    protected float _rechargeTime;
    public float RechargeTime
    {
        get => _rechargeTime;
        protected set
        {
            if (value <= 0)
                Debug.LogError("Recharge Time should be more than 0.");
            _rechargeTime = value;
        }
    }

    [SerializeField] private PlayerController _player;

    protected new void Awake()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerController>();
        AmountRemaining = _magazineCapacity;
        _lastTime = Time.time;
        ammunitionsText = GameObject.Find("Player").GetComponent<PlayerUiManager>().AmmunitionText;

        base.Awake();
    }

    protected virtual void PickedUp()
    {
        GameObject gameObjectCopy = gameObject;
        gameObjectCopy.GetComponent<MeshRenderer>().material.color = m_baseColor;
        _player.EquipWeapon(gameObjectCopy);
        if (!ammunitionsText.gameObject.activeSelf)
            ammunitionsText.gameObject.SetActive(true);
        UpdateAmmunitions(AmountRemaining);
    }

    protected new void Update()
    {
        if (Input.GetMouseButtonDown(0) && isHovered)
        {
            PickedUp();
        }
        base.Update();
    }

    protected void UpdateAmmunitions(int nb)
    {
        AmountRemaining = nb;
        ammunitionsText.text = AmountRemaining + "/" + _magazineCapacity;
    }

    public virtual void Shoot()
    {
        float newTime = Time.time;
        _timer -= newTime - _lastTime;
        _isShooting = true;
        StopCoroutine(nameof(RechargeRoutine));

        if (_timer <= 0 && AmountRemaining > 0)
        {
            Instantiate(bulletPrefab, shootPoint.position, Quaternion.FromToRotation(Vector3.forward, shootPoint.forward) * bulletPrefab.transform.rotation);
            _timer = _cadency;
            UpdateAmmunitions(AmountRemaining - 1);
        }
        _lastTime = newTime;
    }

    protected virtual IEnumerator RechargeRoutine()
    {
        yield return new WaitForSeconds(_rechargeTime);
        UpdateAmmunitions(_magazineCapacity);
    }

    public virtual void Recharge()
    {
        _isShooting = false;
        StartCoroutine(nameof(RechargeRoutine));
    }

    public virtual void Drop()
    {
        ammunitionsText.gameObject.SetActive(false);
    }
}
