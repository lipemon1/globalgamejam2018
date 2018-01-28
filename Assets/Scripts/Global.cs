using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class PlayerInfo
{
    public bool exist;
    public Transform transform;
    public Transform PositionToSpawn;
    public Player Instance;
    public EnergyHandler PlayerEnergy;
    public Blinker Blinker;
    public PlayerAnimController PlayerAnimationsController;
    public int kills;
    public int deaths;

    public void ResetInfo()
    {
        exist = false;
        transform = null;
        Instance = null;
        kills = 0;
        deaths = 0;
    }

    public PlayerInfo()
    {
        ResetInfo();
    }
}

public static class Global {
    
    public const int MaxPlayers = 4;

    public static List<PlayerInfo> Player = new List<PlayerInfo>();

    public static void ResetAllPlayers()
    {
        if(Player.Count == 0)
        {
            for (int i = 0; i < MaxPlayers; i++)
                Player.Add(new PlayerInfo());
            return;
        }
        foreach(PlayerInfo _player in Player)
        {
            _player.ResetInfo();
        }
    }

    public static void StartKillingSomePlayer(int playerToStartKilling)
    {
        PlayerInfo playerToKill = GetMyPlayer(playerToStartKilling);

        playerToKill.Instance.DisablePlayerControl();

        GameLoop.Instance.RemoveTicket();
        //TO DO Call death animation here
    }

    public static void RespawnSomePlayer(int playerToRespawnNow)
    {
        PlayerInfo playerToRespawn = GetMyPlayer(playerToRespawnNow);

        if(GameLoop.Instance.RespawnWithEnergy)
            playerToRespawn.Instance.gameObject.GetComponent<EnergyHandler>().RecieveSomeEnergy(GameLoop.INITIAL_ENERGY);

        playerToRespawn.Instance.gameObject.transform.position = playerToRespawn.PositionToSpawn.position;
        playerToRespawn.Instance.gameObject.transform.rotation = playerToRespawn.PositionToSpawn.rotation;

        playerToRespawn.Instance.EnablePlayerControl();
        playerToRespawn.Instance.ResetPlayer();
        playerToRespawn.PlayerEnergy.ToogleShield(false);
        playerToRespawn.PlayerEnergy.SetHoldingShot(false, 0f);

        playerToRespawn.PlayerAnimationsController.Respawn();

        playerToRespawn.Blinker.StartBlink(() => playerToRespawn.PlayerEnergy.SetCanRecieveDamage(true));

        SoundManager.Instance.PlaySomeAudio("Respawn");
    }

    public static void AddKill(int playerIndex)
    {
        GetMyPlayer(playerIndex).kills++;
    }

    public static void AddDeath(int playerIndex)
    {
        GetMyPlayer(playerIndex).deaths++;
    }

    private static PlayerInfo GetMyPlayer(int playerIndex)
    {
        return Player[playerIndex];
    }
}
