using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargePlatformBreaker : MonoBehaviour {

    public float EnableTime;

    void OnEnable()
    {
        StartCoroutine(DisableSelf());
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
        if(other.gameObject.layer == LayerMask.NameToLayer("Platform") ||
           other.gameObject.layer == LayerMask.NameToLayer("PokeStop"))
        {
            Destroy(other.gameObject);
        }
    }

    IEnumerator DisableSelf()
    {
        yield return new WaitForSeconds(EnableTime);
        gameObject.SetActive(false);
    }
}
