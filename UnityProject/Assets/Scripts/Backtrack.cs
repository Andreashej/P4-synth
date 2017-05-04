using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backtrack : MonoBehaviour
{
    public TextAsset backtrackChannel;
    NoteHelper[] backtrackNotes;
    public int channel;
    float beatsPerMinute;
    float timeBetweenSpawnsInSeconds;
    float nextSpawnTime;
    float speed;
    int currentNote;
    public bool turnOFF;
    int whereWeAre = 1;
    bool sent = false;
    public string message = "";

    void Start()
    {
        message = SendStop();
        beatsPerMinute = FindObjectOfType<Spawner>().beatsPerMinute;
        timeBetweenSpawnsInSeconds = 60f / beatsPerMinute;
        speed = FindObjectOfType<Spawner>().spaceBetweenNotes * 0.64f / timeBetweenSpawnsInSeconds;
        backtrackNotes = RandomEnumSetter.MakeSongFromText(backtrackChannel);
        currentNote = 0;
    }


    void Update()
    {
        if (!turnOFF)
        {
            
            if (Time.time > nextSpawnTime)
            {
                if (currentNote < backtrackNotes.Length)
                {
                    int noteLength = backtrackNotes[currentNote].length;
                    
                    
                    if (backtrackNotes[currentNote].pitch == "Break")
                    {
                        nextSpawnTime = Time.time + timeBetweenSpawnsInSeconds;// + (0.64f * (noteLength - 1) / speed);
                        if (!sent)
                        {
                            message = SendStop();
                            sent = true;
                        }
                        
                    }
                    else
                    {
                        nextSpawnTime = Time.time + timeBetweenSpawnsInSeconds;
                        if (!sent)
                        {
                            message = SendBacktrack(currentNote);
                            sent = true;
                        }
                        

                        /*if (noteLength == 1)
                        {
                            nextSpawnTime = Time.time + timeBetweenSpawnsInSeconds;
                            SendBacktrack(currentNote);
                        }
                        else
                        {
                            nextSpawnTime = Time.time + timeBetweenSpawnsInSeconds + (0.64f * (noteLength - 1) / speed);
                            SendBacktrack(currentNote);

                        }*/
                    }
                   
                    if (whereWeAre == noteLength)
                    {
                        
                        currentNote++;
                        //message = SendStop();
                        whereWeAre = 1;
                        sent = false;
                    }
                    else whereWeAre++;

                }
                else message = SendStop();
            }
        }
    }

    public string SendStop()
    {
        string msg = "/selector-0";
        return msg;
    }

    public void PlayStop()
    {
        string hello = "/ch" + channel + "+/selector-0";
        tcpserver.PDSend(hello);
    }

    public string SendBacktrack(int note)
    {
        float freq = RandomEnumSetter.CalculateFrequency(backtrackNotes[note].pitch, backtrackNotes[note].octave);
        int waveform = backtrackNotes[note].waveform;
        int length = backtrackNotes[note].length;

        string msg = "/freq-" + freq.ToString() + "+/selector-" + waveform.ToString();
        return msg;
    }

    public void PlayBacktrack(int note)
    {
        float freq = RandomEnumSetter.CalculateFrequency(backtrackNotes[note].pitch, backtrackNotes[note].octave);
        int waveform = backtrackNotes[note].waveform;
        int length = backtrackNotes[note].length;

        string hello = "/ch" + channel.ToString() + "+/freq-" + freq.ToString();
        tcpserver.PDSend(hello);
        hello = "/ch" + channel.ToString() + "+/selector-" + waveform.ToString();
        tcpserver.PDSend(hello);


        /*
			this is where you play the channels, backtrackNotes[0-3]
			backtrackNotes[i][j].<name> are the i channel's j note's pitch, octave, waveform and length
			pitch is a string: A, A#, B, C, etc.. have to be converted to float. "Break" is handled in the conversion, as it returns 0, but could be handled here too.
			octave is an int, ranges from -1 to 3. It's used in the frequency calculation.
			waveform is an int, ranges from 0-4, Sine, Triangle, Square, Sawtooth, Pulse, this can be changed.
			length is an int, in 1/16s.
		*/
    }

}