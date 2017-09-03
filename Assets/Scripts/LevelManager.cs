using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public GameObject Calvin, Mewtwo;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (!Calvin)
        {
            StartCoroutine(ReturnAfterSeconds(2f, 0));
        }
        if (!Mewtwo)
            StartCoroutine(ReturnAfterSeconds(2f, 2));
	}

    IEnumerator ReturnAfterSeconds(float s, int scene)
    {
        yield return new WaitForSeconds(s);
        SceneManager.LoadScene(scene);
    }
}
