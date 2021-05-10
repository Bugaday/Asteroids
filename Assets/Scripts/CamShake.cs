using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    #region Variables
    public bool camShakeActive = false;
    [Range(0,1)]
    [SerializeField]
    float shakeStrength = 0.8f;
    [SerializeField]
    float strengthMult = 0.5f;
    [SerializeField]
    float strengthMag = 0.7f;
    [SerializeField]
    float rotMag = 10f;
    [SerializeField]
    float shakeDecay = 1.5f;

    float timeCounter;
    #endregion


    #region Accessors

    public float ShakeStrength
    {
        get
        {
            return shakeStrength;
        }
        set
        {
            shakeStrength = Mathf.Clamp01(value);
        }
    }
    #endregion

    float GetPerlinFloat(float seed)
    {
        return Mathf.PerlinNoise(seed, timeCounter - 0.5f) * 2;
    }

    Vector3 GetVec3()
    {
        return new Vector3(GetPerlinFloat(1), GetPerlinFloat(10),0);
    }

    // Update is called once per frame
    void Update()
    {
        if (camShakeActive)
        {
            timeCounter += Time.deltaTime * Mathf.Pow(shakeStrength,0.3f) * strengthMult;
            Vector3 newPos = GetVec3() * strengthMag;
            transform.localPosition = newPos;
            transform.localRotation = Quaternion.Euler(newPos * rotMag);

            ShakeStrength -= Time.deltaTime * shakeDecay;
        }
        else
        {
            Vector3 vel = new Vector3();
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero,ref vel, 1);
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.identity, 1);
        }
    }
}
