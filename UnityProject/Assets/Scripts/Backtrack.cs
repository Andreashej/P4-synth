using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backtrack : MonoBehaviour
{
    public TextAsset[] backtrackChannels;
    NoteHelper[][] backtrackNotes;

    void Start()
    { 
        PlayBacktrack();
    }

    public void FillBacktrackChannels()
    {  

        backtrackNotes = new NoteHelper[backtrackChannels.Length][];

        for (int i = 0; i < backtrackNotes.Length; i++)
        {
            backtrackNotes[i] = RandomEnumSetter.MakeSongFromText(backtrackChannels[i]);
        }

    }

    public void PlayBacktrack()
    {
        FillBacktrackChannels();
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
