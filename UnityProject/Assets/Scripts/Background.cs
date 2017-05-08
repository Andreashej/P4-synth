using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{

    public GameObject bg;
    Vector2 screenHalfSizeInWorldUnits;
    int bgCount;
    float bgWidth;

    // Use this for initialization
    void Start()
    {
        screenHalfSizeInWorldUnits = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
        bgCount = 0;
        bgWidth = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 spawnPosition = new Vector3(0, 0, 1);
        if (bgCount * bgWidth < 2 * screenHalfSizeInWorldUnits.x)
        {
            GameObject bgElement = (GameObject) Instantiate(bg, spawnPosition, Quaternion.identity);
            //RectTransform rt = (RectTransform)bgElement.transform;
            float scale = screenHalfSizeInWorldUnits.y * 2 / bgElement.GetComponent<SpriteRenderer>().bounds.size.y;
            bgWidth = scale * bgElement.GetComponent<SpriteRenderer>().bounds.size.x;
            bgElement.transform.localScale = new Vector3(scale, scale, 1);
            bgElement.transform.position = new Vector3(-screenHalfSizeInWorldUnits.x + bgWidth / 2 + bgCount * bgWidth, 0, 1);
            bgCount++;
        }
    }
}
