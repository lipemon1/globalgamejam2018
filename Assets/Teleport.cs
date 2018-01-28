using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

    public Vector3 teleport;
    public bool draw;

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
            col.transform.Translate(teleport, Space.World);
        //col.transform.position = teleport;
    }

    private void OnDrawGizmos()
    {
        if (draw)
            Gizmos.DrawWireCube(transform.position + teleport, transform.localScale);
    }
}
