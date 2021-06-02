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

    // Start is called before the first frame update
    void Awake()
    {
        gm = FindObjectOfType<GameManager>();

        if (gm.IsInThreeD)
        {
            SwitchToThreeD();
        }
        else
        {
            SwitchToTwoD();
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
            foreach (Transform effect in ThreeD.transform)
            {
                ParticleSystemRenderer effectRenderer = effect.GetComponent<ParticleSystemRenderer>();
                effectRenderer.renderMode = ParticleSystemRenderMode.None;

                var subEmitter = effect.GetComponent<ParticleSystem>().subEmitters;
                subEmitter.enabled = false;
            }
            foreach (Transform effect in TwoD.transform)
            {
                ParticleSystemRenderer effectRenderer = effect.GetComponent<ParticleSystemRenderer>();
                effectRenderer.renderMode = ParticleSystemRenderMode.Billboard;
            }
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
            foreach (Transform effect in TwoD.transform)
            {
                ParticleSystemRenderer effectRenderer = effect.GetComponent<ParticleSystemRenderer>();
                effectRenderer.renderMode = ParticleSystemRenderMode.None;
            }
            foreach (Transform effect in ThreeD.transform)
            {
                ParticleSystemRenderer effectRenderer = effect.GetComponent<ParticleSystemRenderer>();
                effectRenderer.renderMode = ParticleSystemRenderMode.Billboard;

                var subEmitter = effect.GetComponent<ParticleSystem>().subEmitters;
                subEmitter.enabled = true;
            }
            return;
        }

        TwoD.SetActive(false);
        ThreeD.SetActive(true);

    }
}
