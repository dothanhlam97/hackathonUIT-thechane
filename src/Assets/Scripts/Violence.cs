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

    [SerializeField]
    public GameObject gameControllerObj;

    public GameController gameController;
    // Use this for initialization
    void Start()
    {
        gameController = gameControllerObj.GetComponent<GameController>();

        animator = GetComponent<Animator>();

        initValue();
        initPosition();
    }

    void initValue()
    {
        moveSpeed = (float)0.01f;
        stunTime = (float)2f;
        cntValue = (float)10f;
    }

    void initPosition()
    {
        float width = Screen.width / 2;
        float height = 0;

        Vector3 vt = Camera.main.ScreenToWorldPoint(new Vector3(width, height, 1));
        vt.x += 3f;
        vt.y += 7f;

        transform.position = vt;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (gameController.gameState)
        {
            case GameController.GameState.Countdown:
                {
                    handleCountDownState();
                    break;
                }
            case GameController.GameState.Play:
                {
                    handlePlayState();
                    break;
                }
            case GameController.GameState.Pause:
                {
                    break;
                }
            case GameController.GameState.Lose:
                {
                    break;
                }

            case GameController.GameState.Win:
                {
                    break;
                }
        }

    }

    void handleCountDownState()
    {
        if (animator.enabled == true)
        {
            animator.enabled = false;
        }
    }

    void handlePlayState()
    {
        if (animator.enabled == false)
        {
            animator.enabled = true;
        }

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

    void handlePauseGame()
    {

    }

    void handleMove()
    {
        Vector3 vt = transform.position;

        vt.x -= moveSpeed;
        //Debug.Log(vt.x);

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
        sidlerBar.value += cntValue;

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
    }

    public void onTriggerWithActor()
    {

    }
}
