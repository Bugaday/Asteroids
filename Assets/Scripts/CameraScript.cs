using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    GameManager gm;
    UIManager um;
    Vector3 vel;
    Camera cam;

    float scrnWidth;
    float scrnHeight;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        um = FindObjectOfType<UIManager>();
        cam = Camera.main;

        FourByThreeAspect();
    }

    private void FourByThreeAspect()
    {
        scrnWidth = Screen.width;
        scrnHeight = Screen.height;

        //If, on the rare occasion that width is thinner than height, calculate 4:3 with this
        if(scrnWidth < scrnHeight)
        {
            //um.UIWrapper.GetComponent<AspectRatioFitter>().aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
            um.canvasScaler.matchWidthOrHeight = 0f;

            //If width is thinner than height, find number of pixels in screen height required for 4:3
            float fourByThreePixelsHeight = (scrnWidth / 4) * 3;

            float fourByThreeHeight = fourByThreePixelsHeight / scrnHeight;

            float yPos = (1 - fourByThreeHeight) / 2;

            cam.rect = new Rect(0f, yPos, 1f, fourByThreeHeight);

            return;
        }

        //um.UIWrapper.GetComponent<AspectRatioFitter>().aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
        um.canvasScaler.matchWidthOrHeight = 1f;

        //Find number of pixels in screen width required for 4:3
        float fourByThreePixels = (scrnHeight / 3) * 4;


        //Calculate 4:3 ratio
        float fourByThree = fourByThreePixels / scrnWidth;

        //New X position (as a ratio) of camera
        //Take width ratio, find remaining on either side then divide by 2
        float xPos = (1 - fourByThree) / 2;
        cam.rect = new Rect(xPos, 0f, fourByThree, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.CurrentShip)
        {
            Vector3 shipDir = gm.CurrentShip.rb.velocity.normalized;
            Vector2 shipDir2D = gm.CurrentShip.rb.velocity.normalized;

            Vector3 targetPos = new Vector3(shipDir2D.x * 10, shipDir2D.y * 10, transform.position.z);

            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref vel, 1f);
        }

        if(Screen.width != scrnWidth || Screen.height != scrnHeight)
        {
            FourByThreeAspect();
        }
    }
}
