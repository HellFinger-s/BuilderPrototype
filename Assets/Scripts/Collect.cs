using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Collect : MonoBehaviour
{
    private MineControl component;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Mine"))// рэйкаст попал в шахту | raycast got into the mine
                {
                    component = hit.collider.gameObject.GetComponent<MineControl>();
                    // если шахта готова к сборке и есть место куда собирать | if mine is ready and we have enough space to collect
                    if (component.GetStatus() && (Convert.ToInt32(component.GetValue().text) + component.GetProductivity()) <= component.GetValueControl().returnCapacity())
                    {
                        component.Collect();
                    }
                }
            }
        }
    }
}
