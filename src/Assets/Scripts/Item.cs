using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Rigidbody2D body;
    private Vector3 angle;

    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.AddForce(new Vector2(-250, 80));
        angle = new Vector3(0, 0, 20);
    }

    // Update is called once per frame
    void Update()
    {
        body.transform.Rotate(angle);
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.gameObject.tag == "Violence")
        {
            body.AddForce(new Vector2(280, 90));
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
