using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyHandler : MonoBehaviour {

    [Header("Debug")]
    [SerializeField] private bool _canShoot;
    [SerializeField] private bool _shieldIsOn;
    [SerializeField] private bool _holdingShot;

    [Header("Shield Params")]
    [SerializeField] private GameObject _shieldObject;

    [Header("Shoot Params")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _standardBulletVelocity = 1f;
    [SerializeField] private float _bulletDistance = 0;
    [Range(0,1)]
    [SerializeField] private float _maxForceToShoot = 1f;
    [SerializeField] private float _bulletShootForce = 0f;
    [SerializeField] private float _forceMultiplier = 0.5f;
    [SerializeField] private float _testForceMultiplier = 10f;

    [Header("Distances")]
    [SerializeField] private float _minBulletDistance = 2.5f;
    [SerializeField] private float _maxBulletDistance = 10f;

    [Header("Player Data")]
    [SerializeField] private PlayerData _playerData;

	// Use this for initialization
	void Start () {
        Invoke("EnableShooting", 1f);
        _playerData.SetEnergyAmount(1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void EnableShooting()
    {
        _canShoot = true;
    }

    /// <summary>
    /// Tenta receber o dano da energia recebida
    /// </summary>
    /// <param name="damageAmount"></param>
    public void TryRecieveDamage(int damageAmount)
    {
        if (_shieldIsOn)
        {
            if (damageAmount <= _playerData.GetEnergyAmount())
            {
                ToogleShield(false);
            }
            else
            {
                OnDeath();
            }
        }
    }

    private void OnDeath()
    {

    }

    public void ToogleShield(bool value)
    {
        _shieldIsOn = value;
        _shieldObject.SetActive(value);
    }

    public void SetHoldingShot(bool value, float forceValue)
    {
        _holdingShot = value;

        if (_bulletShootForce < _maxForceToShoot)
            _bulletShootForce = forceValue;
            //_bulletShootForce = forceValue * _forceMultiplier;
        else
            _bulletShootForce = _maxForceToShoot;
    }

    public void TryToShoot()
    {
        if (CanShoot())
        {
            _holdingShot = false;
            float force = _bulletShootForce;
            _bulletShootForce = 0f;

            _playerData.SetEnergyAmount(-1);

            Debug.LogWarning("Shoot with force: " + force.ToString("F2"));

            Shoot(_bulletPrefab, force);
        }
        else
        {
            Debug.LogWarning("You tried to shoot but the shoot system is not ready yet.");
        }
    }

    private void Shoot(GameObject bulletPrefab, float holdTime)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);

        //bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * force * _standardBulletVelocity);
        _bulletDistance = Mathf.Lerp(_minBulletDistance, _maxBulletDistance, Mathf.InverseLerp(0, _maxForceToShoot, holdTime));

        BulletBehaviour bulletBehaviour = bullet.GetComponent<BulletBehaviour>();
        bulletBehaviour.Set(_standardBulletVelocity, _bulletDistance);
    }

    /// <summary>
    /// Se o jogador pode atirar ou não
    /// </summary>
    /// <returns></returns>
    private bool CanShoot()
    {
        return (_playerData.GetEnergyAmount() > 0 && _canShoot);
    }
}
