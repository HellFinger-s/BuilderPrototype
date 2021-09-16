using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueControl : MonoBehaviour
{
    private int maximumCapacity = 150;

    public int returnCapacity() => maximumCapacity;

    public void IncreaseCapacity(int delta)
    {
        maximumCapacity += delta;
    }
}
