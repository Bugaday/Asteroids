using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    GameManager gm;
    UIManager um;
    AudioManager am;
    CapsuleCollider2D capsuleCol;
    Camera cam;

    public GameObject ThreeD;
    public GameObject TwoD;

    Vector3 rotAxis3D;
    public float MinRotSpeed = 30f;
    public float MaxRotSpeed = 50f;

    public int stage = 1;
    int chunks = 2;
    LayerMask bulletMask;

    private void Awake()
    {
        capsuleCol = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        um = FindObjectOfType<UIManager>();
        am = FindObjectOfType<AudioManager>();
        cam = Camera.main;

        Vector2 randDir = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100)).normalized;
        GetComponent<Rigidbody2D>().AddForce(randDir * 600);
        GetComponent<Rigidbody2D>().AddTorque(Random.Range(-9, 9));
        bulletMask = LayerMask.NameToLayer("Bullet");

        MaxRotSpeed = Random.Range(MinRotSpeed, MaxRotSpeed);
        rotAxis3D = Random.rotation.eulerAngles;
    }

    private void Update()
    {
        ThreeD.transform.rotation = Quaternion.AngleAxis(Time.timeSinceLevelLoad * MaxRotSpeed, rotAxis3D);
        TwoD.transform.Rotate(0, 0, MaxRotSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == bulletMask) DestroyAndBreakup(collision.gameObject);
    }

    void DestroyAndBreakup(GameObject collided)
    {
        gm.AddScore(stage * 10);

        if (stage >= 3)
        {
            DestroyAsteroid();
        }
        else
        {
            for (int i = 0; i < chunks; i++)
            {
                Asteroid asteroidType = gm.asteroidTypes[Random.Range(0, gm.asteroidTypes.Length - 1)];
                float newRot = Random.Range(-180, 180);

                Asteroid newChunk = Instantiate(asteroidType, transform.position, Quaternion.Euler(0, 0, newRot));
                newChunk.stage = stage + 1;

                //Scale both 3D and 2D children down
                foreach (Transform child in newChunk.transform)
                {
                    child.localScale /= 2;
                }

                newChunk.capsuleCol.size /= 2;

                float newRandRot = Random.Range(collided.transform.eulerAngles.z + 30, collided.transform.eulerAngles.z + 150);
                float xDir = Mathf.Cos(newRandRot * Mathf.Deg2Rad);
                float yDir = Mathf.Sin(newRandRot * Mathf.Deg2Rad);

                Vector2 newChunkDir =  new Vector2(xDir,yDir);

                GetComponent<Rigidbody2D>().AddTorque(Random.Range(-750 * newChunk.stage, 750 * newChunk.stage));
                newChunk.GetComponent<Rigidbody2D>().AddForce(newChunkDir * 500 * newChunk.stage);


            }
            DestroyAsteroid();
        }
    }

    private void DestroyAsteroid()
    {
        Instantiate(gm.ExplosionAsteroid[stage-1], transform.position, Quaternion.identity);
        am.Play("Explosion");
        cam.GetComponent<CamShake>().ShakeStrength = 1 / (float)stage;
        cam.GetComponent<CamShake>().camShakeActive = true;
        Destroy(gameObject);
    }
}
