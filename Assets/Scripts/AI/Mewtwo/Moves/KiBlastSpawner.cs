using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiBlastSpawner : MonoBehaviour {

    //TOTAL LENGTH SHOULD BE LESS THAN 0.6f
    //Delay spawn til 0.3f

    public Action<bool> Action;
    public bool WasFacingRight;

    public float SpawnDelay;
    public Vector3 SpawnOffset;
    public IPlayer Target;

    public int NumSpawn;
    public float SpawnInBetweenDelay;

    public GameObject Projectile;

    #region Private Member
    private bool m_performedAction;
    #endregion

    // Use this for initialization
    void OnEnable () {
        m_performedAction = false;
        StartCoroutine(SpawnKiBlast());
	}

    void OnDisable()
    {
        StopAllCoroutines();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void PerformAction(bool success)
    {
        if(!m_performedAction)
        {
            Action(success);
            m_performedAction = true;
        }
    }

    bool CheckAllProjectilesDespawned(IList<GameObject> projectiles)
    {
        foreach(GameObject projectile in projectiles)
        {
            if(projectile != null)
            {
                return false;
            }
        }

        return true;
    }

    IEnumerator SpawnKiBlast()
    {
        IList<GameObject> projectiles = new List<GameObject>();
        Vector3 spawnLocation = new Vector3(transform.position.x + (WasFacingRight ? SpawnOffset.x : -1 * SpawnOffset.x),
                                            transform.position.y + SpawnOffset.y,
                                            transform.position.z);
        yield return new WaitForSeconds(SpawnDelay);

        for(int i = 0; i < NumSpawn; ++i)
        {
            GameObject obj = (GameObject)Instantiate(Projectile, spawnLocation, transform.rotation);
            projectiles.Add(obj);

            obj.GetComponent<KiBlastBehaviour>().SetTravelProperties(WasFacingRight ? new Vector3(1, 0, 0) : new Vector3(-1, 0, 0));
            obj.GetComponent<KiBlastBehaviour>().Action = PerformAction;        //Lambda

            if((Target.GetObject.transform.position.x < transform.position.x && !WasFacingRight) ||
                (Target.GetObject.transform.position.x > transform.position.x && WasFacingRight))
            obj.GetComponent<KiBlastBehaviour>().Target = Target;
            obj.GetComponent<KiBlastBehaviour>().enabled = true;

            
            yield return new WaitForSeconds(SpawnInBetweenDelay);
        }

        //Wait for all projectiles to despawn
        while(!CheckAllProjectilesDespawned(projectiles))
        {
            yield return new WaitForFixedUpdate();
        }

        //Failed Action
        if(!m_performedAction)
        {
            Action(false);
        }

        gameObject.SetActive(false);

        yield return null;
    }
}
