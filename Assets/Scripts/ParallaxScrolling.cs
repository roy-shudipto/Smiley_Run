using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScrolling : MonoBehaviour {

    public float scrollSpeed = -0.5f;

    void Update () 
    {
        Vector2 offset = new Vector2(Time.time/2 * scrollSpeed, 0);
        GetComponent<Renderer>().material.mainTextureOffset = offset;
	}
}
