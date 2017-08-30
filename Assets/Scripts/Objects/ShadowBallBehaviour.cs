using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBallBehaviour : MonoBehaviour, IBullet
{

    #region Public Members
    public float PersistentTime;
    public float ProjectileSpeed;
    #endregion

    #region Private Members
    Rigidbody2D m_objectRB;
    #endregion

    //Awake
    void Awake()
    {
        m_objectRB = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start()
    {
        //Scan for nearest Enemy, if failed then destroy after persistent time
        if (false)
        {

        }
        else
        {
            StartCoroutine("PersistUntilEnd");
        }
    }

    void Update()
    {

    }

    #region Interface
    public void SetTravelProperties(Vector3 direction, float speed)
    {
        m_objectRB.velocity = direction * speed;
    }
    public void SetTravelProperties(Vector3 direction)
    {
        m_objectRB.velocity = direction * ProjectileSpeed;
    }

    public void OnHit(GameObject other)
    {
        //Deal Effects to other GameObject
    }
    #endregion

    #region Coroutine
    IEnumerator PersistUntilEnd()
    {
        yield return new WaitForSeconds(PersistentTime);
        Destroy(this.gameObject);
        yield return null;
    }
    #endregion
}
