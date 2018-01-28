using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _startSpeed = 50;
    [SerializeField]
    private float _currentSpeed = 0;
    [SerializeField]
    private float _radius = 1;
    [SerializeField]
    private string _playerTag = "Player";

    [Header("Debug")]
    [SerializeField]
    private bool _canBePicked;
    [SerializeField] private int _ownerId = -1;

    [Header("Energy Amount")]
    [SerializeField]
    private int _energyAmount;
    [SerializeField]
    private ParticleSystem ChargeParticle;

    [Header("Bullet")]
    [SerializeField]
    private GameObject _bulletPrefab;
    [Range(0, 10)]
    [SerializeField]
    private float _distanceToChildrenBullets = 0.25f;

    public void Fire(float distance, int ownerId)
    {
        StartCoroutine(FireCo(distance));
        _ownerId = ownerId;
    }

    public IEnumerator FireCo(float distance)
    {
        Vector3 direction = transform.forward;
        float a = -Mathf.Pow(_startSpeed, 2) / 2 / distance;

        _currentSpeed = _startSpeed;
        while (_currentSpeed > 0)
        {
            _currentSpeed += a * Time.deltaTime;
            Vector3 destinationPosition = transform.position + direction * _currentSpeed * Time.deltaTime;
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, _radius, direction, out hit, _currentSpeed * Time.deltaTime))
            //if (Physics.Linecast(transform.position, destinationPosition, out hit))
            {
                transform.Translate(direction * hit.distance, Space.World);
                direction = Vector3.Reflect(direction, hit.normal);
                CheckAndKillPlayer(hit.collider);
            }
            else
            {
                transform.Translate(direction * _currentSpeed * Time.deltaTime, Space.World); 
                //Rigidbody rb = GetComponent<Rigidbody>();
                //rb.velocity = (_currentSpeed * direction * Time.deltaTime + transform.position) - transform.position;
            }
            yield return null;
        }

        BulletStop();
    }

    private void CheckAndKillPlayer(Collider col)
    {
        if (col.gameObject.CompareTag(_playerTag))
        {
            CollactableHandler collactableHandler = col.gameObject.GetComponent<CollactableHandler>();
            if (collactableHandler != null)
            {
                EnergyHandler energyHandler = col.gameObject.GetComponent<EnergyHandler>();
                energyHandler.TryRecieveDamage(_energyAmount, () => KillPlayer(collactableHandler));
            }
        }
    }

    private void KillPlayer(CollactableHandler collactableHandler)
    {
        collactableHandler.KillPlayer();
        Global.AddKill(_ownerId);
    }

    private void BulletStop()
    {
        SetCanBePicked(true);

        if (_energyAmount > 1)
            SpawnNewBullets(_energyAmount);
    }

    private void SpawnNewBullets(int energyAmount)
    {
        for (int i = 0; i < energyAmount; i++)
        {
            GameObject newBullet = Instantiate(_bulletPrefab, transform.position, Quaternion.Euler(GetRandomDirection()));
            BulletBehaviour bulletBehaviour = newBullet.GetComponent<BulletBehaviour>();
            bulletBehaviour.SetEnergyAmount(1);
            bulletBehaviour.Fire(_distanceToChildrenBullets, _ownerId);
            newBullet.transform.localScale = Vector3.one;
        }
        SetEnergyAmount(1);
        Destroy(gameObject); 
    }

    Vector3 GetRandomDirection()
    {
        float yRot = Random.Range(0, 360);

        return new Vector3(0f, yRot, 0f);
    }

    public bool CanBePicked() { return _canBePicked; }
    public void SetCanBePicked(bool value)
    {
        _canBePicked = value;
        transform.localScale *= 1.5f;
    }

    public int GetEnergyAmount() { return _energyAmount; }

    public void SetEnergyAmount(int amount)
    {
        _energyAmount = amount;
#pragma warning disable CS0618 // O tipo ou membro é obsoleto
        ChargeParticle.startSize = amount / 5f;
#pragma warning restore CS0618 // O tipo ou membro é obsoleto
    }
}
