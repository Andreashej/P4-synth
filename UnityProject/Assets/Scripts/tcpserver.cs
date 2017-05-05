using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;
using UnityEngine.UI;
using System;

public class tcpserver : MonoBehaviour
{
    private static TcpClient client;
    private static Stream stream;
    private static System.Text.ASCIIEncoding asEn;
    int sine = 1;
    int sawtooth = 2;
    int triangle = 3;
    int pulse = 4;
    int square = 5;
    float[] freqs = new float[11];
    private float median;
    int count = 0;
    string lastMsg;
    int selector = 0;
    int lastSelector = 0;
    string selectorMsg;
    string freqMsg;
    public string msg; 

    public float Low = 50;
    public float High = 100;


    // Use this for initialization
    void Start()
    {
        client = new TcpClient(); //Create new instance of TCP Client
        client.Connect("localhost", 17435); //Connect TCP client to port 17435
        stream = client.GetStream(); //get the TCP stream
        asEn = new System.Text.ASCIIEncoding(); //Init ASCII encoded string
        PDSend("/dsp 1"); //Turn DSP on in PD
        Arduino.NewDataEvent += NewData;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void PDSend(string msg)
    {
        string message = msg + ";"; //Store message and add ; to end of line
        byte[] byteMsg = asEn.GetBytes(message); //Convert the message to a byte array to send
        stream.Write(byteMsg, 0, byteMsg.Length); //Write the byte array to the stream
    }

    private void OnApplicationQuit()
    {
        PDSend("/dsp 0"); //Turn DSP off at application quit
        client.Close(); //Close TCP client when application quit
    }

    void NewData(Arduino arduino)
    {
        string selectorMsg = "+/selector-";

        if (arduino.flex > 720)
        {
            if (arduino.RotX > 15)
            {
                selector = pulse;
            }
            else if (arduino.RotX < -20)
            {
                selector = triangle;
            }
            else if (arduino.RotY > 20)
            {
                selector = square;
            }
            else if (arduino.RotY < -20)
            {
                selector = sawtooth;
            }
            else
            {
                selector = sine;
            }
            selectorMsg += selector;

        }
        else
        {
            selector = 0;
            selectorMsg += selector;
        }

        float frequency = arduino.freq;

        if (frequency == 7f)
        {
            if (count > 1)
                frequency = freqs[count - 1];
            else
                frequency = freqs[10];
        }

        freqs[count] = frequency;
        count++;

        if (count > freqs.Length - 1) count = 0;

        float[] freqsort = new float[11];
        freqs.CopyTo(freqsort, 0);
        Array.Sort(freqsort);
        median = freqsort[5];

        float freqMedian = Mathf.Lerp(RandomEnumSetter.CalculateFrequency("F#", 1), RandomEnumSetter.CalculateFrequency("C", 3), Mathf.InverseLerp(Low, High, median));
        Debug.Log("Median: " + median + " Freq: " + freqMedian);
        /*
        Text freq = GameObject.FindWithTag("freq").GetComponent<Text>();
        freq.text = median + " Hz";

        string[] waves = { "Off", "Sine", "Sawtooth", "Triangle", "Pulse", "Square" };
        Text wave = GameObject.FindWithTag("wave").GetComponent<Text>();
        wave.text = waves[int.Parse(msg.Remove(0, "/selector ".Length))];
        */
        freqMsg += "+/freq-" + freqMedian;

        msg = "/ch1" + freqMsg + selectorMsg;

        if (msg != lastMsg)
        {
            FindObjectOfType<BacktrackHandler>().ch1Msg = msg;
            lastMsg = msg;
        }

        freqMsg = "";
        selectorMsg = "";


    }

    public float GetMedian()
    {
        return Mathf.Lerp(RandomEnumSetter.CalculateGMajorPosition("F#", 1), RandomEnumSetter.CalculateGMajorPosition("C", 3), Mathf.InverseLerp(Low, High, median));
    }
}
