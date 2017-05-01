using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class applyText : MonoBehaviour {
	Text msg;

	// Use this for initialization
	void Start () {
		msg = GetComponent<Text>();
		Arduino.NewDataEvent += NewData;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void NewData(Arduino arduino) {
		
	}
}
