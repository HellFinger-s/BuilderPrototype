using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageControl : MonoBehaviour
{
    [SerializeField] private ValueControl component;
    [SerializeField] private int increaseValue;
    void Start()
    {
        component.IncreaseCapacity(increaseValue);
    }
}
