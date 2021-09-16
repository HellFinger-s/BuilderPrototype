using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelBuilding : MonoBehaviour
{
    [SerializeField] private GameObject placeSelectObject;
    public void Cancel()// отменяем строительсто | build cancel
    {
        placeSelectObject.GetComponent<SelectPlace>().StopWork();
        placeSelectObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
