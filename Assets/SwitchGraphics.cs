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
                effect.GetComponent<ParticleSystemRenderMode>() = ParticleSystemRenderMode.None;
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
            return;
        }

        TwoD.SetActive(false);
        ThreeD.SetActive(true);

    }
}
