﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorBehaviour : MonoBehaviour {

    #region Public Members
    public GameObject Explosion;
    public Transform ExplosionSpawn;
    public float FallVelocity;
    public float MaxHorizontalVelocity;
    #endregion

    #region Private Members
    private Rigidbody2D m_rb;
    private float m_defaultAngle = 320f;
    #endregion

    void Start () {
        m_rb = GetComponent<Rigidbody2D>();
        RandomizeDirection();
	}
	
	void FixedUpdate () {
        CorrectDirection();
    }

    #region Helper Functions
    void CorrectDirection()
    {
        Vector2 from = new Vector2(0, -1);
        Vector2 movement = new Vector2(m_rb.velocity.x, m_rb.velocity.y);
        float angle = calculateAngle(from, movement);
        transform.rotation = Quaternion.Euler(0, 0, angle - m_defaultAngle);
    }

    void RandomizeDirection()
    {
        m_rb.velocity = new Vector3(Random.Range(-MaxHorizontalVelocity, MaxHorizontalVelocity), -FallVelocity, 0);
    }

    float calculateAngle(Vector2 vec1, Vector2 vec2)
    {
        float dotProduct = Vector2.Dot(vec1, vec2);
        float determinant = vec1.x * vec2.y - vec1.y * vec2.x;
        float angle = Mathf.Atan2(determinant, dotProduct) * 180 / Mathf.PI;

        return angle;
    }

    void DestroySelf()
    {
        Destroy(gameObject);
        Instantiate(Explosion, ExplosionSpawn.position, Quaternion.identity);
    }

    #endregion

    #region Trigger Handlers
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            // TODO: damage player

            DestroySelf();
        }
        else if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Platform")))
        {
            Destroy(other.gameObject);
            DestroySelf();
        }
        else if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Ground")))
        {
            DestroySelf();
        }
    }
    #endregion
}