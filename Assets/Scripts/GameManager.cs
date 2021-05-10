﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Asteroid[] asteroidTypes;
    public Ship NewShip;
    public UFO ufo;

    public GameObject HyperSpaceEffect;

    public GameObject ExplosionShip;
    public List<GameObject> ExplosionAsteroid;

    public int AsteroidsToSpawn = 5;
    public int ExtraLifeInterval = 40000;

    float ufoTimer = 25.0f;
    float ufoTimeToSpawn;

    public Ship CurrentShip;
    UIManager um;
    Camera cam;

    public int lives = 3;
    public int Score = 0;
    public int extraLifeAtScore;


    private void Awake()
    {
        um = FindObjectOfType<UIManager>();
        cam = Camera.main;

        ufoTimeToSpawn = ufoTimer;
        extraLifeAtScore = ExtraLifeInterval;
        CurrentShip = FindObjectOfType<Ship>();
    }

    private void Start()
    {
        Cursor.visible = false;
        StartCoroutine(NewLevel());
        StartCoroutine(CheckLevelOver());
    }

    IEnumerator NewLevel()
    {
        yield return new WaitForSeconds(2);
        for (int i = 0; i < AsteroidsToSpawn; i++)
        {
            Vector3 randPos = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), 0);
            float randRot = Random.Range(-180, 180);
            Asteroid newAsteroid = Instantiate(asteroidTypes[Random.Range(0, asteroidTypes.Length - 1)], randPos, Quaternion.Euler(0, 0, randRot));
        }
        StartCoroutine(HyperSpaceIn(Vector3.zero));
        yield return new WaitForSeconds(2);
        StartCoroutine(CurrentShip.StartInvincible());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(0);

        if(Time.timeSinceLevelLoad > ufoTimeToSpawn)
        {
            ufoTimeToSpawn = Time.timeSinceLevelLoad + ufoTimer;
            SpawnUfo();
        }
    }

    public void ShipDestroyed()
    {
        lives--;
        um.UpdateLives(lives);
        if(lives <= 0)
        {
            StartCoroutine(GameOver());
        }
        else
        {
            StartCoroutine(HyperSpaceIn(Vector3.zero));
            StartCoroutine(CurrentShip.StartInvincible());
        }
    }

    void SpawnUfo()
    {
        Instantiate(ufo, new Vector3(-100.0f, 0, 0), Quaternion.identity);
    }

      public void AddScore(int ScoreToAdd)
    {
        Score += ScoreToAdd;
        if(Score >= extraLifeAtScore)
        {
            lives++;
            um.UpdateLives(lives);
            extraLifeAtScore = Score + ExtraLifeInterval;
        }

        um.UpdateScore(Score);
    }

    //Recursive function that's constantly checking how many asteroids are left
    IEnumerator CheckLevelOver()
    {
        int asteroidsLeft = FindObjectsOfType<Asteroid>().Length;
        if (asteroidsLeft <= 0)
        {
            HyperSpaceOut();
            AsteroidsToSpawn++;
            StartCoroutine(NewLevel());
        }
        yield return new WaitForSeconds(2);
        StartCoroutine(CheckLevelOver());
    }

    void HyperSpaceOut()
    {
        Destroy(CurrentShip);
        Instantiate(HyperSpaceEffect, CurrentShip.transform.position, Quaternion.identity);
    }

    IEnumerator HyperSpaceIn(Vector3 newPosition)
    {
        Instantiate(HyperSpaceEffect, CurrentShip.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1);
        CurrentShip = Instantiate(NewShip, newPosition, Quaternion.identity);
    }

    IEnumerator GameOver()
    {
        Time.timeScale = 0;
        um.GameOverUI();
        yield return new WaitForSecondsRealtime(3);
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
