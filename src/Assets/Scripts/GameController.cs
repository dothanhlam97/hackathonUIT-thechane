using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using PDollarGestureRecognizer;
using SketchDetection;


public class GameController : MonoBehaviour
{
    public static string THROW_TRIGGER = "triggerPlayerThrow";

    public enum GameState
    {
        Countdown,
        Play,
        Pause,
        Lose,
        Win,
        Destroy
    };

    public GameState gameState;
    float cntdownBegin = 3f;

    [SerializeField]
    GameObject violent;
    [SerializeField]
    GameObject student;
    [SerializeField]
    GameObject player;

    [SerializeField]
    GameObject shapeControllerObj;

    [SerializeField]
    Slider sliderBar;

    [SerializeField]
    Text text;

    [SerializeField]
    GameObject pnlCountdown;

    [SerializeField]
    GameObject pnlPause;

    [SerializeField]
    Button btnResumePausePanel;

    [SerializeField]
    Button btnMenuPausePanel;

    [SerializeField]
    GameObject pnlWin;

    [SerializeField]
    Button btnReplayWinPanel;

    [SerializeField]
    Button btnMenuWinPanel;

    [SerializeField]
    GameObject pnlLose;

    [SerializeField]
    Button btnReplyLosePanel;

    [SerializeField]
    Button btnMenuLosePanel;

    [SerializeField]
    Button btnPause;

    [SerializeField]
    GameObject loseGameObject;

    Lose lose;

    private Player playerScript;

    public float old_x;
    public float pre_x;

    private float EPS = 0.0005f;
    //----------------------------------------------------------

    private void Awake()
    {
        old_x = Math.Abs(player.transform.position.x - student.transform.position.x) - 6f;
        pre_x = old_x;

        gameState = GameState.Countdown;
    }
    // Use this for initialization
    void Start()
    {

        lose = loseGameObject.GetComponent<Lose>();

        playerScript = player.gameObject.GetComponent<Player>();

        btnPause.onClick.AddListener(() => onPauseClick());

        btnResumePausePanel.onClick.AddListener(() => onResumeClick());
        btnMenuPausePanel.onClick.AddListener(() => onMenuClick());

        btnReplayWinPanel.onClick.AddListener(() => onReplyClick());
        btnMenuWinPanel.onClick.AddListener(() => onMenuClick());

        btnReplyLosePanel.onClick.AddListener(() => onReplyClick());
        btnMenuLosePanel.onClick.AddListener(() => onMenuClick());
    }

    void onPauseClick()
    {
        gameState = GameState.Pause;
        pnlPause.SetActive(true);
    }

    void onResumeClick()
    {
        gameState = GameState.Play;
        pnlPause.SetActive(false);
        Time.timeScale = 1f;
    }

    void onReplyClick()
    {
        gameState = GameState.Destroy;
        //SceneManager.LoadScene(1, LoadSceneMode.Single);

        Application.LoadLevel("GameScene");
    }

    void onMenuClick()
    {
        gameState = GameState.Destroy;
        //SceneManager.LoadScene(0, LoadSceneMode.Single);

        Application.LoadLevel("MenuScene");
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        switch (gameState)
        {
            case GameState.Destroy:
                {
                    break;
                }
            case GameState.Countdown:
                {
                    handlerCountdown();
                    break;
                }
            case GameState.Play:
                {
                    
                    handleSlider();
                    checkLogicGame();
                    //handleMouseEvent();
                    break;
                }
            case GameState.Pause:
                {
                    Time.timeScale = 0f;
                    break;
                }

            case GameState.Lose:
                {
                    handleLoseState();
                    break;
                }

            case GameState.Win:
                {
                    handleWinState();
                    break;
                }
        }

    }

    void checkLogicGame()
    {
        if (Math.Abs(sliderBar.value - sliderBar.maxValue) < EPS)
        {
            gameState = GameState.Win;
        }
        else if (Math.Abs(sliderBar.value - sliderBar.minValue) < EPS)
        {
            Debug.Log(Math.Abs(sliderBar.value - sliderBar.minValue));
            gameState = GameState.Lose;
        }
    }

    void handleWinState()
    {
        if (pnlWin.active == false)
        {
            pnlWin.SetActive(true);
        }

    }

    public bool onLoseDone = false;
    void handleLoseState()
    {
        if (onLoseDone == true)
        {
            if (pnlLose.active == false)
            {
                pnlLose.SetActive(true);
                onLoseDone = false;
                
            }
        }
        else
        {
            violent.SetActive(false);
            student.SetActive(false);
            ShapeController sShapeController = shapeControllerObj.GetComponent<ShapeController>();

            sShapeController.handleStopGame();
            lose.onLose();
        }
    }

    void handlerCountdown()
    {
        cntdownBegin -= Time.deltaTime;

        if (cntdownBegin <= 0f)
        {
            pnlCountdown.SetActive(false);
            gameState = GameState.Play;
            ShapeController sShapeController = shapeControllerObj.GetComponent<ShapeController>();

            sShapeController.handleStartGame();

        }
        else if (cntdownBegin <= 1f)
        {
            text.text = "1";
        }
        else if (cntdownBegin <= 2f)
        {
            text.text = "2";
        }
        else
        {
            text.text = "3";
        }
    }

    void handleSlider()
    {

        Vector3 violentPosition = violent.transform.position;
        Vector3 studentPosition = student.transform.position;
        float x = Math.Abs(violentPosition.x - student.transform.position.x) - 4.0f;

        if (pre_x == x)
        {
            //sliderBar.value++;
            old_x = x * 100 / sliderBar.value;
        }
        // float y = abs(violent.transform.position.y - student.transform.position.y);
        // Debug.Log(x);
        // Debug.Log(x / old_x);
        pre_x = x;
        sliderBar.value = (x / old_x) * 100;
    }

}
