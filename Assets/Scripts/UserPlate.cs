using UnityEngine;
using UnityEngine.UI;

public class UserPlate : MonoBehaviour
{
    [SerializeField] private Image _fillImage;

    public void SetFill(float value) { _fillImage.fillAmount = value; }
}