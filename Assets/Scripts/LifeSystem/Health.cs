using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public float MaxHealth;

    #region private members
    private float m_currentHealth;
    private IDictionary<string, Func<float, float>> m_multipliers;
    #endregion

    #region Properties
    public float CurrentHealth
    {
        get
        {
            return m_currentHealth;
        }
    }
    #endregion

    #region Usage
    public void AddModifiers(KeyValuePair<string, Func<float, float>> modifier)
    {
        try
        {
            m_multipliers.Add(modifier);
        }
        catch (Exception e)
        {
            //DEBUG
            Debug.Log("Modifier already added! " + e.Message);
            //
        }
    }

    public void DealDamage(float damage)
    {
        float finalDamage = damage;

        //Iterate through lambda of damage modifiers (IE. debuff effects)
        foreach(KeyValuePair<string, Func<float, float>> mod in m_multipliers)
        {
            finalDamage = mod.Value(finalDamage);
        }

        m_currentHealth -= finalDamage;

        //DEBUG
        Debug.Log(m_currentHealth);
        //
    }
    #endregion

    // Use this for initialization
    void Start () {
        m_currentHealth = MaxHealth;
        m_multipliers = new Dictionary<string, Func<float, float>>();
	}
}
