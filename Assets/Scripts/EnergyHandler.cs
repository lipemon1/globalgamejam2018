using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyHandler : MonoBehaviour {

    [Header("Debug")]
    [SerializeField] private bool _canRecieveDamage;
    [SerializeField] private bool _canShoot;
    [SerializeField] private bool _shieldIsOn;
    [SerializeField] private bool _holdingShot;

    [Header("Shield Params")]
    [SerializeField] private GameObject _shieldObject;

    [Header("Shoot Params")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private ParticleSystem ChargingParticle;
    [SerializeField] private float _standardBulletVelocity = 1f;
    [SerializeField] private float _bulletDistance = 0;
    [Range(0,1)]
    [SerializeField] private float _maxForceToShoot = 1f;
    [SerializeField] private float _bulletShootForce = 0f;
    [SerializeField] private float _forceMultiplier = 0.5f;
    [SerializeField] private float _testForceMultiplier = 10f;

    [Header("Distances")]
    [SerializeField] private Transform _shootSpawner;
    [SerializeField] private float _minBulletDistance = 2.5f;
    [SerializeField] private float _maxBulletDistance = 10f;

    [Header("Player Data")]
    [SerializeField] private PlayerData _playerData;

	// Use this for initialization
	void Start () {
        Invoke("EnableShooting", 1f);
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
        if (_canRecieveDamage)
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
        {
            _bulletShootForce = forceValue;
#pragma warning disable CS0618 // O tipo ou membro é obsoleto
            ChargingParticle.startSize = _bulletShootForce / 2;
#pragma warning restore CS0618 // O tipo ou membro é obsoleto
        }
        else
            _bulletShootForce = _maxForceToShoot;
#pragma warning disable CS0618 // O tipo ou membro é obsoleto
        ChargingParticle.startSize = 0.5f;
#pragma warning restore CS0618 // O tipo ou membro é obsoleto
    }

    /// <summary>
    /// Recebe uma energia que foi coletada
    /// </summary>
    /// <param name="amount"></param>
    public void RecieveSomeEnergy(int amount)
    {
        _playerData.SetEnergyAmount(amount);
    }

    public void TryToShoot()
    {
        if (CanShoot())
        {
            float force = _bulletShootForce;
            _bulletShootForce = 0f;
            Debug.LogWarning("Shoot with force: " + force.ToString("F2"));

            Shoot(_bulletPrefab, force, _playerData.GetEnergyAmount());
        }
        else
        {
            Debug.LogWarning("You tried to shoot but the shoot system is not ready yet.");
        }

        _holdingShot = false;
    }

    private void Shoot(GameObject bulletPrefab, float holdTime, int energyAmount)
    {
        _playerData.SetEnergyAmount(-energyAmount);
        GameObject bullet = Instantiate(bulletPrefab, _shootSpawner.transform.position, transform.rotation);

        bullet.GetComponent<BulletBehaviour>().SetEnergyAmount(energyAmount);

        _bulletDistance = Mathf.Lerp(_minBulletDistance, _maxBulletDistance, Mathf.InverseLerp(0, _maxForceToShoot, holdTime));

        BulletBehaviour bulletBehaviour = bullet.GetComponent<BulletBehaviour>();
        bulletBehaviour.Fire(_bulletDistance);
#pragma warning disable CS0618 // O tipo ou membro é obsoleto
        ChargingParticle.startSize = 0f;
        ChargingParticle.Clear();
#pragma warning restore CS0618 // O tipo ou membro é obsoleto
    }

    /// <summary>
    /// Retorna a quantidade de energia atual do jogador
    /// </summary>
    /// <returns></returns>
    public int GetPlayerEnergyAmount()
    {
        return _playerData.GetEnergyAmount();
    }

    /// <summary>
    /// Se o jogador pode atirar ou não
    /// </summary>
    /// <returns></returns>
    private bool CanShoot()
    {
        return (_playerData.GetEnergyAmount() > 0 && _canShoot);
    }

    public void SetCanRecieveDamage(bool value) { _canRecieveDamage = value; }
}
