using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BacktrackHandler : MonoBehaviour
{

    public Component[] backtrackChannels;
    Player player;
    float beatsPerMinute;
    float timeBetweenSpawnsInSeconds;
    float speed;
    float nextSpawnTime;
    string msg = "";
    Vector2 screenHalfSizeInWorldUnits;
    float delay;
    public string ch1Msg;
    bool gameOn;

    // Use this for initialization
    void Start()
    {
        backtrackChannels = GetComponentsInChildren<Backtrack>();
        beatsPerMinute = FindObjectOfType<Spawner>().beatsPerMinute;
        speed = FindObjectOfType<Spawner>().spaceBetweenNotes * 0.64f / timeBetweenSpawnsInSeconds;
        gameOn = FindObjectOfType<Spawner>().gameOn;
        player = FindObjectOfType<Player>();
        timeBetweenSpawnsInSeconds = 60f / beatsPerMinute + 0.64f / speed;
        screenHalfSizeInWorldUnits = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
        float distance = screenHalfSizeInWorldUnits.x + 0.32f - player.transform.position.x;
        delay = 0;
        tcpserver.PDSend("/ch1+/selector-0 /ch2+/selector-0 /ch3+/selector-0 /ch4+/selector-0 /ch5+/selector-0"); //so the game doesn't play music in the beginning
        ch1Msg = "";
    }

    // Update is called once per frame
    void Update()
    {
        gameOn = FindObjectOfType<Spawner>().gameOn;
        if (gameOn)
        {
            if (Time.time > delay)
            {
                SendMessage();
            }
        }
    }

    void SendMessage()
    {
        msg = ch1Msg + " ";
        for (int i = 0; i < backtrackChannels.Length; i++)
        {
            msg += "/ch" + backtrackChannels[i].GetComponent<Backtrack>().channel + "+" + backtrackChannels[i].GetComponent<Backtrack>().message + " ";
        }
        tcpserver.PDSend(msg);
        msg = "";
    }
}
