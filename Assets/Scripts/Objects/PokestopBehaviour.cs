using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokestopBehaviour : MonoBehaviour {

    #region Public Members
    public GameObject ChildToAnimate;
    public float DestroyDelay;
    #endregion

    #region Private Members
    Animator m_animator;
    bool m_used;
    #endregion

    // Use this for initialization
    void Start () {
        m_animator = ChildToAnimate.GetComponent<Animator>();
        m_used = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    #region Helper Functions
    private void SpinningAnim()
    {
        m_animator.SetTrigger("TriggerSpin");
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
            }
        }
    }
    #endregion
}
