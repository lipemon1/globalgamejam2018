using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerInfo
{
    public bool exist;
    public Transform transform;
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
    
    public const int MaxPlayers = 8;

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
}
