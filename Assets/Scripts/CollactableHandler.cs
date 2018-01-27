using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollactableHandler : MonoBehaviour {

    [Header("Energy Tag")]
    [SerializeField] private string _energyTag = "EnergyBullet";

    [Header("EnergyHandler")]
    [HideInInspector] private EnergyHandler _energyHandler;

    // Use this for initialization
    void Start () {
        _energyHandler = GetComponent<EnergyHandler>();
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
        Global.StartKillingSomePlayer(this.gameObject);
        Invoke("Respawn", 2f);
    }

    private void Respawn()
    {
        Global.RespawnSomePlayer(this.gameObject);
    }
}
