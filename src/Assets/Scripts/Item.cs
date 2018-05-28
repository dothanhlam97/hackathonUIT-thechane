using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Rigidbody2D body;
    private Vector3 angle;

    [SerializeField]
    public GameObject gameControllerObj;

    [SerializeField]
    private GameObject bubblePref;

    public GameController gameController;
    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.AddForce(new Vector2(-270, 120));
        angle = new Vector3(0, 0, 5);

        gameController = gameControllerObj.GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.gameState != GameController.GameState.Pause)
            body.transform.Rotate(angle);
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.gameObject.tag == "Violence")
        {
            body.AddForce(new Vector2(280, 120));

            Instantiate(bubblePref, body.transform.position, Quaternion.identity);

            this.gameObject.SetActive(false);

        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
