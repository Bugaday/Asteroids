﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject Environment;
    public GameObject Planet;
    public Material[] PlanetMaterials;
    public Mesh[] PlanetMeshes;
    public Material[] Skyboxes;
    public GameObject PlanetRings;
    public GameObject Sun;
    public GameObject AsteroidField;


    public Asteroid[] asteroidTypes;
    public Ship NewShip;
    public UFO ufo;

    public GameObject HyperSpaceEffect;

    public GameObject ExplosionShip;
    public List<GameObject> ExplosionAsteroid;

    public int AsteroidsToSpawn = 5;
    public int ExtraLifeInterval = 40000;
    float xSpaceMin;
    float xSpaceMax;
    float ySpaceMin;
    float ySpaceMax;
    float envXSpaceMin;
    float envXSpaceMax;
    float envYSpaceMin;
    float envYSpaceMax;

    float ufoTimer = 25.0f;
    float ufoTimeToSpawn;

    public Ship CurrentShip;
    UFO CurrentUFO;

    UIManager um;
    Camera cam;

    public int lives = 3;
    public int Score = 0;
    public int extraLifeAtScore;
    bool hyperSpaceAvailable = true;


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
        xSpaceMin = cam.ViewportToWorldPoint(new Vector3(0, 0.5f, cam.transform.position.z * -1)).x;
        xSpaceMax = cam.ViewportToWorldPoint(new Vector3(1f, 0.5f, cam.transform.position.z * -1)).x;
        ySpaceMin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0, cam.transform.position.z * -1)).y;
        ySpaceMax = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, cam.transform.position.z * -1)).y;

        Cursor.visible = false;
        StartCoroutine(StartGame());
        StartCoroutine(CheckLevelOver());
    }

    #region Hyperspace

    IEnumerator StartGame()
    {
        PopulateLevel();
        yield return new WaitForSeconds(1);
        Instantiate(HyperSpaceEffect, Vector3.zero, Quaternion.identity);
        yield return new WaitForSeconds(1);
        CurrentShip = Instantiate(NewShip, Vector3.zero, Quaternion.identity);
    }

    IEnumerator NewLevel()
    {
        //yield return new WaitForSeconds(1);
        //um.LevelClearText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        Instantiate(HyperSpaceEffect, CurrentShip.transform.position, Quaternion.identity);
        Destroy(CurrentShip.gameObject);

        yield return new WaitForSeconds(3);
        //um.LevelClearText.gameObject.SetActive(false);
        foreach (Transform item in Environment.transform)
        {
            Destroy(item.gameObject);
        }

        //Reset UFO
        ufoTimeToSpawn = Time.timeSinceLevelLoad + ufoTimer;
        if (CurrentUFO) Destroy(CurrentUFO.gameObject);
        Bullet[] bulletsInScene = FindObjectsOfType<Bullet>();
        foreach (var bullet in bulletsInScene)
        {
            Destroy(bullet.gameObject);
        }
        PopulateLevel();
        yield return new WaitForSeconds(1);
        Instantiate(HyperSpaceEffect, Vector3.zero, Quaternion.identity);
        yield return new WaitForSeconds(1);
        CurrentShip = Instantiate(NewShip, Vector3.zero, Quaternion.identity);
        StartCoroutine(CurrentShip.StartInvincible());
        hyperSpaceAvailable = true;
        um.ChangeWarp(true);
        StartCoroutine(CheckLevelOver());
    }

    IEnumerator FreshShip()
    {
        yield return new WaitForSeconds(1);
        Instantiate(HyperSpaceEffect, Vector3.zero, Quaternion.identity);
        yield return new WaitForSeconds(1);
        CurrentShip = Instantiate(NewShip, Vector3.zero, Quaternion.identity);
        StartCoroutine(CurrentShip.StartInvincible());
    }

    IEnumerator HyperSpace(Vector3 newPosition)
    {
        Instantiate(HyperSpaceEffect, CurrentShip.transform.position, Quaternion.identity);
        Destroy(CurrentShip.gameObject);
        yield return new WaitForSeconds(1);
        Instantiate(HyperSpaceEffect, newPosition, Quaternion.identity);
        yield return new WaitForSeconds(1);
        CurrentShip = Instantiate(NewShip, newPosition, Quaternion.identity);
    }

    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(0);
        if (Input.GetKeyDown(KeyCode.N)) StartCoroutine(NewLevel());

        if (Input.GetButtonDown("Hyperspace"))
        {
            if (hyperSpaceAvailable)
            {
                hyperSpaceAvailable = false;
                um.ChangeWarp(false);
                Vector3 newPos = RandomPointInLevel();
                StartCoroutine(HyperSpace(newPos));
            }
        }

        if (Time.timeSinceLevelLoad > ufoTimeToSpawn)
        {
            ufoTimeToSpawn = Time.timeSinceLevelLoad + ufoTimer;
            SpawnUfo();
        }
    }

    private Vector3 RandomPointInLevel()
    {
        float newX = Random.Range(xSpaceMin, xSpaceMax);
        float newY = Random.Range(ySpaceMin, ySpaceMax);
        Vector3 newPos = new Vector3(newX, newY, 0);
        return newPos;
    }

    Vector3 RandomEnvPosition(float zMin, float zMax)
    {
        float envObjectDistance = Random.Range(zMin, zMax);
        envXSpaceMin = cam.ViewportToWorldPoint(new Vector3(0, 0.5f, cam.transform.position.z * -1 + envObjectDistance)).x;
        envXSpaceMax = cam.ViewportToWorldPoint(new Vector3(1, 0.5f, cam.transform.position.z * -1 + envObjectDistance)).x;
        envYSpaceMin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0, cam.transform.position.z * -1 + envObjectDistance)).y;
        envYSpaceMax = cam.ViewportToWorldPoint(new Vector3(0.5f, 1, cam.transform.position.z * -1 + envObjectDistance)).y;

        float newX = Random.Range(envXSpaceMin, envXSpaceMax);
        float newY = Random.Range(envYSpaceMin, envYSpaceMax);

        Vector3 newEnvPos = new Vector3(newX, newY, envObjectDistance);
        return newEnvPos;
    }

    private void PopulateLevel()
    {
        //Create random skybox and offset it
        RenderSettings.skybox = Skyboxes[Random.Range(0, Skyboxes.Length - 1)];
        float newSkyboxRotation = Random.Range(90, 270);
        RenderSettings.skybox.SetFloat("_Rotation", newSkyboxRotation);

        for (int i = 0; i < AsteroidsToSpawn; i++)
        {
            Vector3 randPos = RandomPointInLevel();
            float randRot = Random.Range(-180, 180);
            Asteroid newAsteroid = Instantiate(asteroidTypes[Random.Range(0, asteroidTypes.Length - 1)], randPos, Quaternion.Euler(0, 0, randRot));
        }

        int planetsToSpawn = Random.Range(0, 4);
        int starsToSpawn = Random.Range(0, 2);

        for (int i = 0; i < planetsToSpawn; i++)
        {
            Vector3 newPlanetRandPos = RandomEnvPosition(800f,1900f);
            Quaternion planetRandRot = Quaternion.Euler(new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180)));
            Mesh planetMesh = PlanetMeshes[Random.Range(0,PlanetMeshes.Length - 1)];
            Material newPlanetMaterial = PlanetMaterials[Random.Range(0, PlanetMaterials.Length - 1)];
            GameObject newPlanet = Instantiate(Planet, newPlanetRandPos, planetRandRot,Environment.transform);
            newPlanet.GetComponent<MeshFilter>().mesh = planetMesh;
            newPlanet.GetComponent<MeshRenderer>().material = newPlanetMaterial;
            //20% chance for planet to have rings
            if (Random.value < 0.2f)
            {
                Quaternion ringRot = Quaternion.Euler(new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180)));
                Instantiate(PlanetRings, newPlanet.transform.position, ringRot, newPlanet.transform);
            }
        }
        //80% chance to spawn a star
        if (Random.value >= 0.2f)
        {
            Vector3 newSunPos = RandomEnvPosition(1000f, 1800f);
            Instantiate(Sun, newSunPos, Quaternion.identity, Environment.transform);
        }

        //Spawn asteroid fields
        for (int i = 0; i < 3; i++)
        {
            Vector3 fieldPos = RandomEnvPosition(150, 250);
            Instantiate(AsteroidField, fieldPos, Quaternion.identity, Environment.transform);
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
            StartCoroutine(FreshShip());
        }
    }

    void SpawnUfo()
    {
        CurrentUFO = Instantiate(ufo, new Vector3(-100.0f, 0, 0), Quaternion.identity);
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
            AsteroidsToSpawn++;
            StartCoroutine(NewLevel());
        }
        else
        {
            yield return new WaitForSeconds(2);
            StartCoroutine(CheckLevelOver());
        }
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
