using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lose : MonoBehaviour {

    [SerializeField]
    GameObject gameControllerObj;

    GameController gameController;

    float countdown = 2f;
    bool isLose = false;
    // Use this for initialization
	void Start () {
        gameController = gameControllerObj.GetComponent<GameController>();

        initPosition();
    }

    // Update is called once per frame
    void FixedUpdate () {

        if (isLose == true)
        {
            countdown -= Time.deltaTime;

            if (countdown <= 0.0005f)
            {
                gameController.onLoseDone = true;
            }
        }
    }


    public void onLose()
    {
        gameObject.SetActive(true);

        isLose = true;
    }
    void initPosition()
    {
        Vector3 vt = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 1));

        vt.x += 4f;

        vt.y += 5f;

        transform.position = vt;
    }
}
