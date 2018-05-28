using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject monster;
    public GameObject actor;

    private Camera camera;

    int count = 0;
	// Use this for initialization
	void Start () {
        monster = GameObject.FindGameObjectWithTag("monster");
        actor = GameObject.FindGameObjectWithTag("actor");
            Camera camera = this.gameObject.GetComponent<Camera>();
       
	}
	
	// Update is called once per frame
	void Update () {
        float distance = actor.transform.position.x - monster.transform.position.x;

        // Debug.Log(distance);

        Vector3 camPos = transform.position;

        if (distance < 20)
        {
        
        }
        else
        {
            camPos.x = monster.transform.position.x;
            transform.position = camPos;
        }
	}
}
