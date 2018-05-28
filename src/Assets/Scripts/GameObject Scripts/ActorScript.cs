using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using PDollarGestureRecognizer;
using SketchDetection;

public class ActorScript : MonoBehaviour {
    private Animator animator;
	private bool detach;
	[SerializeField]
	private GameObject knifePrefab;
	public GameObject horizontalShape;

    // public GameObject bullet;

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


    //GUI
	private string message;
	private string message1;
	private string message2;
	private bool recognized;
	private string newGestureName = "";
	public Detection oDetection = new Detection();

   




	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        Screen.orientation = ScreenOrientation.LandscapeLeft;
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
        message1 = "Horizontal";
		Vector3 temp = new Vector3(5.0f,0,-5.0f);
		horizontalShape.transform.position += temp;
		oDetection.setShape(Detection.Shape.Horizontal);

	}
	
	// Update is called once per frame
	void Update () {
        // hanleEvent();

        // partical system 
         // Get the mouse position in pixels, and convert to camera view by dividing by the number of pixels the camera is displaying.
        




        if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer) {
			if (Input.touchCount > 0) {
				// message = Convert.ToString(Input.touchCount) + "|";
				virtualKeyPosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
			}
		} else {
			if (Input.GetMouseButton(0)) {
				virtualKeyPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
			}
		}

		if (drawArea.Contains(virtualKeyPosition)) {

			if (Input.GetMouseButtonDown(0)) {

                message = "";
                recognized = false;
                strokeId = -1;

                points.Clear();

                foreach (LineRenderer lineRenderer in gestureLinesRenderer) {

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
			
			if (Input.GetMouseButton(0)) {
				points.Add(new Point(virtualKeyPosition.x, -virtualKeyPosition.y, strokeId));

				message += "(" + Convert.ToString(virtualKeyPosition.x) + "," + Convert.ToString(virtualKeyPosition.y) + ")  ";

				currentGestureLineRenderer.SetVertexCount(++vertexCount);
				currentGestureLineRenderer.SetPosition(vertexCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 10)));

			}

			if (Input.GetMouseButtonUp(0)) {
				recognized = true;
                Debug.Log("check");
				if (oDetection.checkShape(points)) {
                    Debug.Log("Pass");
					hanleEvent();
				}
				message1 = "Vertical";
				oDetection.setShape(Detection.Shape.Vertical);
			}
		}
	}

    void hanleEvent() {
        //neu detach dung thi chay ham nem
            animator.SetTrigger("onThrow");
            throwKnife();
    }

    void throwKnife() {
		Instantiate(knifePrefab, transform.position, Quaternion.identity);
    }
}
