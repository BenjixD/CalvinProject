using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashJumpBehaviour : MonoBehaviour {

    #region Private Members
    Animator m_animator;
    #endregion

    // Use this for initialization
    void Start () {
        m_animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f && !m_animator.IsInTransition(0))
        {
            Destroy(this.gameObject);
        }
	}
}
