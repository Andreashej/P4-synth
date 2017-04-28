using System;
using UnityEngine;
using System.Collections;
using System.IO;


public class ArduinoLogger : MonoBehaviour
{
    public string FileName = "Arduino Log";
    public string FileFormat = ".tsv";
    public string path = "";
    public string header = "UnityMillis\tArduinoMillis\tEDA\tIBI\tDistance";

    private StreamWriter fileWriter;
    

	// Use this for initialization
	void Start ()
	{
        if(path == "")
            path = Application.dataPath + "/logs/";

        FileName += string.Format(" {0:HH mm ss yyyy-MM-dd}", DateTime.Now) + FileFormat; 
        fileWriter = new StreamWriter(path + FileName);
	    fileWriter.WriteLine(header);

	    Arduino.NewDataEvent += NewData;
	}

    void NewData(Arduino arduino)
    {
        fileWriter.Write((uint)(1000 * Time.realtimeSinceStartup) + "\t" + arduino.NewestIncomingData);
        //fileWriter.Write(arduino.NewestIncomingData);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDisable()
    {
        fileWriter.Flush();
        fileWriter.Close();
        
    }
}
