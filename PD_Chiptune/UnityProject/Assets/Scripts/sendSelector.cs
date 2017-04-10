using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sendSelector : MonoBehaviour {
    public float frequency;
    Slider slider;

    // Use this for initialization
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        //Arduino.NewDataEvent += newData;

        tcpserver.PDSend("/selector " + slider.value);
    }

    void newData(Arduino arduino)
    {
        slider.value = arduino.freq;
    }
}
