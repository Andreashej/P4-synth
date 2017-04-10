using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sendFreq : MonoBehaviour {
    float value;
    public string prefix;
    Slider slider;
    

    // Use this for initialization
    void Start () {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update () {
        //Arduino.NewDataEvent += newData;
        setValue();

    }

    public void setValue()
    {
        value = slider.value;
        tcpserver.PDSend(prefix + " " + value);
    }

    void newData(Arduino arduino) {
        switch(prefix) {
            case "/freq": 
                //slider.value = arduino.freq;
                break;
            case "/selector":
                //slider.value = arduino.selector;
                break;
            default:;
                break;
        }
    }
}
