using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour {

    #region Public Members
    public GameObject[] PlatformLayouts;
    public float TranslucentPlatformAlpha;
    public float TranslucentWaitTime;
    #endregion

    // Use this for initialization
    void Start () {

        // TODO: if all platforms are gone, and after x seconds:
        StartCoroutine(SpawnPlatformLayout(GetRandomLayoutIndex()));

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    #region Helper Functions
    int GetRandomLayoutIndex()
    {
        return Random.Range(0, PlatformLayouts.Length);
    }

    void DeactivateLayout(GameObject layout)
    {
        foreach (SpriteRenderer renderer in layout.GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.color = new Color(1f, 1f, 1f, TranslucentPlatformAlpha);
        }
        foreach (EdgeCollider2D collider in layout.GetComponentsInChildren<EdgeCollider2D>())
        {
            collider.enabled = false;
        }
    }
    
    void ActivateLayout(GameObject layout)
    {
        foreach (SpriteRenderer renderer in layout.GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.color = new Color(1f, 1f, 1f, 1f);
        }
        foreach (EdgeCollider2D collider in layout.GetComponentsInChildren<EdgeCollider2D>())
        {
            collider.enabled = true;
        }
    }
    #endregion

    #region Coroutine
    IEnumerator SpawnPlatformLayout(int layoutIndex)
    {
        GameObject layout = Instantiate(PlatformLayouts[layoutIndex]);
        DeactivateLayout(layout);
        yield return new WaitForSeconds(TranslucentWaitTime);
        ActivateLayout(layout);
        yield return null;
    }
    #endregion
}
