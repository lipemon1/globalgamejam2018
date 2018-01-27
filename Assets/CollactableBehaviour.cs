using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollactableBehaviour : MonoBehaviour {

	[Header("Debug")]
    [SerializeField] private bool _canBePicked;

    [Header("Energy Amount")]
    [SerializeField] private int _energyAmount;

    public bool CanBePicked() { return _canBePicked; }
    public void SetCanBePicked(bool value)
    {
        _canBePicked = value;
        transform.localScale *= 1.5f;
    }

    public int GetEnergyAmount() { return _energyAmount; }
}
