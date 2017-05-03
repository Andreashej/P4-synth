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

    void Start()
    {
        beatsPerMinute = FindObjectOfType<Spawner>().beatsPerMinute;
        timeBetweenSpawnsInSeconds = 60f / beatsPerMinute;
        speed = FindObjectOfType<Spawner>().spaceBetweenNotes * 0.64f / timeBetweenSpawnsInSeconds;
        backtrackNotes = RandomEnumSetter.MakeSongFromText(backtrackChannel);
        currentNote = 0;
    }


    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            if (currentNote < backtrackNotes.Length)
            {
                PlayStop();
                int noteLength = backtrackNotes[currentNote].length;
                if (backtrackNotes[currentNote].pitch == "Break")
                {
                    nextSpawnTime = Time.time + timeBetweenSpawnsInSeconds + (0.64f * (noteLength - 1) / speed);
                }
                else
                {
                    if (noteLength == 1)
                    {
                        nextSpawnTime = Time.time + timeBetweenSpawnsInSeconds;
                        PlayBacktrack(currentNote);
                    }
                    else
                    {
                        nextSpawnTime = Time.time + timeBetweenSpawnsInSeconds + (0.64f * (noteLength - 1) / speed);
                        PlayBacktrack(currentNote);

                    }
                }
                currentNote++;
            }
        }
    }

    public void PlayStop(){
        string hello = "/ch" + channel + " /selector 0";
        tcpserver.PDSend(hello);
    }

    public void PlayBacktrack(int note)
    {
        float freq = RandomEnumSetter.CalculateFrequency(backtrackNotes[note].pitch, backtrackNotes[note].octave);
        int waveform = backtrackNotes[note].waveform;
        int length = backtrackNotes[note].length;

        string hello = "/ch" + channel.ToString() + " /freq " + freq.ToString(); 
        tcpserver.PDSend(hello);
        hello = "/ch" + channel.ToString() + " /selector " + waveform.ToString();
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