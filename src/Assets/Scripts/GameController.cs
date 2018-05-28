using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour {
    //--------------------Lam-----------------------------
    [SerializeField]
	GameObject violent;
	[SerializeField]
	GameObject student;
	[SerializeField]
	Slider sliderBar;
    
    public float old_x;
	public float pre_x;
    //----------------------------------------------------------

    //--- TAI--
    [SerializeField]
    private GameObject verticalPrefab;
    [SerializeField]
    private GameObject horizontalPrefab;
    [SerializeField]
    private GameObject leftPrefab;
    [SerializeField]
    private GameObject rightPrefab;
    [SerializeField]
    private GameObject zizagPrefab;
    [SerializeField]
    private GameObject circumflexPrefab;
    //---------

    // Use this for initialization
    void Start () {
		old_x = Math.Abs(violent.transform.position.x - student.transform.position.x) - 2;
		pre_x = old_x;
	}
	
	// Update is called once per frame
	void Update () {
        handleSlider();
    }

    void handleSlider()
    {
        float x = Math.Abs(violent.transform.position.x - student.transform.position.x) - 2;

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
