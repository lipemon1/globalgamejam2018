using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData {
    [SerializeField] private int _energyAmount;

    public int GetEnergyAmount() { return _energyAmount; }
    public void SetEnergyAmount(int amountToAdd)
    {
        _energyAmount += amountToAdd;

        if (_energyAmount < 0)
            _energyAmount = 0;
    }
}
