using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollactableHandler : MonoBehaviour {

    [Header("Debug")]
    [SerializeField] private bool _canRecieveDamage;

    [Header("Energy Tag")]
    [SerializeField] private string _energyTag = "EnergyBullet";

    [Header("EnergyHandler")]
    [HideInInspector] private EnergyHandler _energyHandler;
    [HideInInspector] private Player _playerController;

    [Header("Player Animatino")]
    [SerializeField] private PlayerAnimController _playerAnimController;

    // Use this for initialization
    void Start () {
        _energyHandler = GetComponent<EnergyHandler>();
        _playerController = GetComponent<Player>();
        _playerAnimController = GetComponent<PlayerAnimController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(_energyTag))
        {
            BulletBehaviour bulletCB = other.GetComponent<BulletBehaviour>();

            if (bulletCB.CanBePicked() && _energyHandler.GetPlayerEnergyAmount() < GameLoop.MAX_ENERGY)
                CollectEnergy(bulletCB);
            else if (bulletCB.CanBePicked() == false)
                KillPlayer();
        }
    }

    private void CollectEnergy(BulletBehaviour bulletCB)
    {
        Debug.Log("EnergyCollected");
        _energyHandler.RecieveSomeEnergy(bulletCB.GetEnergyAmount());
        Destroy(bulletCB.gameObject);
    }

    private void KillPlayer()
    {
        _playerAnimController.Die();
        Global.StartKillingSomePlayer((int)_playerController.Index);
    }

    public void Respawn()
    {
        Global.RespawnSomePlayer((int)_playerController.Index);
    }
}
