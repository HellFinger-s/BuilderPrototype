using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BuildStructure : MonoBehaviour
{
    [SerializeField] private string structureType; //тип строения 

    [SerializeField] private int structureCost;//стоимость строения

    [SerializeField] private Text gasValue;//ссылка на текст с количеством газа | link on text with gas value
    [SerializeField] private Text mineralsValue;//ссылка на текст с количеством минералов | linl on text with minerals value

    [SerializeField] private bool isGasPrice;

    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject placeSelectObject;
    [SerializeField] private GameObject cancelBuilding;// кнопка с отменой строительства | button with building cancel

    private SelectPlace placeSelectComponent;

    private void Start()
    {
        placeSelectComponent = placeSelectObject.GetComponent<SelectPlace>();
    }


    public void Buy()
    {
        if (!placeSelectComponent.GetBuildingStatus())// если сейчас ничего не строится | if nothing is building now
        {
            if (isGasPrice)
            {
                if (Convert.ToInt32(gasValue.text) >= structureCost)// у нас хватает денег | we have enough money
                {
                    placeSelectObject.SetActive(true);
                    cancelBuilding.SetActive(true);
                    placeSelectObject.GetComponent<SelectPlace>().SetStructureTypeAndCost(structureType, structureCost);// следующая итерация строительства | next iteration of building
                    shopPanel.SetActive(false);
                }
            }
            else
            {
                if (Convert.ToInt32(mineralsValue.text) >= structureCost)
                {
                    placeSelectObject.SetActive(true);
                    cancelBuilding.SetActive(true);
                    placeSelectObject.GetComponent<SelectPlace>().SetStructureTypeAndCost(structureType, structureCost);// следующая итерация строительства | next iteration of building
                    shopPanel.SetActive(false);
                }
            }
        }
    }

}
