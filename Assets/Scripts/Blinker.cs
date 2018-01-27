using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinker : MonoBehaviour {

    [Header("Debug")]
    [SerializeField] private bool _isBlinking;

    [Header("Object to Blink")]
    [SerializeField] private GameObject _target;
    [Range(1, 10)]
    [SerializeField] private float _blinkTimes = 5;
    [SerializeField] private float _delayBetweenBlinks;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartBlink(System.Action actionAfterBlink)
    {
        StartCoroutine(BlinkBehaviour(_blinkTimes, _delayBetweenBlinks, actionAfterBlink));
    }

    IEnumerator BlinkBehaviour(float blinkTimes, float delay, System.Action actionAfterBlink)
    {
        _isBlinking = true;

        while(blinkTimes > 0)
        {
            _target.SetActive(!_target.activeInHierarchy);
            blinkTimes--;
            yield return new WaitForSeconds(delay);
        }

        _isBlinking = false;
        actionAfterBlink();
    }
}
