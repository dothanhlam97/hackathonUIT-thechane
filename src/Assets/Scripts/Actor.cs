using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        initPosition();
    }

    void initPosition()
    {
        float height = Screen.height / 2;

        Vector3 vt = Camera.main.ScreenToWorldPoint(new Vector3(0, height, 1));
        vt.x += 1.5f;

        transform.position = vt;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
