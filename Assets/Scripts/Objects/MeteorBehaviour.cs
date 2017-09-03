using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorBehaviour : MonoBehaviour {

    #region Public Members
    public GameObject Explosion;
    public Transform ExplosionSpawn;
    #endregion

    #region Private Members
    private Rigidbody2D m_rb;
    private Vector3 m_explosionOffset = new Vector3(-1, -1, 0);
    #endregion

    void Start () {
        m_rb = GetComponent<Rigidbody2D>();
        m_rb.velocity = new Vector3(0, -3, 0);
	}
	
	void Update () {

        // TODO: set direction of meteor
        
    }

    #region Trigger Handlers
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            // TODO: damage player?

        }
        else if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Platform")))
        {
            Destroy(other.gameObject);
        }
        Instantiate(Explosion, ExplosionSpawn.position, Quaternion.identity);
        Destroy(gameObject);
        // TODO: ignore mewtwo?
        // TODO: Meteor Explosion Behaviour
    }
    #endregion
}
