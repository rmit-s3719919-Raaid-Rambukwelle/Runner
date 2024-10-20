using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleObjects : MonoBehaviour
{
    public GameObject uiElement;


    private void OnMouseEnter()
    {
        if (Vector3.Distance(PlayerManager.current.transform.position, transform.position) <= 125f)
            uiElement.SetActive(true);
    }

    private void OnMouseExit()
    {
        uiElement.SetActive(false);
    }

}
