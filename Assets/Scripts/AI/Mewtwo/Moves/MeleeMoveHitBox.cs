using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMoveHitBox : MonoBehaviour {

    //TOTAL LENGTH SHOULD BE LESS THAN 0.6f

    public Action<bool> Action;
    public float StartHitBox;
    public float HitBoxDuration;
    public float DelayDeactivate;

    private bool isHit;

	// Use this for initialization
	void OnEnable () {
        isHit = false;
        StartCoroutine("ActivateAndDeactivateCollider");
	}

    void OnDisable()
    {
        StopAllCoroutines();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            isHit = true;
        }
    }

    IEnumerator ActivateAndDeactivateCollider()
    {
        yield return new WaitForSeconds(StartHitBox);
        GetComponent<BoxCollider2D>().enabled = true;

        GetComponent<BoxCollider2D>().offset += new Vector2(0.001f, 0f); //reset Collider Trigger
        GetComponent<BoxCollider2D>().offset += new Vector2(-0.001f, 0f); //reset Collider Trigger

        yield return new WaitForSeconds(HitBoxDuration);
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(DelayDeactivate);
        Action(isHit);
        gameObject.SetActive(false);

        yield return null;
    }
}
