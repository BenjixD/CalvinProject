using System.Collections.Generic;
using UnityEngine;

public class Supply
{
    public GameObject Ammo;
    public int Inventory;

    public Supply(GameObject prefab, int initSupply = 0)
    {
        Ammo = prefab;
        Inventory = initSupply;
    }
}

interface IAmmo
{
    int AmmoCount();
    IDictionary<GameObject, Supply> GetAmmo();
    GameObject[] GetAmmoTypes { get; }
}

