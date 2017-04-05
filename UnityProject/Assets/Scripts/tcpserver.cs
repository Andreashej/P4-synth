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
        client = new TcpClient();
        client.Connect("localhost", 17435);
        stream = client.GetStream();
        asEn = new System.Text.ASCIIEncoding();
        PDSend("/dsp 1");
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public static void PDSend(string msg) {
        string message = msg +";";
        byte[] byteMsg = asEn.GetBytes(message);
        stream.Write(byteMsg, 0, byteMsg.Length);
    }

    private void OnApplicationQuit () {
        PDSend("/dsp 0");
        client.Close();  
    }
}
