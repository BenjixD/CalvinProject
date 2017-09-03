using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffect : MonoBehaviour
{

    #region Public Members
    public AnimationClip anim;
    #endregion

    #region Private Member
    private Animator m_animator;
    #endregion

    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f && !m_animator.IsInTransition(0))
        {
            Destroy(this.gameObject);
        }
    }
}
