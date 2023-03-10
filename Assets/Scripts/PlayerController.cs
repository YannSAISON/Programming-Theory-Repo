using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Rigidbody _rb;

    private float _horizontalMove;
    private float _verticalMove;

    [SerializeField] private Camera _playerCamera;
    [SerializeField] private Transform _shootPoint;

    [SerializeField] private float _lookSpeed = 2.0f;
    [SerializeField] private float _lookXLimit = 80.0f;
    private float _rotationX;
    private bool _isShooting;
    private bool _canShoot;

    [HideInInspector] public Weapon currentWeapon;
    private GameObject currentWeaponGameObject;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Rotate()
    {
        _rotationX += -Input.GetAxis("Mouse Y") * _lookSpeed;
        _rotationX = Mathf.Clamp(_rotationX, -_lookXLimit, _lookXLimit);
        _playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        _shootPoint.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        if (currentWeapon != null)
            currentWeapon.transform.localRotation = Quaternion.Euler(_rotationX, 0, currentWeapon.transform.rotation.eulerAngles.z);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * _lookSpeed, 0);
    }

    private void Update()
    {
        _horizontalMove = Input.GetAxis("Horizontal");
        _verticalMove = Input.GetAxis("Vertical");

        Rotate();

        if (Input.GetMouseButtonUp(0) && currentWeapon != null)
            _isShooting = false;

        if (Input.GetMouseButtonDown(0) && currentWeapon != null && _canShoot)
        {
            currentWeapon.Shoot();
            _isShooting = true;
        } else if (_isShooting && _canShoot)
            currentWeapon.Shoot();

        if (Input.GetMouseButtonDown(1) && currentWeapon != null)
            currentWeapon.Recharge();

    }

    private void FixedUpdate()
    {
        Vector3 move = _horizontalMove * Time.fixedDeltaTime * _speed * transform.right;

        move += _verticalMove * Time.fixedDeltaTime * _speed * transform.forward;
        
        Move(move);
    }

    private void Move(Vector3 direction)
    {
        _rb.velocity = direction;
    }

    private IEnumerator ActivateShoot()
    {
        _canShoot = false;

        yield return new WaitForSeconds(0.1f);
        _canShoot = true;
    }

    public void EquipWeapon(GameObject prefab)
    {
        if (currentWeaponGameObject != null)
        {
            currentWeapon.Drop();
            Destroy(currentWeaponGameObject);
        }
        currentWeaponGameObject = Instantiate(prefab, _shootPoint.position - prefab.GetComponent<Collider>().bounds.size / 2, Quaternion.Euler(0, 0, 90), transform);
        currentWeapon = currentWeaponGameObject.GetComponent<Weapon>();

        currentWeapon.ResetMaterial();

        currentWeapon.ShootPoint = _shootPoint;
        StartCoroutine(nameof(ActivateShoot));
    }
}
