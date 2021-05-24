using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGraphics : MonoBehaviour
{
    public GameObject ThreeD;
    public GameObject TwoD;
    bool isThreeD = true;

    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
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
        ThreeD.SetActive(false);
        TwoD.SetActive(true);
        isThreeD = false;
    }

    void SwitchToThreeD()
    {
        TwoD.SetActive(false);
        ThreeD.SetActive(true);
        isThreeD = true;
    }
}
