using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayController : MonoBehaviour {

    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    private Button btnPause;
    [SerializeField]
    private Button btnResume;
    [SerializeField]
    private Button btnMenu;


    public bool isPauseGame = false;
	// Use this for initialization
	void Start () {
        btnResume.onClick.AddListener(()=> onBtnResumeClick());
        btnPause.onClick.AddListener(() => onBtnPauseClick());
        btnMenu.onClick.AddListener(()=> onBtnMenuClick());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onBtnPauseClick() {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        isPauseGame = true;
    
    }

    public void onBtnResumeClick() {
        Time.timeScale = 1f;
        isPauseGame = false;
        pausePanel.SetActive(false);
    }

    public void onBtnMenuClick() {
        Time.timeScale = 1f;
        Application.LoadLevel("MenuScene");
    }
}
