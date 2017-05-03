using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BacktrackHandler : MonoBehaviour
{

    public Component[] backtrackChannels;
    float beatsPerMinute;
    float timeBetweenSpawnsInSeconds;
    float speed;
    float nextSpawnTime;
    string msg = "helo";

    // Use this for initialization
    void Start()
    {
        backtrackChannels = GetComponentsInChildren<Transform>();
        beatsPerMinute = FindObjectOfType<Spawner>().beatsPerMinute;
        timeBetweenSpawnsInSeconds = 60f / beatsPerMinute;
        speed = FindObjectOfType<Spawner>().spaceBetweenNotes * 0.64f / timeBetweenSpawnsInSeconds;
    }

    // Update is called once per frame
    void Update()
    {
        msg += backtrackChannels[0].GetComponent<Backtrack>().message + " ";
		msg += backtrackChannels[1].GetComponent<Backtrack>().message + " ";
		msg += backtrackChannels[2].GetComponent<Backtrack>().message + " ";
		msg += backtrackChannels[3].GetComponent<Backtrack>().message;

        if (Time.time > nextSpawnTime)
        {
			nextSpawnTime = Time.time + timeBetweenSpawnsInSeconds;
			tcpserver.PDSend(msg);
        }
    }
}
