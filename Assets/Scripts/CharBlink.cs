using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharBlink : MonoBehaviour
{
    public Sprite Normal;
    public Sprite Blink;
    public float BlinkDuration = .2f;

    public Image OutImage;

    [Range(0, 100)] public int BlinkProbabilityPerSecond = 35;

    private bool _isBlinking;

    private void Start()
    {
        StartCoroutine(IEBlinkLoop());
    }

    private IEnumerator IEBlinkLoop()
    {
        yield return new WaitForSeconds(Random.Range(0f, 1f));  
        for (; ; )
        {
            if (!_isBlinking)
            {
                if (Random.Range(0f, 100f) <= BlinkProbabilityPerSecond)
                {
                    _isBlinking = true;
                    StartCoroutine(IEBlink());
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator IEBlink()
    {
        OutImage.sprite = Blink;
        yield return new WaitForSeconds(BlinkDuration);
        OutImage.sprite = Normal;
        yield return new WaitForSeconds(1);
        _isBlinking = false;
    }
}