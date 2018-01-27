using UnityEngine;

public class NamePlateManager : MonoBehaviour
{
    public static NamePlateManager Instance { get; private set; }

    public UserPlate[] UserPlates;

    private void Awake()
    {
        Instance = this;
    }
}