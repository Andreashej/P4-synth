using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backtrack : MonoBehaviour
{
    public TextAsset backtrackChannel;
    NoteHelper[] backtrackNotes;
    public int channel;
    float nextSpawnTime;
    float speed;
    int currentNote;
    public bool turnOFF;
    int whereWeAre = 1;
    bool sent = false;
    public string message = "";
    public bool gameOn;
    float delay;
    int lastWaveForm;
    void Start()
    {
        message = SendStop();
        gameOn = FindObjectOfType<Spawner>().gameOn;
        backtrackNotes = RandomEnumSetter.MakeSongFromText(backtrackChannel);
        currentNote = 0;
        speed = FindObjectOfType<Spawner>().speed;
        delay = FindObjectOfType<Spawner>().delay;
    }


    void Update()
    {
        gameOn = FindObjectOfType<Spawner>().gameOn;
        float currTime = FindObjectOfType<Spawner>().currTime;
        if (gameOn)
        {
            if (!turnOFF)
            {
                if (Time.time > delay + currTime)
                {
                    if (Time.time > nextSpawnTime)
                    {
                        if (currentNote < backtrackNotes.Length)
                        {
                            int noteLength = backtrackNotes[currentNote].length;

                            if (backtrackNotes[currentNote].pitch == "Break")
                            {

                                if (!sent)
                                {
                                    nextSpawnTime = Time.time + (0.64f * noteLength / speed);
                                    message = SendStop();
                                    sent = true;
                                }
                            }
                            else
                            {
                                if (!sent)
                                {
                                    nextSpawnTime = Time.time + (0.64f * noteLength / speed);
                                    message = SendBacktrack(currentNote);
                                    sent = true;
                                }
                            }

                            if (whereWeAre == noteLength)
                            {

                                currentNote++;
                                whereWeAre = 1;
                                sent = false;
                            }
                            else whereWeAre++;

                        }
                        else message = SendStop();
                    }
                }
            }
        }
    }

    public string SendStop()
    {
        string msg = "/selector-0";
        return msg;
    }

    public string SendBacktrack(int note)
    {
        float freq = RandomEnumSetter.CalculateFrequency(backtrackNotes[note].pitch, backtrackNotes[note].octave);
        int waveform = backtrackNotes[note].waveform;
        int length = backtrackNotes[note].length;
        string msg;
        if (waveform != lastWaveForm) msg = "/freq-" + freq.ToString() + "+/selector-" + waveform.ToString();
        else msg = "/freq-" + freq.ToString();
        return msg;
    }

}