using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcessorieChooser : MonoBehaviour {

	public void Pick(int index)
    {
        DisableAllAcessories();

        if (index < transform.childCount)
        {
            transform.GetChild(index).gameObject.SetActive(true);
            return;
        }
        Pick();
    }

    public void Pick()
    {
        DisableAllAcessories();

        transform.GetChild(Random.Range(0,transform.childCount)).gameObject.SetActive(true);
    }

    private void DisableAllAcessories()
    {
        foreach (Transform childiznho in transform)
        {
            childiznho.gameObject.SetActive(false);
        }
    }
}
