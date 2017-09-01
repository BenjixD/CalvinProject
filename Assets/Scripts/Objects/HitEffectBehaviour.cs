using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectBehaviour : MonoBehaviour {

    #region Public Members
    public AnimationClip anim;
    #endregion
    
    void Start () {
        Destroy(gameObject, anim.length);
    }
}
