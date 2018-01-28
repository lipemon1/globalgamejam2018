using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollactableHandler : MonoBehaviour
{

    [Header("Debug")]
    [SerializeField]
    private bool _canRecieveDamage;

    [Header("Respawn Time")]
    [SerializeField] private float _timeForRespawn = 3f;

    [Header("Player Colliders")]
    [HideInInspector]
    private Collider _playerCollider;
    [HideInInspector] private CharacterController _charachterController;

    [Header("Energy Tag")]
    [SerializeField]
    private string _energyTag = "EnergyBullet";

    [Header("EnergyHandler")]
    [HideInInspector]
    private EnergyHandler _energyHandler;
    [HideInInspector] private Player _playerController;

    [Header("Player Animatino")]
    [SerializeField]
    private PlayerAnimController _playerAnimController;

    [Header("Particles")]
    [SerializeField]
    private ParticleSystem _deathParticle;
    [SerializeField] private ParticleSystem _respawnParticle;

    [SerializeField] private AcessorieChooser _acessories;

    // Use this for initialization
    void Start()
    {
        _energyHandler = GetComponent<EnergyHandler>();
        _playerController = GetComponent<Player>();
        _playerAnimController = GetComponent<PlayerAnimController>();
        _playerCollider = GetComponent<Collider>();
        _charachterController = GetComponent<CharacterController>();

        _respawnParticle.Clear();
        _respawnParticle.Play();
        _acessories.Pick();
    }

    // Update is called once per frame
    void Update()
    {

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

    public void KillPlayer()
    {
        Global.AddDeath((int)_playerController.Index);
        _playerCollider.enabled = false;
        _charachterController.enabled = false;

        _playerAnimController.Die();
        Global.StartKillingSomePlayer((int)_playerController.Index);
        Invoke("Respawn", _timeForRespawn);

        _deathParticle.Clear();
        _deathParticle.Play();
        SoundManager.Instance.PlaySomeAudio("Death");
    }

    public void Respawn()
    {
        Global.RespawnSomePlayer((int)_playerController.Index);
        _playerCollider.enabled = true;
        _charachterController.enabled = true;
        _respawnParticle.Clear();
        _respawnParticle.Play();
        _acessories.Pick();
    }
}
