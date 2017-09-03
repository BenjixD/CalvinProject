using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokestopBehaviour : MonoBehaviour {

    #region Public Members
    public GameObject BlueStop;
    public GameObject PurpleStop;
    public float FadeDuration;

    public GameObject Player;

    public int NumAmmoTypes;
    public int NumThrows;
    public float[] AmmoRarity;
    public float AmmoRandomness;
    public float AmmoGrantMinMultiplier;
    public float AmmoGrantMaxMultiplier;

    public AudioSource refillSFX;
    #endregion

    #region Private Members
    bool m_used;
    float m_minAlpha = 0f;
    float m_maxAlpha = 1f;
    float m_startTime;
    float m_smoothStepDuration = 2f;
    #endregion

    // Use this for initialization
    void Start () {
        m_used = false;
    }

    // Update is called once per frame
    void Update () {
		
	}

    #region Helper Functions
    public void SetPlayerReference(GameObject player)
    {
        Player = player;
    }

    private void SpinningAnim()
    {
        foreach (Transform child in BlueStop.transform)
        {
            if(child.name == "PokestopEye")
            {
                child.GetComponent<Animator>().SetTrigger("TriggerSpin");
            }
        }
        foreach (Transform child in PurpleStop.transform)
        {
            if (child.name == "PokestopEye")
            {
                child.GetComponent<Animator>().SetTrigger("TriggerSpin");

            }
        }
    }

    private int GetRandomMultiplier()
    {
        return (int)(Random.Range(AmmoGrantMinMultiplier, AmmoGrantMaxMultiplier) + Random.Range(AmmoGrantMinMultiplier, AmmoGrantMaxMultiplier));
    }

    private void GrantItems()
    {
        GrantRandomAmmo(NumThrows * GetRandomMultiplier());
    }

    public void GrantRandomAmmo(int totalAmmo)
    {
        int remainingAmmo = totalAmmo;
        for (int i = 0; i < NumAmmoTypes - 1; i++)
        {
            int chooseAmount = (int)(totalAmmo * (AmmoRarity[i] * (Random.Range(1 - AmmoRandomness, 1 + AmmoRandomness))));
            Player.GetComponent<AmmoInventory>().AddAmmo(i, chooseAmount);
            remainingAmmo -= chooseAmount;
        }
        Player.GetComponent<AmmoInventory>().AddAmmo(NumAmmoTypes - 1, remainingAmmo);
    }
    #endregion

    #region Coroutine
    IEnumerator FadeToPurple()
    {
        for (;;)
        {
            float t = (Time.time - m_startTime) / FadeDuration;
            foreach (SpriteRenderer renderer in BlueStop.GetComponentsInChildren<SpriteRenderer>(true))
            {
                renderer.color = new Color(1f, 1f, 1f, 1 - Mathf.SmoothStep(m_minAlpha, m_maxAlpha, t));
            }
            if (t >= m_smoothStepDuration)
            {
                BlueStop.SetActive(false);
                yield break;
            }
            yield return null;
        }
    }
    #endregion

    #region Collision Handlers
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            if(!m_used)
            {
                m_used = true;
                SpinningAnim();
                m_startTime = Time.time;
                StartCoroutine("FadeToPurple");
                GrantItems();
                refillSFX.Play();
            }
        }
    }
    #endregion
}
