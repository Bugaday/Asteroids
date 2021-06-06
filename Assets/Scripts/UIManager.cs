using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public CanvasScaler canvasScaler;
    public RectTransform UIWrapper;
    public TextMeshProUGUI ScoreText;
    public RectTransform GameOverText;
    public RectTransform LevelClearText;
    public RectTransform LivesUI;
    public RectTransform Life;
    public RectTransform Warp;
    public RectTransform PauseScreen;
    public RectTransform ControlsScreen;
    public RectTransform QuitScreen;

    public Texture2D CursorSprite;

    public Color WarpAvailable;
    public Color WarpUnavailable;

    GameManager gm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        Cursor.SetCursor(CursorSprite, Vector2.zero, CursorMode.Auto);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(PauseScreen.gameObject.activeInHierarchy == false)
            {
                PauseScreenActive();
            }
            else
            {
                Resume();
            }
        }
    }

    // Start is called before the first frame update
    public void AddLife()
    {
        //ReCreateLives(lives);

        Life lastLife = LivesUI.GetChild(LivesUI.childCount - 1).GetComponent<Life>();


        Vector2 lifePos = new Vector2(lastLife.GetComponent<RectTransform>().anchoredPosition.x - 30, 0);
        RectTransform newLife = Instantiate(Life, LivesUI);
        newLife.anchorMax = Vector2.one;
        newLife.anchorMin = Vector2.one;
        newLife.pivot = Vector2.one;
        newLife.anchoredPosition = lifePos;

        newLife.GetComponent<Life>().GainLife();
    }

    public void RemoveLife()
    {
        //ReCreateLives(lives);

        LivesUI.GetChild(LivesUI.childCount - 1).GetComponent<Life>().LoseLife();
    }

    private void ReCreateLives(int lives)
    {
        foreach (RectTransform life in LivesUI)
        {
            Destroy(life.gameObject);
        }

        for (int i = 0; i < lives; i++)
        {
            Vector2 lifePos = new Vector2(-i * 30, 0);
            RectTransform newLife = Instantiate(Life, LivesUI);
            newLife.anchorMax = Vector2.one;
            newLife.anchorMin = Vector2.one;
            newLife.pivot = Vector2.one;
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

    public void PauseScreenActive()
    {
        PauseScreen.gameObject.SetActive(true);
        ControlsScreenActive();
        Time.timeScale = 0;
        gm.IsGamePaused = true;
        Cursor.visible = true;
        if(gm.CurrentShip) gm.CurrentShip.Reticle.SetActive(false);
    }

    public void ControlsScreenActive()
    {
        QuitScreen.gameObject.SetActive(false);
        ControlsScreen.gameObject.SetActive(true);
    }

    public void QuitScreenActive()
    {
        QuitScreen.gameObject.SetActive(true);
        ControlsScreen.gameObject.SetActive(false);
    }

    public void Resume()
    {
        QuitScreen.gameObject.SetActive(false);
        ControlsScreen.gameObject.SetActive(true);
        PauseScreen.gameObject.SetActive(false);
        Time.timeScale = 1;
        gm.IsGamePaused = false;
        Cursor.visible = false;
        if (gm.CurrentShip) gm.CurrentShip.Reticle.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
