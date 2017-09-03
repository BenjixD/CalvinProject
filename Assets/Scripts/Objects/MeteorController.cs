using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorController : MonoBehaviour {

    #region Public Members
    public GameObject Meteor;
    public float SpawnCooldown;
    public float CooldownDifferenceRange;       // Must not be more than SpawnCooldown.
    #endregion

    #region Private Members
    private BoxCollider2D m_box;
    private float m_maxWidthOffset = 6f;
    #endregion

    // Use this for initialization
    void Start () {

        m_box = GetComponent<BoxCollider2D>();

        // TODO: only spawn meteors if Mewtwo is 50% hp or something

        StartCoroutine(SpawnMeteors());
    }

    #region Helper Functions
    Vector3 choosePoint()
    {
        return new Vector3(transform.position.x + Random.Range(-m_maxWidthOffset, m_maxWidthOffset), transform.position.y, 0);
    }
    #endregion

    #region 
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
