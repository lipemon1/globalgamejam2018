using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour {

    [Header("Player Anim")]
    [SerializeField] private Animator _playerAnim;

    public void UpdateMoveAnimations(float walking)
    {
        _playerAnim.SetFloat("IsWalking", walking);
    }

    public void UpdateChargingValue(float chargingAmount)
    {
        _playerAnim.SetFloat("ChargingAmount", chargingAmount);
    }

    public void DeathAnimation()
    {
        _playerAnim.SetInteger("Death", Random.Range(0,3));
    }

    public void IsDead()
    {
        _playerAnim.SetTrigger("IsDead");
    }

    public void Shoot()
    {
        _playerAnim.SetTrigger("Shoot");
    }

    public void Victory()
    {
        _playerAnim.SetTrigger("Win");
    }
}
