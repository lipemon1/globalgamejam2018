using System.Collections;
using UnityEngine;

public class Blinker : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField]
    private bool _isBlinking;

    [Header("Object to Blink")]
    [SerializeField]
    private Renderer[] _target;

    [Header("Blink Settings")]
    public Color BlinkColor = Color.yellow;
    [SerializeField] [Tooltip("Animation Curve need to be in Ping Pong mode")]
    private AnimationCurve _blinkCurve;
    [SerializeField] private float _blinkSpeed = 1;
    [SerializeField] private float _blinkDuration = 1;


    //public void StartBlink(System.Action actionAfterBlink)
    //{
    //    StartCoroutine(BlinkBehaviour(_blinkTimes, _delayBetweenBlinks, actionAfterBlink));
    //}

    private void Update()
    {
        if (_isBlinking)
        {
            var value = _blinkCurve.Evaluate(Time.time * _blinkSpeed);
            Color color = BlinkColor * value;
            foreach (var rend in _target) rend.material.SetColor("_EmissionColor", color);
        }
    }

    public void StartBlink(System.Action actionAfterBlink)
    {
        _isBlinking = true;
        Invoke("StopBlinking", _blinkDuration);
    }

    private void StopBlinking()
    {
        _isBlinking = false;
        foreach (var rend in _target) rend.material.SetColor("_EmissionColor", Color.black);
    }

    //IEnumerator BlinkBehaviour(float blinkTimes, float delay, System.Action actionAfterBlink)
    //{
    //    _isBlinking = true;

    //    while (blinkTimes > 0)
    //    {
    //        _target.SetActive(!_target.activeInHierarchy);
    //        blinkTimes--;
    //        yield return new WaitForSeconds(delay);
    //    }

    //    _isBlinking = false;
    //    actionAfterBlink();
    //}
}
