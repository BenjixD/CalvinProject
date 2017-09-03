using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour {

    #region Public Members
    public GameObject[] PlatformLayouts;
    public float TranslucentPlatformAlpha;
    public float TranslucentWaitTime;
    public float LayoutDuration;                    // How long a layout lasts. Must be greater than
                                                    // TranscluentWaitTime.
    public float LayoutlessDuration;                // Time between layout being destroyed and the next being
                                                    // spawned. Must be greater than LayoutFadeDuration.

    public float LayoutFadeDuration;
    public float DeactivateCollidersDelay;          // How long the platforms' colliders persist as they fade.
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
        foreach (SpriteRenderer renderer in layout.GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.color = new Color(1f, 1f, 1f, TranslucentPlatformAlpha);
        }
        DeactivateLayoutColliders(layout);
    }

    void DeactivateLayoutColliders(GameObject layout)
    {
        foreach (EdgeCollider2D collider in layout.GetComponentsInChildren<EdgeCollider2D>())
        {
            collider.enabled = false;
        }
    }

    void ActivateLayout(GameObject layout)
    {
        foreach (SpriteRenderer renderer in layout.GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.color = new Color(1f, 1f, 1f, 1f);
        }
        foreach (EdgeCollider2D collider in layout.GetComponentsInChildren<EdgeCollider2D>())
        {
            collider.enabled = true;
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
                renderer.color = new Color(1f, 1f, 1f, 1 - Mathf.SmoothStep(m_minAlpha, m_maxAlpha, t));
            }
            if (t >= DeactivateCollidersDelay)
            {
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
