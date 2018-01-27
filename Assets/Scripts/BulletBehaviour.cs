using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BulletBehaviour : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField]
    private bool _canBePicked;

    public AnimationCurve Curve;
    private float _speed;
    private float _distance;

    public void Set(float speed, float distance)
    {
        _distance = distance;
        _speed = speed;
        StartCoroutine(Go(transform.forward, distance));
        Invoke("MakeItPickable", 4);
    }

    private void MakeItPickable()
    {
        GetComponent<CollactableBehaviour>().SetCanBePicked(true);
    }

    private IEnumerator Go(Vector3 direction, float distance)
    {
        float timer = 0;
        Vector3 initialPosition = transform.position;
        Vector3 finalPosition = transform.position + direction * distance;
        while (timer < Curve.keys.Last().time)
        {
            float value = Curve.Evaluate(timer);
            transform.position = Vector3.Lerp(initialPosition, finalPosition, value);
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
