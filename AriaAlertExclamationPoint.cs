using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AriaAlertExclamationPoint : MonoBehaviour
{

    public GameObject exclamationPoint;

    public void RevealExclamationPoint()
    {

        exclamationPoint.SetActive(true);

    }


    public void HideExclamationPoint()
    {

        exclamationPoint.SetActive(false);

    }
}
