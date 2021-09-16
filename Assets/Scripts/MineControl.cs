using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MineControl : MonoBehaviour
{
    private float timer = 0f;
    [SerializeField] private float cicleTime;// время производства | production time

    [SerializeField] private int productivity;// количество продукта за цикл | product quantity per cicle

    [SerializeField] private Text value;// 

    [SerializeField] private Outline outline;

    private ValueControl component;

    private bool isReady = false;// готово к сбору | ready to collect

    private void Start()
    {
        component = value.gameObject.GetComponent<ValueControl>();
    }
    // Update is called once per frame
    void Update()
    {
        if(timer < cicleTime)
        {
            timer += Time.deltaTime;
        }
        else// цикл закончен | cicle is over
        {
            isReady = true;
            outline.enabled = true;
        }
    }

    public bool GetStatus() => isReady;

    public Text GetValue() => value;

    public int GetProductivity() => productivity;

    public ValueControl GetValueControl() => component;

    public void Collect()
    {
        isReady = false;
        outline.enabled = false;
        value.text = (Convert.ToInt32(value.text) + productivity).ToString();
        timer = 0f;
    }
}
