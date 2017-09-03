using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour {

    #region Public Members
    public GameObject Player;
    public GameObject[] PlatformLayouts;
    public float TranslucentPlatformAlpha;
    public float TranslucentWaitTime;
    public float LayoutDuration;                    // How long a layout lasts. Must be greater than
                                                    // TranscluentWaitTime.
    public float LayoutlessDuration;                // Time between layout being destroyed and the next being
                                                    // spawned. Must be greater than LayoutFadeDuration.

    public float LayoutFadeDuration;
    public float DeactivateCollidersDelay;          // How long the platforms' colliders persist as they fade.

    public float PokestopSpawnProbability;          // Chance that an individual Pokestop will spawn.
    #endregion

    #region Private Members
    private GameObject m_currentLayout;
    private float m_minAlpha = 0f;
    private float m_maxAlpha = 1f;
    private float m_fadeStartTime;
    private float m_smoothStepDuration = 1f;
    #endregion

    // Use this for initialization
    void Start() {
        StartCoroutine(CyclePlatformLayouts());
    }

    // Update is called once per frame
    void Update() {

    }

    #region Helper Functions
    int GetRandomLayoutIndex()
    {
        return Random.Range(0, PlatformLayouts.Length);
    }

    void PrepareLayout(GameObject layout)
    {
        ChoosePokestops();
        foreach (SpriteRenderer renderer in layout.GetComponentsInChildren<SpriteRenderer>(true))
        {
            renderer.color = new Color(1f, 1f, 1f, TranslucentPlatformAlpha);
        }
        DeactivateLayoutColliders(layout);
    }

    void DeactivateLayoutColliders(GameObject layout)
    {
        foreach (Transform platform in layout.GetComponentsInChildren<Transform>())
        {
            if (platform.GetComponent<EdgeCollider2D>() != null)
            {
                platform.GetComponent<EdgeCollider2D>().enabled = false;
            }
            // Disable Pokestop colliders
            if (platform.GetComponent<CircleCollider2D>() != null)
            {
                platform.GetComponent<CircleCollider2D>().enabled = false;
            }
            if (platform.GetComponent<BoxCollider2D>() != null)
            {
                platform.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }

    void ActivateLayout(GameObject layout)
    {
        foreach (SpriteRenderer renderer in layout.GetComponentsInChildren<SpriteRenderer>(true))
        {
            renderer.color = new Color(1f, 1f, 1f, 1f);
        }
        foreach (Transform platform in layout.GetComponentsInChildren<Transform>())
        {
            if (platform.GetComponent<EdgeCollider2D>() != null)
            {
                platform.GetComponent<EdgeCollider2D>().enabled = true;
            }
            // Enable Pokestop colliders
            if (platform.GetComponent<CircleCollider2D>() != null)
            {
                platform.GetComponent<CircleCollider2D>().enabled = true;
            }
            if (platform.GetComponent<BoxCollider2D>() != null)
            {
                platform.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
        SetPokestopsKinematic(false);
    }

    void ChoosePokestops()
    {
        foreach (Transform platform in m_currentLayout.transform)
        {
            foreach (Transform stop in platform)
            {
                if (Random.Range(0f, 1f) < PokestopSpawnProbability)
                {
                    stop.GetComponent<Rigidbody2D>().isKinematic = true;
                    stop.GetComponent<PokestopBehaviour>().SetPlayerReference(Player);
                    stop.gameObject.SetActive(true);
                }
            }
        }
    }

    // Change Pokestops' Kinematic to prevent them from falling to the ground when platforms disappear
    void SetPokestopsKinematic(bool setTo)
    {
        foreach (Transform platform in m_currentLayout.transform)
        {
            if (platform.name == "PokestopObject")
            {
                platform.GetComponent<Rigidbody2D>().isKinematic = setTo;
            }
        }
    }
    #endregion

    #region Coroutines
    IEnumerator CyclePlatformLayouts()
    {
        for (;;)
        {
            yield return new WaitForSeconds(LayoutlessDuration);
            StartCoroutine(SpawnPlatformLayout(GetRandomLayoutIndex()));
            yield return new WaitForSeconds(LayoutDuration);
            m_fadeStartTime = Time.time;
            StartCoroutine(FadeOutPlatformLayout());
        }
    }

    IEnumerator SpawnPlatformLayout(int layoutIndex)
    {
        Debug.Log("Begin Spawn");

        m_currentLayout = Instantiate(PlatformLayouts[layoutIndex]);
        PrepareLayout(m_currentLayout);
        yield return new WaitForSeconds(TranslucentWaitTime);
        ActivateLayout(m_currentLayout);
        yield return null;
    }

    IEnumerator FadeOutPlatformLayout()
    {
        Debug.Log("Begin Fadeout");

        for (;;)
        {
            float t = (Time.time - m_fadeStartTime) / LayoutFadeDuration;
            foreach (SpriteRenderer renderer in m_currentLayout.GetComponentsInChildren<SpriteRenderer>(true))
            {
                // Ignore alpha chance if the current alpha is already less than what you want to change it to
                float newAlpha = 1 - Mathf.SmoothStep(m_minAlpha, m_maxAlpha, t);
                if (renderer.color.a > newAlpha)
                {
                    renderer.color = new Color(1f, 1f, 1f, newAlpha);
                }
            }
            if (t >= DeactivateCollidersDelay)
            {
                SetPokestopsKinematic(true);
                DeactivateLayoutColliders(m_currentLayout);
            }
            if (t >= m_smoothStepDuration)
            {
                Destroy(m_currentLayout);
                yield break;
            }
            yield return null;
        }
    }
    #endregion

}
