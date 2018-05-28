<<<<<<< HEAD
﻿using System.Collections;
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
=======
﻿using System.Collections;
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
>>>>>>> 3dc2a3a08d7d06d8f282a1e8056c1f5f9f43190a
