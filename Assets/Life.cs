using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{

    Animator anim;
    public AnimationClip GainLifeClip;
    public AnimationClip LoseLifeClip;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void GainLife()
    {
        anim.Play("GainLife");
    }

    public void LoseLife()
    {
        anim.Play("LoseLife");
    }

    void DestroyObj()
    {
        Destroy(gameObject);
    }
}
