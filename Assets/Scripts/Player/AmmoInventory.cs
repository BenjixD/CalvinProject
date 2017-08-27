using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoInventory : MonoBehaviour, IAmmo
{
    #region Public Members
    public GameObject[] AmmoTypes;
    #endregion

    #region Private Members
    IDictionary<GameObject, Supply> m_inventory;
    #endregion

    // Use this for initialization
    void Start () {
        m_inventory = new Dictionary<GameObject, Supply>();

        for(int i = 0; i < AmmoTypes.Length; ++i)
        {
            m_inventory.Add(AmmoTypes[i], new Supply(AmmoTypes[i]));
        }
	}

    public IDictionary<GameObject, Supply> GetNonEmptySupplies()
    {
        IDictionary<GameObject, Supply> nonEmptySupplies = new Dictionary<GameObject, Supply>();
        foreach (KeyValuePair<GameObject, Supply> item in m_inventory)
        {
            if(item.Value.Inventory > 0)
            {
                nonEmptySupplies.Add(item);
            }
        }
        return nonEmptySupplies;
    }

    #region Interface
    public GameObject[] GetAmmoTypes
    {
        get
        {
            return AmmoTypes;
        }
    }

    public IDictionary<GameObject, Supply> GetAmmo()
    {
        return m_inventory;
    }

    public int AmmoCount()
    {
        int count = 0;
        foreach(KeyValuePair<GameObject, Supply> item in m_inventory)
        {
            count += item.Value.Inventory;
        }
        return count;
    }
    #endregion

}
