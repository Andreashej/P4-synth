using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;
using UnityEngine.UI;

public class tcpserver : MonoBehaviour {
    private static TcpClient client;
    private static Stream stream;
    private static System.Text.ASCIIEncoding asEn;
    int sine = 1;
    int sawtooth = 2;
    int triangle = 3;
    int pulse = 4;
    int square = 5;
    float[] freqs = new float[11];
    public float median;
    int count = 0;
    string lastMsg;

	// Use this for initialization
	void Start () {
        client = new TcpClient(); //Create new instance of TCP Client
        client.Connect("localhost", 17435); //Connect TCP client to port 17435
        stream = client.GetStream(); //get the TCP stream
        asEn = new System.Text.ASCIIEncoding(); //Init ASCII encoded string
        PDSend("/dsp 1"); //Turn DSP on in PD
        PDSend("/bit 4");
        Arduino.NewDataEvent += NewData;
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public static void PDSend(string msg) {
        string message = msg +";"; //Store message and add ; to end of line
        byte[] byteMsg = asEn.GetBytes(message); //Convert the message to a byte array to send
        stream.Write(byteMsg, 0, byteMsg.Length); //Write the byte array to the stream
    }

    private void OnApplicationQuit () {
        PDSend("/dsp 0"); //Turn DSP off at application quit
        client.Close(); //Close TCP client when application quit
    }

    void NewData(Arduino arduino) {
        string msg = "";
        if(arduino.flex > 750) {
            msg = "/selector ";
            if(arduino.RotX < 20) {
               msg += pulse.ToString();
            } else if (arduino.RotX > -20) {
                msg += triangle;
            } else if (arduino.RotY < 20) {
                msg += square;
            } else if (arduino.RotY > -20) {
                msg += sawtooth;
            } else {
                msg += sine;
            }
            
        } else {
            msg = "/selector 0";
        }

        float frequency = Mathf.Lerp (0, 800, Mathf.InverseLerp ((float)0, (float)150, arduino.freq));;
        
        if(frequency == (float)7.00 && count > 1) {
            freqs[count-1] = frequency;
            count++;
        } else {
            freqs[count] = frequency;
            count++;
        }
        
        if(count > freqs.Length-1) count = 0;
        
        median = freqs[5];

        if(msg != lastMsg) {
            PDSend(msg);
            lastMsg = msg;
        }

        msg = " /freq " + median;
        
        PDSend(msg);
    }
}
