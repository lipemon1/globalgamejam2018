using UnityEngine;
using System.Collections;

[System.Serializable]
public struct ShakePreset
{
    public float shakeAmount;
    public float shakeLenght;
}

public class ScreenShake : MonoBehaviour
{
    [Header("Debug")]
    public bool active;

    [Header("Components")]
    public static ScreenShake InstanceShake = null;
    public Camera mainCam;
    public Transform _elementToShake;

    [Header("Teste Shake")]
    [SerializeField] private bool _testOnSPACE;
    [SerializeField] private float _force;
    [SerializeField] private float _length;

    [Header("Shake Presets ")]
    [SerializeField] private ShakePreset _lowShake;
    [SerializeField] private ShakePreset _normalShake;
    [SerializeField] private ShakePreset _highShake;

    float shakeAmount = 0;
    Vector3 startPosition;


    void Awake()
    {
        startPosition = _elementToShake.transform.position;

        if (mainCam == null)
        {
            mainCam = Camera.main;
        }

        if (InstanceShake != null)
        {
            Destroy(InstanceShake);
        }
        else
        {
            InstanceShake = this;
        }
    }

    void Update()
    {
        //teste do screenshake maroto
        if (Input.GetKeyDown(KeyCode.Space) && _testOnSPACE)
        {
            if (active)
            {
                Shake(_force, _length);
            }
        }
    }

    /// <summary>
    /// /Aplica o Shake da Câmera
    /// </summary>
    /// <param name="amount">Proporção do "tremor"</param>
    /// <param name="length">Duração do shake</param>
    public void NormalShake()
    {
        Shake(_normalShake.shakeAmount, _normalShake.shakeLenght);
    }

    public void LowShake()
    {
        Shake(_lowShake.shakeAmount, _lowShake.shakeLenght);
    }

    public void HighShake()
    {
        Shake(_highShake.shakeAmount, _highShake.shakeLenght);
    }

    public void Shake(float amt, float length)
    {
        shakeAmount = amt;
        InvokeRepeating("BeginShake", 0, 0.01f);
        Invoke("StopShake", length);
    }

    void BeginShake()
    {
        if (shakeAmount > 0)
        {
            Vector3 camPos = _elementToShake.transform.position;

            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float offsetY = Random.value * shakeAmount * 2 - shakeAmount;

            camPos.x += offsetX;
            camPos.y += offsetY;

            _elementToShake.transform.position = camPos;
        }
    }

    void StopShake()
    {
        CancelInvoke("BeginShake");
        _elementToShake.transform.localPosition = startPosition; //STARTPOSITION TÁ BUGANDO A CÂMERA NO FINAL DO SCREENSHAKE
    }
}
