using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGraphics : MonoBehaviour
{
    public GameObject ThreeD;
    public GameObject TwoD;
    public bool IsParticleEffect = false;

    bool isThreeD = true;

    GameManager gm;

    public ParticleSystemRenderer[] pRenderSystems3D;
    public ParticleSystemRenderMode[] originalRenderModes3D;

    public ParticleSystemRenderer[] pRenderSystems2D;
    public ParticleSystemRenderMode[] originalRenderModes2D;

    // Start is called before the first frame update
    void Awake()
    {
        gm = FindObjectOfType<GameManager>();

        if (IsParticleEffect)
        {
            GetParticleSystems();
        }

        if (gm.IsInThreeD)
        {
            SwitchToThreeD();
        }
        else
        {
            SwitchToTwoD();
        }
    }

    void GetParticleSystems()
    {
        pRenderSystems3D = ThreeD.GetComponentsInChildren<ParticleSystemRenderer>();
        originalRenderModes3D = new ParticleSystemRenderMode[pRenderSystems3D.Length];

        for (int i = 0; i < pRenderSystems3D.Length; i++)
        {
            originalRenderModes3D[i] = pRenderSystems3D[i].renderMode;
        }

        pRenderSystems2D = TwoD.GetComponentsInChildren<ParticleSystemRenderer>();
        originalRenderModes2D = new ParticleSystemRenderMode[pRenderSystems2D.Length];

        for (int i = 0; i < pRenderSystems2D.Length; i++)
        {
            originalRenderModes2D[i] = pRenderSystems2D[i].renderMode;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (isThreeD)
            {
                SwitchToTwoD();
            }
            else
            {
                SwitchToThreeD();
            }
        }
    }

    void SwitchToTwoD()
    {
        isThreeD = false;

        if (IsParticleEffect)
        {
            ParticleEffectsOff();
            return;
        }

        ThreeD.SetActive(false);
        TwoD.SetActive(true);
    }

    void SwitchToThreeD()
    {
        isThreeD = true;

        if (IsParticleEffect)
        {
            ParticleEffectsOn();
            return;
        }

        TwoD.SetActive(false);
        ThreeD.SetActive(true);
    }

    void ParticleEffectsOn()
    {
        for (int i = 0; i < pRenderSystems3D.Length; i++)
        {
            pRenderSystems3D[i].renderMode = originalRenderModes3D[i];
        }

        foreach (ParticleSystemRenderer render2D in pRenderSystems2D)
        {
            render2D.renderMode = ParticleSystemRenderMode.None;
        }
    }

    void ParticleEffectsOff()
    {
        for (int i = 0; i < pRenderSystems2D.Length; i++)
        {
            pRenderSystems2D[i].renderMode = originalRenderModes2D[i];
        }

        foreach (ParticleSystemRenderer render3D in pRenderSystems3D)
        {
            render3D.renderMode = ParticleSystemRenderMode.None;
        }
    }


}
