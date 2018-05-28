using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour {

    [SerializeField]
    Button btnPlay;
	// Use this for initialization
	void Start () {
        btnPlay.onClick.AddListener(() => onBtnPlayClick());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void onBtnPlayClick()
    {
        Application.LoadLevel("GameScene");
    }
}
