using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed = 5f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float inputY = Input.GetAxisRaw("Vertical");
        float velocity = inputY * speed;
        transform.Translate(Vector2.up * velocity * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D triggerCollider) //This one is used for scoring
    {
        NoteHelper note;
        note.pitch = "";
        note.octave = 0;
        note.waveform = 0;
        if (triggerCollider.tag == "Single Note")
        {
            Debug.Log("Collided with " + triggerCollider.tag);
            note.pitch = triggerCollider.GetComponent<Note>().pitch;
            note.waveform = (int)triggerCollider.GetComponent<Note>().wave;
            note.octave = triggerCollider.GetComponent<Note>().octave;
            float freq = RandomEnumSetter.CalculateFrequency(note.pitch, note.octave);

            string msg = "/ch1" + " /freq " + freq.ToString();
            tcpserver.PDSend(msg);
            msg = "/ch1" + " /selector " + note.waveform.ToString();
            tcpserver.PDSend(msg);
        }

        else if (triggerCollider.tag == "Long Note")
        {
            Debug.Log("Collided with " + triggerCollider.tag);
            note.pitch = triggerCollider.GetComponent<LongNote>().pitch;
            note.waveform = (int)triggerCollider.GetComponent<LongNote>().wave;
            note.octave = triggerCollider.GetComponent<LongNote>().octave;
            float freq = RandomEnumSetter.CalculateFrequency(note.pitch, note.octave);

            string msg = "/ch1" + " /freq " + freq.ToString();
            tcpserver.PDSend(msg);
            msg = "/ch1" + " /selector " + note.waveform.ToString();
            tcpserver.PDSend(msg);
        }

        //else if (note.pitch != "") Debug.Log(note.pitch + note.waveform + note.octave);
    }

    void OnTriggerStay2D(Collider2D triggerCollider) //This one is used to play note
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        NoteHelper note;
        note.pitch = "";
        note.waveform = 0;
        note.octave = 0;

        if (triggerCollider.tag == "Single Note")
        {
            note.pitch = triggerCollider.GetComponent<Note>().pitch;
            note.waveform = (int)triggerCollider.GetComponent<Note>().wave;
            note.octave = triggerCollider.GetComponent<Note>().octave;
        }
        else if (triggerCollider.tag == "Long Note")
        {
            note.pitch = triggerCollider.GetComponent<LongNote>().pitch;
            note.waveform = (int)triggerCollider.GetComponent<LongNote>().wave;
            note.octave = triggerCollider.GetComponent<LongNote>().octave;
        }
        else if (note.pitch != "") Debug.Log(note.pitch + note.waveform + note.octave);
    }

    void OnTriggerExit2D(Collider2D triggerCollider) //This one is used to stop playing note, it may not be needed.
    {
        if (triggerCollider.tag != "Head")
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            tcpserver.PDSend("/ch1 /selector 0");
        }
    }
}
