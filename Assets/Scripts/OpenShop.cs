using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenShop : MonoBehaviour
{
    [SerializeField] private GameObject shopPanel;

    public void ShopEnableDisable() => shopPanel.SetActive(!shopPanel.activeInHierarchy);
}
