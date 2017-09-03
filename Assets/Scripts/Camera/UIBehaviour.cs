using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBehaviour : MonoBehaviour {

    Text ammo;
    public Slider playerHP, mewtwoHP;

    public GameObject Calvin, Mewtwo;
    Health CalvinHealth, MewtwoHealth;
    AmmoInventory ammoInv;

    // Use this for initialization
	void Start () {
        ammo = transform.Find("AmmoCount").Find("Ammo").GetComponent<Text>();
        ammoInv = Calvin.GetComponent<AmmoInventory>();
        CalvinHealth = Calvin.GetComponent<Health>();
        MewtwoHealth = Mewtwo.GetComponent<Health>();

	}
	
	// Update is called once per frame
	void Update () {
        //Change later
        playerHP.normalizedValue = CalvinHealth.CurrentHealth / CalvinHealth.MaxHealth;
        mewtwoHP.normalizedValue = MewtwoHealth.CurrentHealth / MewtwoHealth.MaxHealth;
        ammo.text = ammoInv.AmmoCount().ToString();
	}
}
