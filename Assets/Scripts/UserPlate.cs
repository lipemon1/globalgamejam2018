using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class UserPlate : MonoBehaviour
{
    [Header("Setted in Runtime")]
    public Player Owner;

    public PlayerIndex Index;
    [SerializeField] private Text _energyCountText;

    public void SetEnergyCount(int value)
    {
        _energyCountText.text = value.ToString();
    }

    public void DisableNamePlate()
    {
        foreach (PlayerInfo t in Global.Player) if (!t.exist) gameObject.SetActive(false);
    }

    private void Update()
    {
        SetEnergyCount(Owner.GetComponent<EnergyHandler>().GetPlayerEnergyAmount());
    }

    public void EnableNamePlate()
    {
        gameObject.SetActive(true);
    }
}