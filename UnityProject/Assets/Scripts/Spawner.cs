using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode { Premade, Random, Free };
public class Spawner : MonoBehaviour
{
    public GameObject SingleNotePrefab;
    public GameObject LongNotePrefab;
    public GameMode gameMode;
    public GameObject menuCanvas;
    public Button btn;
    public TextAsset song;


    [Range(1, 16)]
    public int spaceBetweenNotes = 1; //don't change it it will ruin everything
    public float speed;
    public int lanes = 5;
    public float spawnBoundary = 0.32f;
    Vector2 screenHalfSizeInWorldUnits;
    public float beatsPerMinute = 60;
    float timeBetweenSpawnsInSeconds;
    float nextSpawnTime;
    NoteHelper[] songArray;
    int currentNote;
    public float delay;
    public bool gameOn = false;
    public float currTime;

    void Start()
    {
        btn.onClick.AddListener(StartGame);
        timeBetweenSpawnsInSeconds = 60f / beatsPerMinute;
        speed = spaceBetweenNotes * 0.64f / timeBetweenSpawnsInSeconds;
        screenHalfSizeInWorldUnits = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
        songArray = RandomEnumSetter.MakeSongFromText(song);
        currentNote = 0;
        float dist = screenHalfSizeInWorldUnits.x - FindObjectOfType<Player>().transform.position.x;
        delay = dist / speed;
    }

    void Update()
    {
        if (gameOn)
        {
            if (true)
            {
                if (Time.time > nextSpawnTime)
                {
                    if (gameMode == GameMode.Random)
                    {
                        Vector2 spawnPosition = new Vector2(screenHalfSizeInWorldUnits.x + 0.64f, -screenHalfSizeInWorldUnits.y + spawnBoundary + Random.Range(0, lanes) * 2 * (screenHalfSizeInWorldUnits.y - spawnBoundary) / (lanes - 1));
                        Waveform randomWave = RandomEnumSetter.GetRandomWaveform();
                        if (Random.Range(0, 2) == 0)
                        {
                            nextSpawnTime = Time.time + timeBetweenSpawnsInSeconds;
                            GameObject newNote = Instantiate(SingleNotePrefab, spawnPosition, Quaternion.identity);
                            newNote.GetComponent<Note>().speed = speed;
                            newNote.GetComponent<Note>().SetColor(randomWave);
                        }
                        else
                        {
                            int noteLength = Random.Range(2, 4);
                            nextSpawnTime = Time.time + timeBetweenSpawnsInSeconds + (0.64f * (noteLength - 1) / speed);
                            GameObject newLongNote = Instantiate(LongNotePrefab, spawnPosition, Quaternion.identity);
                            newLongNote.GetComponent<LongNote>().speed = speed;
                            newLongNote.GetComponent<LongNote>().noteLength = noteLength;
                            newLongNote.GetComponent<LongNote>().wave = randomWave;
                        }
                    }
                    if (gameMode == GameMode.Premade)
                    {
                        if (currentNote < songArray.Length)
                        {
                            int noteLength = songArray[currentNote].length;
                            if (songArray[currentNote].pitch == "Break")
                            {
                                nextSpawnTime = Time.time + timeBetweenSpawnsInSeconds + (0.64f * (noteLength - 1) / speed);
                            }
                            else
                            {
                                int notePosition = RandomEnumSetter.GMajorPos[songArray[currentNote].pitch];
                                int octaveOffset = (songArray[currentNote].octave - 1) * 8;

                                Vector2 spawnPosition = new Vector2(screenHalfSizeInWorldUnits.x + 0.64f, -screenHalfSizeInWorldUnits.y + spawnBoundary + (notePosition + octaveOffset) * 2 * (screenHalfSizeInWorldUnits.y - spawnBoundary) / (lanes - 1));
                                if (noteLength == 1)
                                {
                                    nextSpawnTime = Time.time + timeBetweenSpawnsInSeconds;
                                    GameObject newNote = Instantiate(SingleNotePrefab, spawnPosition, Quaternion.identity);
                                    newNote.GetComponent<Note>().speed = speed;
                                    newNote.GetComponent<Note>().pitch = songArray[currentNote].pitch;
                                    newNote.GetComponent<Note>().octave = songArray[currentNote].octave;
                                    newNote.GetComponent<Note>().wave = (Waveform)songArray[currentNote].waveform;
                                    newNote.GetComponent<Note>().SetColor((Waveform)songArray[currentNote].waveform);
                                }
                                else
                                {
                                    nextSpawnTime = Time.time + timeBetweenSpawnsInSeconds + (0.64f * (noteLength - 1) / speed);
                                    GameObject newLongNote = Instantiate(LongNotePrefab, spawnPosition, Quaternion.identity);
                                    newLongNote.GetComponent<LongNote>().noteLength = songArray[currentNote].length;
                                    newLongNote.GetComponent<LongNote>().speed = speed;
                                    newLongNote.GetComponent<LongNote>().pitch = songArray[currentNote].pitch;
                                    newLongNote.GetComponent<LongNote>().octave = songArray[currentNote].octave;
                                    newLongNote.GetComponent<LongNote>().wave = (Waveform)songArray[currentNote].waveform;
                                }
                            }
                            currentNote++;
                        }
                        else
                        {
                            if (Time.time > delay + (songArray.Length + 2) * 0.64f / speed)
                            {
                                gameOn = false;
                                Debug.Log(FindObjectOfType<Player>().noteAccuracy);
                            }
                        }
                    }
                }
            }
        }
    }

    void StartGame()
    {
        gameOn = true;
        menuCanvas.SetActive(false);
        currTime = Time.time;
    }

}