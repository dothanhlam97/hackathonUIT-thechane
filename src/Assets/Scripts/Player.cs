using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using PDollarGestureRecognizer;
using SketchDetection;

public class Player : MonoBehaviour
{
    public static string THROW_TRIGGER = "triggerPlayerThrow";

    [SerializeField]
    private GameObject familyPrefabs;

    [SerializeField]
    private GameObject friendPrefabs;

    [SerializeField]
    private GameObject lovePrefabs;

    [SerializeField]
    private GameObject socialPrefabs;

    [SerializeField]
    private GameObject talkPrefabs;

    GameObject[] prefabs;

    private Animator animator;

    [SerializeField]
    public GameObject gameControllerObj;

    public GameController gameController;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        initPosition();

        gameController = gameControllerObj.GetComponent<GameController>();

        prefabs = new GameObject[5];
        prefabs[0] = lovePrefabs;
        prefabs[1] = familyPrefabs;
        prefabs[2] = socialPrefabs;
        prefabs[3] = friendPrefabs;
        prefabs[4] = talkPrefabs;
    }

    void initPosition()
    {
        float width = Screen.width - 50;
        float height = 0;

        Vector3 vt = Camera.main.ScreenToWorldPoint(new Vector3(width, height, 1));
        vt.x -= 1f;
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
        //handleMouseEvent();
    }


    void handleCountDownState()
    {
        animator.enabled = false;
    }

    void handlePlayState()
    {
        if (animator.enabled == false)
        {
            animator.enabled = true;
        }

        abc();
    }


    public void handleThrowItem()
    {

        onThrow = true;
        cntDelay = 0;

        //Vector3 vt = transform.position;
        //vt.x += 5f;
        //vt.y += 1f;

        //Instantiate(itemPrefabs, vt, Quaternion.identity);
    }


    bool onThrow = false;

    float cntDelay = 0f;
    void abc()
    {
        if (onThrow)
        {
            if (cntDelay <= 0.005f)
            {
                animator.SetTrigger(THROW_TRIGGER);
            }

            cntDelay += Time.deltaTime;

            if (cntDelay >= 0.5f)
            {
                int idx = Random.Range((int)0, (int)4);
                Debug.Log(idx);
                Instantiate(prefabs[idx], transform.position, Quaternion.identity);
                onThrow = false;
            }
        }
    }

}
