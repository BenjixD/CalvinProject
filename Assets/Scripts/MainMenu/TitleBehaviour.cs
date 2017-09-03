using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleBehaviour : MonoBehaviour {

    Text title;
    Rigidbody2D rb;

    public float colorChangeTime;
    private float elapsedTime;

    public float titleSpeed;
    private Vector2 spd;
    private bool firstRun;

	// Use this for initialization
	void Start () {
        title = GetComponent<Text>();
        rb = GetComponent<Rigidbody2D>();
        elapsedTime = 0;

        spd = new Vector2(titleSpeed, 0);
        rb.velocity = spd;
        firstRun = true;
	}
	
	// Update is called once per frame
	void Update () {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= colorChangeTime) {
            spd.x *= -1;
            rb.velocity = spd;
            title.color = GetRandomColor();
            elapsedTime = 0;
            if (firstRun)
            {
                colorChangeTime *= 2;
                firstRun = false;
            }
        }
	}

    Color GetRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value);
    }
}
