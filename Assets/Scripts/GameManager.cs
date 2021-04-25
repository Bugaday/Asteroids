using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Asteroid[] asteroidTypes;
    public Ship NewShip;
    public UFO ufo;

    public int AsteroidsToSpawn = 5;
    public int ExtraLifeInterval = 40000;

    float ufoTimer = 25;
    float ufoTimeToSpawn;

    Ship CurrentShip;
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
    }

    private void Start()
    {
        CurrentShip = FindObjectOfType<Ship>();
        NewLevel();
        StartCoroutine(CheckLevelOver());
    }

    private void NewLevel()
    {
        for (int i = 0; i < AsteroidsToSpawn; i++)
        {
            Vector3 randPos = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
            float randRot = Random.Range(-180, 180);

            Asteroid newAsteroid = Instantiate(asteroidTypes[Random.Range(0, asteroidTypes.Length - 1)], randPos, Quaternion.Euler(0, 0, randRot));
        }

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
            StartCoroutine(RespawnShip());
        }
    }

    void SpawnUfo()
    {
        Instantiate(ufo, cam.ViewportToWorldPoint(new Vector3(0, 0.5f, cam.nearClipPlane)), Quaternion.identity);
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

    IEnumerator CheckLevelOver()
    {
        print("Checking Level");
        int asteroidsLeft = FindObjectsOfType<Asteroid>().Length;
        print("Asteroids left: " + asteroidsLeft);
        if (asteroidsLeft <= 0)
        {
            print("No asteroids found");
            AsteroidsToSpawn++;
            NewLevel();
        }
        yield return new WaitForSeconds(2);
        StartCoroutine(CheckLevelOver());
    }

    IEnumerator RespawnShip()
    {
        yield return new WaitForSeconds(2);
        CurrentShip = Instantiate(NewShip, Vector3.zero, Quaternion.identity);
        StartCoroutine(CurrentShip.StartInvincible());
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
