using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    Animator aC;
    public Waveform wave;
    public string pitch;
    public int octave;
    public float speed = 3f;
    
    Vector2 screenHalfSizeInWorldUnits;


    // Use this for initialization
    void Start()
    {
        screenHalfSizeInWorldUnits = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        if (transform.position.x <= -(screenHalfSizeInWorldUnits.x + 0.64f))
        {
			if(gameObject != null){
				Destroy(gameObject);
			}
        }
    }

    public void SetColor(Waveform inputWave)
    {
        aC = gameObject.GetComponent<Animator>();
        aC.runtimeAnimatorController = Resources.Load("Sprites/note_" + RandomEnumSetter.colors[(int)inputWave] + "_0") as RuntimeAnimatorController;
    }
}
