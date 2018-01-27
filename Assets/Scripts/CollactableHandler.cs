using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollactableHandler : MonoBehaviour {

    [Header("Debug")]
    [SerializeField] private Collider _itemToCollect;
    [SerializeField] private CollactableBehaviour _lastItemCollected;

    [Header("Energy Tag")]
    [SerializeField] private string _energyTag;

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
            CollactableBehaviour bulletCB = other.GetComponent<CollactableBehaviour>();

            if (bulletCB.CanBePicked() && _energyHandler.GetPlayerEnergyAmount() < GameLoop.MAX_ENERGY)
            {
                _itemToCollect = other;
                CollectEnergy(bulletCB);
            }
        }
    }

    private void CollectEnergy(CollactableBehaviour bulletCB)
    {
        Debug.Log("EnergyCollected");

        _itemToCollect = null;
        _lastItemCollected = bulletCB;

        _energyHandler.RecieveSomeEnergy(bulletCB.GetEnergyAmount());
        Destroy(bulletCB.gameObject);
    }
}
