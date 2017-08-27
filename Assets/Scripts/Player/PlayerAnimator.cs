using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {

    #region Private Members
    Animator m_animator;
    Rigidbody2D m_playerRB;
    Player m_player;
    #endregion

    // Use this for initialization
    void Start () {
        m_playerRB = GetComponent<Rigidbody2D>();
        m_player = GetComponent<Player>();
        m_animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        //Set Jumping State
        JumpingAnim();
        //Set Moving State
        MovingAnim();
        //Set Attacking State
        AttackingAnim();
	}

    #region Helper Functions
    private void JumpingAnim()
    {
        m_animator.SetBool("Grounded", m_player.Grounded);
    }

    private void MovingAnim()
    {
        m_animator.SetBool("Moving", m_player.Moving);
    }

    private void AttackingAnim()
    {
        if(m_player.Attacking)
        {
            int attackNum = Random.Range(0, 3);
            switch (attackNum)
            {
                case 0:
                    m_animator.SetTrigger("Attack1");
                    break;
                case 1:
                    m_animator.SetTrigger("Attack2");
                    break;
                case 2:
                    m_animator.SetTrigger("Attack3");
                    break;
            }
        }
    }
    #endregion
}
