using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatyBehaviour : MonoBehaviour {

    public float BobbleTime;
    public int NumSteps;
    public Vector3 Step;

    private Vector3 m_originalPosition;

    #region Properties
    public Vector3 OriginalPosition
    {
        get
        {
            return m_originalPosition;
        }
    }
    #endregion

    // Use this for initialization
    void OnEnable()
    {
        m_originalPosition = transform.position;
        StartCoroutine(FloatUp());
    }

    void OnDisable()
    {
        transform.position = m_originalPosition;
        StopAllCoroutines();
    }

    #region Coroutines
    IEnumerator FloatUp()
    {
        for(int i = 0; i < NumSteps; ++i)
        {
            //Bobble Up
            m_originalPosition = transform.position - (Step * i);
            transform.position += Step;

            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(FloatDown());
        yield return null;
    }

    IEnumerator FloatDown()
    {
        for (int i = NumSteps - 1; i >= 0; --i)
        {
            //Bobble Down
            m_originalPosition = transform.position - (Step * i);
            transform.position -= Step;

            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(FloatUp());
        yield return null;
    }
    #endregion
}
