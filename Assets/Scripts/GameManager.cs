using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Material[] PlanetMaterials;
    public Mesh[] PlanetMeshes;
    public Material[] Skyboxes;
    public GameObject PlanetRings;


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

    float ufoTimer = 25.0f;
    float ufoTimeToSpawn;

    public Ship CurrentShip;

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

        print(xSpaceMin + " , " + xSpaceMax + " , " + ySpaceMin + " , " + ySpaceMax);

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
        yield return new WaitForSeconds(1);
        um.LevelClearText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        Instantiate(HyperSpaceEffect, CurrentShip.transform.position, Quaternion.identity);
        Destroy(CurrentShip.gameObject);
        yield return new WaitForSeconds(3);
        um.LevelClearText.gameObject.SetActive(false);
        PopulateLevel();
        yield return new WaitForSeconds(1);
        Instantiate(HyperSpaceEffect, Vector3.zero, Quaternion.identity);
        yield return new WaitForSeconds(1);
        CurrentShip = Instantiate(NewShip, Vector3.zero, Quaternion.identity);
        StartCoroutine(CurrentShip.StartInvincible());
        hyperSpaceAvailable = true;
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
                //hyperSpaceAvailable = false;
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

    private void PopulateLevel()
    {
        RenderSettings.skybox = Skyboxes[Random.Range(0, Skyboxes.Length - 1)];
        for (int i = 0; i < AsteroidsToSpawn; i++)
        {
            Vector3 randPos = RandomPointInLevel();
            float randRot = Random.Range(-180, 180);
            Asteroid newAsteroid = Instantiate(asteroidTypes[Random.Range(0, asteroidTypes.Length - 1)], randPos, Quaternion.Euler(0, 0, randRot));
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
