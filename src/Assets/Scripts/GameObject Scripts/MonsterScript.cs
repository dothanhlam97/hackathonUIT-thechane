﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterScript : MonoBehaviour
{
    public static string PARAM_DYING = "isDying";
    public enum State { Alive, Dying, Dead }

    //[SerializeField]
    //private Slider HPAlive;

    //[SerializeField]
    //private Slider HPDying;

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



    private GameObject vertical, horizontal, left, right, zizag, circumflex;

	private List<GameObject> listShape;
    private List<Coordinates> listCoordinates;


    public int speed = 2;


    private State monsterState = State.Alive;
    private Animator animator;

    private float damageKnife = 10f;

    private float offsetHPBar = 0.5f;
    private float valueHealth = 0.2f;
    
    // Use this for initialization
    void AWake()
    {
        
    }

    private void Start()
    {
        Debug.Log("Awake");
        animator = GetComponent<Animator>();
		init();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        updateMonster();
    }

    void updateMonster()
    {
        moveMonster();
		Debug.Log(transform.position.x);
		Debug.Log(transform.position.y);
        
		updateIndex(transform.position.x, transform.position.y);
		createShap(5);
    }

    //void updateShap()
    //{
	//	moveShap();
    //    updateValueHealthBar();
    //}



    public GameObject getShape(int vt)
    {
        return listShape[vt];
    }

    public Coordinates getCoordinates(int vt)
    {
        return listCoordinates[vt];
    }
     
	void setShap(int vt){
		
	}

    void initShap(int vt)
	{
        Coordinates coordinates = listCoordinates[vt];
        GameObject gameObject;
        gameObject = listShape[vt];
        Vector3 vector = gameObject.transform.position;
        vector.x = coordinates.getX();
        vector.y = coordinates.getY();
        gameObject.transform.position = vector;
        gameObject.GetComponent<Renderer>().enabled = true;
    }
    
    
    void updateIndex(float x, float y)
    {
        listCoordinates[0].setXY(x - 5, y + 4);
        listCoordinates[1].setXY(x - 2, y + 4);
        listCoordinates[2].setXY(x + 1, y + 4);
        listCoordinates[3].setXY(x + 4, y + 4);
        listCoordinates[4].setXY(x + 7, y + 4);
    }

   
    void moveMonster()
    {
        if (monsterState.Equals(State.Alive))
        {
            // move object
            Vector3 vt = transform.position; //vi tri hien tai
            vt.x += speed * Time.deltaTime; //cho muot
            transform.position = vt;
            // move healbar
        }
    }
    
    public void OnTriggerEnter2D(Collider2D target)
    {
        if (target.gameObject.tag == "Knife")
        {
            switch (monsterState)
            {
                case State.Alive:
                    {
                       
                            Vector3 vt = transform.position; //vi tri hien tai
                            vt.x = (vt.x - 1.5f);
                            transform.position = vt;

                           // HPAlive.value -= damageKnife;
   
                        break;
                    }
                case State.Dying:
                    {
                        break;
                    }
                case State.Dead:
                    {
                        break;
                    }
            }
        }
    }


	void createShap(int n){
		int i;
		if(n == 5 || n == 4){
			i = 0;
		}else if(n == 3 || n == 2){
			i = 1;
		}else {
			i = 2;
		}
		for (int j = 0; j < n; j++)
		{
			initShap(i);
			i++;
		}
	}

	void init()
    {
        listShape = new List<GameObject>();
        listCoordinates = new List<Coordinates>();

        listShape.Add(vertical);
        listShape.Add(horizontal);
        listShape.Add(left);
        listShape.Add(right);
        listShape.Add(zizag);
        listShape.Add(circumflex);
       
        listShape[0] = Instantiate(verticalPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        listShape[1] = Instantiate(horizontalPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        listShape[2] = Instantiate(leftPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        listShape[3] = Instantiate(rightPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        listShape[4] = Instantiate(zizagPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        listShape[5] = Instantiate(circumflexPrefab, new Vector3(0, 0, 0), Quaternion.identity);



        for (int i = 0; i < 6; i++)
        {
            getShape(i).GetComponent<Renderer>().enabled = false;
        
        }
        

		listCoordinates.Add(new Coordinates(0, 0));
        listCoordinates.Add(new Coordinates(0, 0));
        listCoordinates.Add(new Coordinates(0, 0));
        listCoordinates.Add(new Coordinates(0, 0));
        listCoordinates.Add(new Coordinates(0, 0));

		updateIndex(transform.position.x, transform.position.y);
		//createShap(3);
        


    }
}