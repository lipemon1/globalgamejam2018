using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class UserPlate : MonoBehaviour
{
    [Header("Setted in Runtime")]
    public Player Owner;

    public PlayerIndex Index;
    [SerializeField] private Image _fillImage;

    public void SetFill(float value)
    {
        _fillImage.fillAmount = value;
    }

    public void DisableNamePlate()
    {
        foreach (PlayerInfo t in Global.Player) if (!t.exist) gameObject.SetActive(false);
    }

    private void Update()
    {
        SetFill((float)Owner.GetComponent<EnergyHandler>().GetPlayerEnergyAmount() / GameLoop.MAX_ENERGY);
    }
}