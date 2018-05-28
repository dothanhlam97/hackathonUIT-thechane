using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Violence : MonoBehaviour
{
    [SerializeField]
    private Slider sidlerBar;

    [SerializeField]
    private float moveSpeed = 0.001f;

    [SerializeField]
    public float cntValue;

    public static string PARAM_STUN = "onViolenceIdle";
    private Animator animator;

    public enum State { Move, Stun, Attack };

    float stunTime;
    private State state;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        Application.targetFrameRate = 30;

        initValue();
        initPosition();
    }

    void initValue()
    {
        moveSpeed = (float)0.015f;
        stunTime = (float)1f;
        cntValue = (float)2f;
    }

    void initPosition()
    {
        float width = Screen.width / 2;
        float height = Screen.height / 2;

        Vector3 vt = Camera.main.ScreenToWorldPoint(new Vector3(width, height, 1));

        transform.position = vt;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (state == State.Stun)
        {
            stunTime -= Time.deltaTime;
        }

        updateWithState();
    }

    void updateWithState()
    {
        switch (state)
        {
            case State.Move:
                {
                    handleMove();
                    break;
                }
            case State.Stun:
                {
                    handleStun();
                    break;
                }
        }
    }

    void handleMove()
    {
        Vector3 vt = transform.position;

        vt.x -= moveSpeed;
        Debug.Log(vt.x);

        transform.position = vt;
    }

    void handleStun()
    {
        if (stunTime <= 0)
        {
            state = State.Move;

            animator.SetTrigger("onTriggerWalk");
        }
    }

    public void OnTriggerEnter2D(Collider2D target)
    {

        if (target.gameObject.tag == "Item")
        {
            onTriggerWithItem();
        }
        else if (target.gameObject.tag == "Actor")
        {
            onTriggerWithActor();
        }
    }

    public void onTriggerWithItem()
    {
        switch (state)
        {
            case State.Move:
                {
                    state = State.Stun;

                    animator.SetTrigger("onTriggerIdle");
                    stunTime = 1f;
                    break;
                }
            case State.Stun:
                {
                    stunTime = 1f;
                    break;
                }
            case State.Attack:
                {
                    break;
                }
        }

        sidlerBar.value += cntValue;
    }

    public void onTriggerWithActor()
    {

    }
}
