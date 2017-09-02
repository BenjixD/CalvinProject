using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokestopBehaviour : MonoBehaviour {

    #region Public Members
    public GameObject[] ChildrenToAnimate;
    public float DestroyDelay;
    #endregion

    #region Private Members
    bool m_used;
    #endregion

    // Use this for initialization
    void Start () {
        m_used = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    #region Helper Functions
    private void SpinningAnim()
    {
		foreach (GameObject child in ChildrenToAnimate) {
			child.GetComponent<Animator>().SetTrigger("TriggerSpin");
		}
    }

	private void FadeToPurple(){
		
	}
    #endregion

    #region Collision Handlers
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            if(!m_used)
            {
                m_used = true;
                SpinningAnim();
				FadeToPurple();
            }
        }
    }
    #endregion
}
