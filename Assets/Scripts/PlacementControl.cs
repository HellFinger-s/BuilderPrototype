using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementControl : MonoBehaviour
{
    [SerializeField] private bool isFree = true;

    public void Occupied()
    {
        isFree = false;
    }

    public bool GetStatus()
    {
        return isFree;
    }
}
