using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokestopBehaviour : MonoBehaviour {

    #region Public Members
    public GameObject BlueStop;
    public GameObject PurpleStop;
    public float FadeDuration;
    #endregion

    #region Private Members
    bool m_used;
    float m_minAlpha = 0f;
    float m_maxAlpha = 1f;
    float m_startTime;
    float m_smoothStepDuration = 2f;
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
        foreach (Transform child in BlueStop.transform)
        {
            if(child.name == "PokestopEye")
            {
                child.GetComponent<Animator>().SetTrigger("TriggerSpin");
            }
        }
        foreach (Transform child in PurpleStop.transform)
        {
            if (child.name == "PokestopEye")
            {
                child.GetComponent<Animator>().SetTrigger("TriggerSpin");

            }
        }
    }

	IEnumerator FadeToPurple(){
        for(;;)
        {
            float t = (Time.time - m_startTime) / FadeDuration;
            foreach (SpriteRenderer renderer in BlueStop.GetComponentsInChildren<SpriteRenderer>())
            {
                renderer.color = new Color(1f, 1f, 1f, 1 - Mathf.SmoothStep(m_minAlpha, m_maxAlpha, t));
            }
            if (t >= m_smoothStepDuration)
            {
                yield break;
            }
            yield return null;
        }
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
                m_startTime = Time.time;
                StartCoroutine("FadeToPurple");
            }
        }
    }
    #endregion
}
