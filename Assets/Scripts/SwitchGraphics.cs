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
            ParticleEffectsOn(TwoD);
            ParticleEffectsOff(ThreeD);
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
            ParticleEffectsOn(ThreeD);
            ParticleEffectsOff(TwoD);
            return;
        }

        TwoD.SetActive(false);
        ThreeD.SetActive(true);
    }

    void ParticleEffectsOn(GameObject obj)
    {
        foreach (Transform effect in obj.transform)
        {
            ParticleSystemRenderer effectRenderer = effect.GetComponent<ParticleSystemRenderer>();
            effectRenderer.renderMode = ParticleSystemRenderMode.Billboard;

            ParticleEffectsOn(effect.gameObject);
        }
    }

    void ParticleEffectsOff(GameObject obj)
    {
        foreach (Transform effect in obj.transform)
        {
            ParticleSystemRenderer effectRenderer = effect.GetComponent<ParticleSystemRenderer>();
            effectRenderer.renderMode = ParticleSystemRenderMode.None;

            ParticleEffectsOff(effect.gameObject);
        }
    }


}
