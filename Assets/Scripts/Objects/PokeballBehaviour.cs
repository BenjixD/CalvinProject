using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeballBehaviour : MonoBehaviour, IBullet
{

    #region Public Members
    public Vector3 ScanOffsetTopRight;
    public Vector3 ScanOffsetBottomLeft;
    public float PersistentTime;
    public float ProjectileSpeed;
    public GameObject HitObject;
    #endregion

    #region Private Members
    Rigidbody2D m_objectRB;
    GameObject m_target;
    Vector3 m_direction;
    #endregion

    //Awake
    void Awake()
    {
        m_objectRB = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start () {
	    //Scan for nearest Enemy, if failed then destroy after persistent time
        if(ScanForNearestEnemy())
        {
            Vector3 direction = (m_target.transform.position + new Vector3(0, Random.Range(0, m_target.GetComponent<BoxCollider2D>().size.y), 0) - transform.position).normalized;
            SetTravelProperties(direction);
        }
        else
        {
            StartCoroutine("PersistUntilEnd");
        }
	}

    void Update()
    {

    }

    private bool ScanForNearestEnemy()
    {
        Vector3 corner1 = m_direction.x > 0 ? ScanOffsetBottomLeft : -1 * ScanOffsetBottomLeft;
        Vector3 corner2 = m_direction.x > 0 ? ScanOffsetTopRight : -1 * ScanOffsetTopRight;

        Collider2D[] hitColliders = Physics2D.OverlapAreaAll(transform.position + corner1,
                                                             transform.position + corner2,
                                                             1 << LayerMask.NameToLayer("Enemy"));

        if(hitColliders.Length > 0)
        {
            m_target = hitColliders[0].gameObject;
        }
        
        return (hitColliders.Length > 0);
    }

    #region Trigger Handlers
    void OnTriggerEnter2D(Collider2D other)
    {
        // TODO: check if other is an enemy
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            OnHit(other.gameObject);
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

    public void OnHit(GameObject other)
    {
        //Deal Effects to other GameObject

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
