using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiBlastBehaviour : MonoBehaviour, IBullet
{

    #region Public Members
    public Action<bool> Action;
    public float PersistentTime;
    public float ProjectileSpeed;
    public GameObject HitObject;

    public float Damage;
    public Vector3 Knockback;

    public IPlayer Target;
    #endregion

    #region Private Members
    Rigidbody2D m_objectRB;
    Vector3 m_direction;
    #endregion

    //Awake
    void Awake()
    {
        m_objectRB = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start()
    {
        if(Target != null)
        {
            Vector3 direction = (Target.GetObject.transform.position + new Vector3(0, UnityEngine.Random.Range(0, Target.GetObject.GetComponent<BoxCollider2D>().size.y), 0) - transform.position).normalized;
            SetTravelProperties(direction);
        }
        StartCoroutine("PersistUntilEnd");
    }

    void Update()
    {

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
        //Flip if going left
        if(direction.x < 0)
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    public void SetTravelProperties(Vector3 direction)
    {
        m_objectRB.velocity = direction * ProjectileSpeed;
        if (direction.x < 0)
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void DealDamageAndStatus(GameObject other)
    {
        other.GetComponent<Health>().DealDamage(Damage);
        other.GetComponent<IPlayer>().KnockbackPlayer(new Vector3((transform.localScale.x == 1? 1 : -1) * Knockback.x, Knockback.y, 0));
    }

    public void OnHit(GameObject other)
    {
        //Deal Effects to other GameObject
        DealDamageAndStatus(other);

        //Call hit anim
        Action(true);
        Instantiate(HitObject, transform.position, transform.rotation);
        Destroy(gameObject);
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
