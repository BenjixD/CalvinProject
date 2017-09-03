using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBallSpawner : MonoBehaviour {

    //TOTAL LENGTH SHOULD BE LESS THAN 0.6f
    //Delay spawn til 0.3f

    public Action<bool> Action;
    public bool WasFacingRight;

    public Vector3 SpawnOffset;

    public GameObject Projectile;

    #region Private Member
    private bool m_performedAction;
    #endregion

    // Use this for initialization
    void OnEnable () {
        m_performedAction = false;
        StartCoroutine(SpawnShadowBall());
	}

    void OnDisable()
    {
        StopAllCoroutines();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator SpawnShadowBall()
    {
        Vector3 spawnLocation = new Vector3(transform.position.x + (WasFacingRight ? SpawnOffset.x : -1 * SpawnOffset.x),
                                            transform.position.y + SpawnOffset.y,
                                            transform.position.z);

        GameObject obj = (GameObject)Instantiate(Projectile, spawnLocation, transform.rotation);

        obj.GetComponent<ShadowBallBehaviour>().SetTravelProperties(WasFacingRight ? new Vector3(1, 0, 0) : new Vector3(-1, 0, 0));
        obj.GetComponent<ShadowBallBehaviour>().Action = Action;        //Lambda
        obj.GetComponent<ShadowBallBehaviour>().enabled = true;

        gameObject.SetActive(false);

        yield return null;
    }
}
