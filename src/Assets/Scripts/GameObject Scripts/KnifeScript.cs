using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeScript : MonoBehaviour {
    [SerializeField]
    private float speed;

    private Rigidbody2D body;
    private Animator animator;
    private Vector3 angle;

    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody2D>();
        body.AddForce(new Vector2(-200, 80));
        angle = new Vector3(0, 0, 20);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        body.transform.Rotate(angle);
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.gameObject.tag == "Monster")
        {
            body.AddForce(new Vector2(240, 90));
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
