using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDebris : MonoBehaviour
{
    float timeSpawned = 0;
    public float timeToDie = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform piece in transform)
        {
            piece.position = transform.position + new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0).normalized;
            Vector2 directionOfPiece = (piece.position - transform.position).normalized;

            Rigidbody2D pieceRb = piece.GetComponent<Rigidbody2D>();

            pieceRb.AddForce(directionOfPiece * 1000);
            pieceRb.AddTorque(Random.Range(-200, 200));
        }
    }

    private void Update()
    {
        timeSpawned += Time.deltaTime;
        if(timeSpawned > timeToDie)
        {
            Destroy(gameObject);
        }
    }
}
