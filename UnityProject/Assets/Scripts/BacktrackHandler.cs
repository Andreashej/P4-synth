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

    // Use this for initialization
    void Start()
    {
        backtrackChannels = GetComponentsInChildren<Backtrack>();
        beatsPerMinute = FindObjectOfType<Spawner>().beatsPerMinute;
        speed = FindObjectOfType<Spawner>().spaceBetweenNotes * 0.64f / timeBetweenSpawnsInSeconds;
        player = FindObjectOfType<Player>();
        timeBetweenSpawnsInSeconds = 60f / beatsPerMinute + 0.64f / speed;
        screenHalfSizeInWorldUnits = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
        float distance = screenHalfSizeInWorldUnits.x + 0.32f - player.transform.position.x;
        delay = 0;
        Debug.Log(distance);
        Debug.Log(delay);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > delay)
        {
            msg = player.msg + " ";
            //if (Time.time > nextSpawnTime)
            //{
            for (int i = 0; i < backtrackChannels.Length; i++)
            {
                msg += "/ch" + backtrackChannels[i].GetComponent<Backtrack>().channel + "+" + backtrackChannels[i].GetComponent<Backtrack>().message + " ";
            }
            //nextSpawnTime = Time.time + timeBetweenSpawnsInSeconds/2f;
            tcpserver.PDSend(msg);
            msg = "";
            //}
        }
    }
}
