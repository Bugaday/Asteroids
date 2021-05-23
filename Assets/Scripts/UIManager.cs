﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    public RectTransform GameOverText;
    public RectTransform LevelClearText;
    public RectTransform LivesUI;
    public RectTransform Life;
    public RectTransform Warp;

    public Color WarpAvailable;
    public Color WarpUnavailable;

    // Start is called before the first frame update
    public void UpdateLives(int lives)
    {
        foreach (RectTransform life in LivesUI)
        {
            Destroy(life.gameObject);
        }

        for (int i = 0; i < lives; i++)
        {
            Vector2 lifePos = new Vector2(385 - i * 30,270);
            RectTransform newLife = Instantiate(Life,LivesUI);
            newLife.anchoredPosition = lifePos;
        }
    }

    public void ChangeWarp(bool isAvailable)
    {
        if (isAvailable)
        {
            Warp.GetComponent<Image>().color = WarpAvailable;
        }
        else
        {
            Warp.GetComponent<Image>().color = WarpUnavailable;
        }
    }

    // Update is called once per frame
    public void UpdateScore(int scoreAmount)
    {
        ScoreText.text = scoreAmount.ToString();
    }

    public void GameOverUI()
    {
        GameOverText.gameObject.SetActive(true);
    }
}
