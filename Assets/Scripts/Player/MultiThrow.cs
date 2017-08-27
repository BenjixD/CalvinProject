using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiThrow : MonoBehaviour, IAttack
{
    #region Public Members
    public int NumThrows;
    #endregion

    #region Private Members
    private IPlayer m_player;
    private IAmmo m_ammo;

    private bool m_attacking;
    #endregion

    void Start()
    {
        m_player = GetComponent<IPlayer>();
        m_ammo = GetComponent<IAmmo>();

        m_attacking = false;
    }

    #region Interface Members
    public void InitAttack()
    {
        if(m_ammo.AmmoCount() > 0 && !m_attacking)
        {
            StartCoroutine("SpawnAmmo");
        }
    }
    #endregion

    #region Coroutines
    IEnumerator SpawnAmmo()
    {
        m_attacking = true;     //Guard against consuming more ammo than necessary

        bool facingRight = m_player.IsFacingRight;
        for(int i = 0; i < NumThrows; ++i)
        {
            GameObject toSpawn = m_ammo.GetAmmoTypes[Random.Range(0, m_ammo.GetAmmoTypes.Length)];  //Randomize which pokeball to choose

            //obj = (GameObject)Instantiate(Prefab, MyShip.transform.position, MyShip.transform.rotation);
            yield return new WaitForSeconds(0.1f);
        }

        m_attacking = false;    //End Guard
        yield return null;
    }
    #endregion
}

