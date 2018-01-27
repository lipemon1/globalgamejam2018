﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyHandler : MonoBehaviour {

    [Header("Debug")]
    [SerializeField] private bool _shieldIsOn;
    [SerializeField] private bool _holdingShot;

    [Header("Shield Params")]
    [SerializeField] private GameObject _shieldObject;

    [Header("Player Data")]
    [SerializeField] private PlayerData _playerData;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Tenta receber o dano da energia recebida
    /// </summary>
    /// <param name="damageAmount"></param>
    public void TryRecieveDamage(int damageAmount)
    {
        if(_shieldIsOn)
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

    public void SetHoldingShot(bool value)
    {
        _holdingShot = value;
    }
}