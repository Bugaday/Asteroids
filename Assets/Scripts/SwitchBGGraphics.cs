using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBGGraphics : MonoBehaviour
{
    Camera cam;
    GameManager gm;
    public GameObject PpVolume;
    public bool GfxOn = true;

    private void Start()
    {
        cam = Camera.main;
        gm = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchGFX();
        }
    }

    public void SwitchGFX()
    {
        if (GfxOn)
        {
            PpVolume.SetActive(false);
            RenderSettings.skybox = null;
            gm.Environment.SetActive(false);
            GfxOn = false;
        }
        else
        {
            PpVolume.SetActive(true);
            RenderSettings.skybox = gm.CurrentSkybox;
            gm.Environment.SetActive(true);
            GfxOn = true;
        }

    }
}
