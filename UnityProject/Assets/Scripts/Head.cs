using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour {
	
	Animator aC;

	public void SetColor(Waveform inputWave)
    {
        aC = gameObject.GetComponent<Animator>();
        aC.runtimeAnimatorController = Resources.Load("Sprites/note_long_left_" + RandomEnumSetter.colors[(int)inputWave] + "_0") as RuntimeAnimatorController;
    }
}
