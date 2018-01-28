using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour {

    [Header("Player Anim")]
    [HideInInspector] private Animator _playerAnim;

    private void Awake()
    {
        _playerAnim = GetComponentInChildren<Animator>();
    }

    public void UpdateMoveAnimations(float walking)
    {
        _playerAnim.SetFloat("IsWalking", walking);
    }

    public void UpdateChargingValue(float chargingAmount)
    {
        _playerAnim.SetFloat("ChargingAmount", chargingAmount);
    }

    private void DeathAnimation()
    {
        _playerAnim.SetInteger("Death", Random.Range(0,3));
    }

    [ContextMenu("Die")]
    public void Die()
    {
        DeathAnimation();
        _playerAnim.SetTrigger("IsDead");
    }

    public void Shoot()
    {
        UpdateChargingValue(0f);
        _playerAnim.SetTrigger("Shoot");
    }

    public void Respawn()
    {
        _playerAnim.SetTrigger("Respawn");
    }

    public void Victory()
    {
        _playerAnim.SetTrigger("Win");
    }
}
