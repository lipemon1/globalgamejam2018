using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnner : MonoBehaviour
{
    private GameObject lastSpawn;

    public GameObject BulletPrefab;
    public float Velocity;

    public float MaxTime;
    public float _timer;

    public float MaxDistance;
    public float MinDistance;

    private float _distance = 0;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _timer = 0;
        }
        else if (Input.GetMouseButton(0))
        {
            _timer += Time.deltaTime;
            _distance = Mathf.Lerp(MinDistance, MaxDistance, Mathf.InverseLerp(0, MaxTime, _timer));
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (lastSpawn != null)
                Destroy(lastSpawn);

            _timer += Time.deltaTime;
            _distance = Mathf.Lerp(MinDistance, MaxDistance, Mathf.InverseLerp(0, MaxTime, _timer));

            GameObject bullet = Instantiate(BulletPrefab, transform.position, transform.rotation);
            BulletBehaviour bulletBehaviour = bullet.GetComponent<BulletBehaviour>();
            float drag = (Mathf.Pow(Velocity, 2)) / 2 / _distance;
            bulletBehaviour.Set(Velocity, drag);

            lastSpawn = bullet;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, MaxDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, MinDistance);

        Vector3 finalPosition = transform.position + transform.forward * _distance;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(finalPosition, 1);
    }
}
