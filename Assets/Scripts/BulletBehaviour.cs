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

    [Header("Debug")]
    [SerializeField]
    private bool _canBePicked;

    [Header("Energy Amount")]
    [SerializeField]
    private int _energyAmount;

    [Header("Bullet")]
    [SerializeField]
    private GameObject _bulletPrefab;
    [Range(0, 10)]
    [SerializeField]
    private float _distanceToChildrenBullets = 0.25f;

    public void Fire(float distance)
    {
        StartCoroutine(FireCo(distance));
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
            if(Physics.SphereCast(transform.position, _radius, direction, out hit, _currentSpeed * Time.deltaTime))
            //if (Physics.Linecast(transform.position, destinationPosition, out hit))
            {
                transform.Translate(direction * hit.distance, Space.World);
                direction = Vector3.Reflect(direction, hit.normal);
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
            bulletBehaviour.Fire(_distanceToChildrenBullets);
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

    public void SetEnergyAmount(int amount) { _energyAmount = amount; }
}
