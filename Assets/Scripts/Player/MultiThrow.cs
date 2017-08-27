using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultiThrow : MonoBehaviour, IAttack
{
    #region Public Members
    public int NumThrows;
    public Vector3 SpawnOffset;
    #endregion

    #region Private Members
    private IPlayer m_player;
    private IAmmo m_ammo;

    private bool m_attacking;
    #endregion

    void Start()
    {
        m_player = GetComponent<Player>();
        m_ammo = GetComponent<AmmoInventory>();

        m_attacking = false;
    }

    #region Helper Functions
    private void SpawnPokeball(Supply toSpawn)
    {
        bool facingRight = m_player.IsFacingRight;

        //Spawn Object and decrement inventory
        Vector3 spawnLocation = new Vector3(transform.position.x + (facingRight ? SpawnOffset.x : -1 * SpawnOffset.x),
                                              transform.position.y + SpawnOffset.y,
                                              transform.position.z);
        GameObject obj = (GameObject)Instantiate(toSpawn.Ammo, spawnLocation, transform.rotation);
        obj.GetComponent<IBullet>().SetTravelProperties(facingRight? new Vector3(1, 0, 0): new Vector3(-1, 0, 0));
    }
    #endregion

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
        for(int i = 0; i < NumThrows; ++i)
        {
            AmmoInventory am = m_ammo as AmmoInventory;
            IList<Supply> nonEmptyAmmo = Enumerable.ToList(am.GetNonEmptySupplies().Values);

            //Sanity Check we aren't out of ammo
            if(nonEmptyAmmo.Count > 0)
            {
                Supply toSpawn = nonEmptyAmmo[Random.Range(0, nonEmptyAmmo.Count)];    //Randomize which non-empty pokeball inventory to choose from

                SpawnPokeball(toSpawn);
                toSpawn.Inventory--;
            }

            yield return new WaitForSeconds(0.1f);
        }

        yield return null;
    }
    #endregion
}

