using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{

    private Animator animator;
    // Use this for initialization

    [SerializeField]
    public GameObject gameControllerObj;

    public GameController gameController;

    void Start()
    {
        animator = GetComponent<Animator>();

        initPosition();

        gameController = gameControllerObj.GetComponent<GameController>();
    }

    void initPosition()
    {
        Vector3 vt = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 1));

        vt.x += 2f;

        vt.y += 5f;

        transform.position = vt;
    }
    // Update is called once per frame
    void Update()
    {
        switch (gameController.gameState)
        {
            case GameController.GameState.Countdown:
                {
                    animator.enabled = false;
                    break;
                }
            case GameController.GameState.Play:
                {
                    if (animator.enabled == false)
                    {
                        animator.enabled = true;
                    }

                    break;
                }
        }
    }


}
