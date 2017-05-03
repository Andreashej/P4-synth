using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNote : MonoBehaviour
{
    public Waveform wave;
    public string pitch;
    public int octave;
    public float speed = 3f;

    [Range(2, 16)]
    public int noteLength = 2;
    int length; //length 0 means it's 2 notes long - head, midsection and tail are all 0.64 units.
    Vector2 screenHalfSizeInWorldUnits;
    public GameObject HeadPrefab;
    public GameObject MidPrefab;
    public GameObject TailPrefab;
    float beatsPerMinute;
    float timeBetweenSpawnsInSeconds;

    void Start()
    {
        beatsPerMinute = FindObjectOfType<Spawner>().beatsPerMinute;
        timeBetweenSpawnsInSeconds = 60f / beatsPerMinute;
        speed = FindObjectOfType<Spawner>().spaceBetweenNotes * 0.64f / timeBetweenSpawnsInSeconds;
        length = noteLength - 2;
        screenHalfSizeInWorldUnits = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
        GameObject newHead = Instantiate(HeadPrefab, transform.position, Quaternion.identity);
        newHead.transform.parent = gameObject.transform;
        newHead.GetComponent<Head>().SetColor(wave);
        for (int i = 0; i < length; i++)
        {
            GameObject newMid = Instantiate(MidPrefab, new Vector2(transform.position.x + (i + 1) * 0.64f, transform.position.y), Quaternion.identity);
            if (newMid != null)
            {
                newMid.transform.parent = gameObject.transform;
                newMid.GetComponent<Mid>().SetColor(wave);
            }
        }
        GameObject newTail = Instantiate(TailPrefab, new Vector2(transform.position.x + (length + 1) * 0.64f, transform.position.y), Quaternion.identity);
        newTail.transform.parent = gameObject.transform;
        newTail.GetComponent<Tail>().SetColor(wave);
        gameObject.GetComponent<CapsuleCollider2D>().offset = new Vector2(-0.32f + noteLength * 0.64f / 2f, 0);
        gameObject.GetComponent<CapsuleCollider2D>().size = new Vector2(noteLength * 0.64f, 0.64f);
    }

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        if (transform.position.x + noteLength * 0.64f / 2f <= -(screenHalfSizeInWorldUnits.x + 0.64f))
        {
            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }
    }
}
