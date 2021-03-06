﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public enum Waveform { None, Sine, Sawtooth, Triangle, Pulse, Square };

public static class RandomEnumSetter
{
    public static string[] colors = { "white", "blue", "red", "orange", "green", "pink" };
    public static Dictionary<string, int> noteFreqAndPos = new Dictionary<string, int>()
    {
        {"A", 0},
        {"A#", 1},
        {"B", 2},
        {"C", 3},
        {"C#", 4},
        {"D", 5},
        {"D#", 6},
        {"E", 7},
        {"F", 8},
        {"F#", 9},
        {"G", 10},
        {"G#", 11},
    };

    public static Dictionary<string, int> GMajorPos = new Dictionary<string, int>()
    {
        {"A", 0},
        {"B", 1},
        {"C", 2},
        {"D", 3},
        {"E", 4},
        {"F#", 5},
        {"G", 6},
    };

     public static Dictionary<int, string> GMajorPosInv = new Dictionary<int, string>()
    {
        {0, "A"},
        {1, "B"},
        {2, "C"},
        {3, "D"},
        {4, "E"},
        {5, "F#"},
        {6, "G"},
    };

    public static Waveform GetRandomWaveform()
    {
        Array values = Enum.GetValues(typeof(Waveform));
        System.Random random = new System.Random();
        Waveform randomWave = (Waveform)values.GetValue(random.Next(values.Length));
        return randomWave;
    }

    public static float CalculateGMajorPosition(string pitch, int octave)
    {
        if (pitch == "Break") return 0;
        float frequency = 110f * Mathf.Pow(2f, GMajorPos[pitch] / 12f) * Mathf.Pow(2, octave);
        return frequency;
    }

    public static float CalculateFrequency(string pitch, int octave)
    {
        if (pitch == "Break") return 0;
        float frequency = 110f * Mathf.Pow(2f, noteFreqAndPos[pitch] / 12f) * Mathf.Pow(2, octave);
        return frequency;
    }

    public static NoteHelper[] MakeSongFromText(TextAsset song)
    {
        char[] separators = { ',', '\n', '\r' };
        string songNoSpace = Regex.Replace(song.text, " ", "");
        string[] songLines = songNoSpace.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
        NoteHelper[] songAsNotes = new NoteHelper[songLines.Length];
        for (int i = 0; i < songLines.Length; i++)
        {
            char[] noteSeparator = { '-' };
            string[] separatedNotes = songLines[i].Split(noteSeparator, System.StringSplitOptions.RemoveEmptyEntries);

            songAsNotes[i].pitch = separatedNotes[0];
            if (separatedNotes[1] == "*1")
            {
                songAsNotes[i].octave = -1;
            }
            else
            {
                songAsNotes[i].octave = System.Convert.ToInt32(separatedNotes[1]);
            }
            songAsNotes[i].waveform = System.Convert.ToInt32(separatedNotes[2]);
            songAsNotes[i].length = System.Convert.ToInt32(separatedNotes[3]);
        }
        return songAsNotes;
    }

}

public struct NoteHelper
{
    public string pitch;
    public int octave;
    public int waveform;
    public int length;
}
