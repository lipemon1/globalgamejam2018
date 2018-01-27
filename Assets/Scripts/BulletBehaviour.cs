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

    [Header("Debug")]
    [SerializeField]
    private bool _canBePicked;

    [Header("Energy Amount")]
    [SerializeField]
    private int _energyAmount;

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
            if (Physics.Linecast(transform.position, destinationPosition, out hit))
            {
                transform.Translate(direction * hit.distance, Space.World);
                direction = Vector3.Reflect(direction, hit.normal);
            }
            else
            {
                transform.Translate(direction * _currentSpeed * Time.deltaTime, Space.World);
            }
            yield return null;
        }

        BulletStop();
    }

    private void BulletStop()
    {
        SetCanBePicked(true);
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
