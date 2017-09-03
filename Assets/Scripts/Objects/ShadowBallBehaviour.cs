using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBallBehaviour : MonoBehaviour, IBullet
{

    #region Public Members
    public Action<bool> Action;
    public float PersistentTime;
    public float ProjectileSpeed;
    public GameObject HitObject;

    public float Damage;
    public Vector3 Knockback;
    #endregion

    #region Private Members
    Rigidbody2D m_objectRB;
    private bool m_hit;
    private Vector3 m_direction;
    #endregion

    //Awake
    void Awake()
    {
        m_objectRB = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start()
    {
        m_hit = false;
        StartCoroutine("PersistUntilEnd");
    }

    #region Trigger Handlers
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            OnHit(collision.gameObject);
        }
    }
    #endregion

    #region Interface
    public void SetTravelProperties(Vector3 direction, float speed)
    {
        m_objectRB.velocity = direction * speed;
        m_direction = direction;
    }
    public void SetTravelProperties(Vector3 direction)
    {
        m_objectRB.velocity = direction * ProjectileSpeed;
        m_direction = direction;
    }

    void DealDamageAndStatus(GameObject other)
    {
        other.GetComponent<Health>().DealDamage(Damage);
        other.GetComponent<IPlayer>().KnockbackPlayer(new Vector3((m_direction.x < 0 ? -1 : 1) * Knockback.x, Knockback.y, 0));
    }

    public void OnHit(GameObject other)
    {
        //Deal Effects to other GameObject
        DealDamageAndStatus(other);

        //Call hit anim
        m_hit = true;
        Action(true);
        Instantiate(HitObject, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    #endregion

    #region Coroutine
    IEnumerator PersistUntilEnd()
    {
        yield return new WaitForSeconds(PersistentTime);

        if (!m_hit)
        {
            Action(false);
        }

        Destroy(this.gameObject);
        yield return null;
    }
    #endregion
}
