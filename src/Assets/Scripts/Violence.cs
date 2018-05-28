using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Violence : MonoBehaviour
{

    public enum State { Move, Stun, Attack };
    public float moveSpeed = 2f;

    private State state;
    // Use this for initialization
    void Start()
    {

    }

    void Update() {
        if (Input.GetKey(KeyCode.Space)) { 
            state = State.Attack;
        } else {
            state = State.Move;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        onMove();
    }

    void onMove()
    {
        
        switch (state)
        {
            case State.Move:
                {
                    Vector3 vt = transform.position;
                    vt.x -= moveSpeed * Time.deltaTime;
                    transform.position = vt;
                    break;
                }
            case State.Attack:
                {
                    break;
                }
            case State.Stun:
                {
                    break;
                }
        }
    }

    public void OnTriggerEnter(Collider target)
    {
        
    }

    public void onTriggerWithItem()
    {
        switch (state)
        {
            case State.Move:
                {
                    break;
                }
            case State.Stun:
                {
                    break;  
                }
            case State.Attack:
                {
                    break;
                }
        }
    }
}
