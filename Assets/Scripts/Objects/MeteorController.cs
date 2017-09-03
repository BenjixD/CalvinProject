using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorController : MonoBehaviour {

    #region Public Members
    public GameObject Meteor;
    public GameObject Boss;
    public float SpawnCooldown;
    public float CooldownDifferenceRange;       // Must not be more than SpawnCooldown.
    public float HealthPercentActivation;
    #endregion

    #region Private Members
    private BoxCollider2D m_box;
    private float m_maxWidthOffset = 6f;
    private float m_waitCheck = 10f;
    private float m_meteorCheckWarmupTime = 3f;
    #endregion

    // Use this for initialization
    void Start () {
        m_box = GetComponent<BoxCollider2D>();
        StartCoroutine("WatchBossHealth");
    }

    #region Helper Functions
    Vector3 choosePoint()
    {
        return new Vector3(transform.position.x + Random.Range(-m_maxWidthOffset, m_maxWidthOffset), transform.position.y, 0);
    }
    #endregion

    #region Coroutines
    IEnumerator WatchBossHealth()
    {
        yield return new WaitForSeconds(m_meteorCheckWarmupTime);
        for (;;)
        {
            if (Boss.GetComponent<Health>().CurrentHealth / Boss.GetComponent<Health>().MaxHealth <  HealthPercentActivation / 100)
            {
                StartCoroutine(SpawnMeteors());
                yield break;
            }
            yield return new WaitForSeconds(m_waitCheck);
        }
    }

    IEnumerator SpawnMeteors()
    {
        for (;;)
        {
            Instantiate(Meteor, choosePoint(), Quaternion.identity);
            yield return new WaitForSeconds(SpawnCooldown + Random.Range(-CooldownDifferenceRange, CooldownDifferenceRange));
        }
    }
    #endregion
}
