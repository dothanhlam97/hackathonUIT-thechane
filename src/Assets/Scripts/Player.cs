﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using PDollarGestureRecognizer;
using SketchDetection;

public class Player : MonoBehaviour
{
    public static string THROW_TRIGGER = "triggerPlayerThrow";

    //------------------------ LAM CODE ------------------------------
    private bool detach;

    public GameObject horizontalShape;

    public bool start = false;

    public Transform gestureOnScreenPrefab;

    private List<Gesture> trainingSet = new List<Gesture>();

    private List<Point> points = new List<Point>();
    private int strokeId = -1;

    private Vector3 virtualKeyPosition = Vector2.zero;
    private Rect drawArea;

    private RuntimePlatform platform;
    private int vertexCount = 0;

    private List<LineRenderer> gestureLinesRenderer = new List<LineRenderer>();
    private LineRenderer currentGestureLineRenderer;
    private LineRenderer sampleLineRenderer;
    //------------------------ LAM CODE ------------------------------

    //GUI
    private bool recognized;
    private string newGestureName = "";
    public Detection oDetection = new Detection();

    [SerializeField]
    private GameObject itemPrefabs;

    private Animator animator;


    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        initPosition();

        codeLam();
    }

    void initPosition()
    {
        float width = Screen.width - 50;
        float height = Screen.height / 2;

        Vector3 vt = Camera.main.ScreenToWorldPoint(new Vector3(width, height, 1));
        vt.x -= 1f;

        transform.position = vt;
    }

    void codeLam()
    {

        platform = Application.platform;
        drawArea = new Rect(0, 0, Screen.width * 2, Screen.height);

        //Load pre-made gestures
        TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/10-stylus-MEDIUM/");
        foreach (TextAsset gestureXml in gesturesXml)
            trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));

        //Load user custom gestures
        string[] filePaths = Directory.GetFiles(Application.persistentDataPath, "*.xml");
        foreach (string filePath in filePaths)
            trainingSet.Add(GestureIO.ReadGestureFromFile(filePath));

        animator = GetComponent<Animator>();
        detach = false;

        Vector3 temp = new Vector3(5.0f, 0, -5.0f);
        horizontalShape.transform.position += temp;
        oDetection.setShape(Detection.Shape.Horizontal);
    }

    // Update is called once per frame
    void Update()
    {
        handleMouseEvent();
    }

    void handleMouseEvent()
    {
        if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer)
        {
            if (Input.touchCount > 0)
            {
                // message = Convert.ToString(Input.touchCount) + "|";
                virtualKeyPosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                virtualKeyPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
            }
        }

        if (drawArea.Contains(virtualKeyPosition))
        {

            if (Input.GetMouseButtonDown(0))
            {
                recognized = false;
                strokeId = -1;

                points.Clear();

                foreach (LineRenderer lineRenderer in gestureLinesRenderer)
                {

                    lineRenderer.SetVertexCount(0);
                    Destroy(lineRenderer.gameObject);
                }

                gestureLinesRenderer.Clear();

                ++strokeId;
                // TODO
                Transform tmpGesture = Instantiate(gestureOnScreenPrefab, transform.position, transform.rotation) as Transform;
                currentGestureLineRenderer = tmpGesture.GetComponent<LineRenderer>();

                gestureLinesRenderer.Add(currentGestureLineRenderer);

                vertexCount = 0;
            }

            if (Input.GetMouseButton(0))
            {
                points.Add(new Point(virtualKeyPosition.x, -virtualKeyPosition.y, strokeId));

                currentGestureLineRenderer.SetVertexCount(++vertexCount);
                currentGestureLineRenderer.SetPosition(vertexCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 10)));
            }

            if (Input.GetMouseButtonUp(0))
            {
                recognized = true;
                if (oDetection.checkShape(points))
                {
                    animator.SetTrigger(THROW_TRIGGER);
                    onThrowItem();
                }

                oDetection.setShape(Detection.Shape.Vertical);
                points.Clear();
                currentGestureLineRenderer.SetVertexCount(0);
                //currentGestureLineRenderer.SetPosition(vertexCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 10)));
            }
        }
    }

    void onThrowItem()
    {
        Instantiate(itemPrefabs, transform.position, Quaternion.identity);
    }
}
