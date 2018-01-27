﻿using System.Collections;
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

    public static void StartKillingSomePlayer(GameObject playerObjectInstance)
    {
        PlayerInfo playerToKill = GetMyPlayer(playerObjectInstance);

        playerToKill.Instance.DisablePlayerControl();

        GameLoop.Instance.RemoveTicket();
        //TO DO Call death animation here
    }

    public static void RespawnSomePlayer(GameObject playerObjectInstance)
    {
        PlayerInfo playerToRespawn = GetMyPlayer(playerObjectInstance);

        playerToRespawn.Instance.gameObject.GetComponent<EnergyHandler>().RecieveSomeEnergy(GameLoop.INITIAL_ENERGY);

        playerToRespawn.Instance.gameObject.transform.position = playerToRespawn.PositionToSpawn.position;
        playerToRespawn.Instance.gameObject.transform.rotation = playerToRespawn.PositionToSpawn.rotation;

        playerToRespawn.Instance.EnablePlayerControl();
        playerToRespawn.Instance.ResetPlayer();
        playerToRespawn.PlayerEnergy.ToogleShield(false);
        playerToRespawn.PlayerEnergy.SetHoldingShot(false, 0f);
    }

    /// <summary>
    /// Retorna qual jogador dentro da lista de jogadores eu sou
    /// </summary>
    /// <param name="playerObjectInstance"></param>
    /// <returns></returns>
    public static PlayerInfo GetMyPlayer(GameObject playerObjectInstance)
    {
        return Player.Where(player => player.Instance.gameObject == playerObjectInstance).ToList().FirstOrDefault();
    }
}
