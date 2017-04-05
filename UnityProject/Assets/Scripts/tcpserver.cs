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

	// Use this for initialization
	void Start () {
        client = new TcpClient(); //Create new instance of TCP Client
        client.Connect("localhost", 17435); //Connect TCP client to port 17435
        stream = client.GetStream(); //get the TCP stream
        asEn = new System.Text.ASCIIEncoding(); //Init ASCII encoded string
        PDSend("/dsp 1"); //Turn DSP on in PD
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
}
