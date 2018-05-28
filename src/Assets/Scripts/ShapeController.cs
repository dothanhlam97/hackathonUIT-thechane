using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using PDollarGestureRecognizer;
using SketchDetection;

public class ShapeController : MonoBehaviour
{
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
    [SerializeField]
    private GameObject verticalPrefabDone;
    [SerializeField]
    private GameObject horizontalPrefabDone;
    [SerializeField]
    private GameObject leftPrefabDone;
    [SerializeField]
    private GameObject rightPrefabDone;
    [SerializeField]
    private GameObject zizagPrefabDone;
    [SerializeField]
    private GameObject circumflexPrefabDone;
    [SerializeField]
    private Text highscoreText;
    [SerializeField]
    private GameObject monster;
    int highScore = 0;
    //---------
    // Lam 
    private GameObject vertical, horizontal, left, right, zizag, circumflex;
    private GameObject verticalDone, horizontalDone, leftDone, rightDone, zizagDone, circumflexDone;

    private List<GameObject> listShape;
    private List<GameObject> listShapeDone;
    public static int number;
    public bool stop = false;
    private static Coordinates monsterPosition;
    private static Vector3 monsterVector3;

    private static bool finishAllShape;

    private List<int> indexShape;
    private List<bool> stateShape;  // state = true => pass 
    public static int nShape = 0;

    // new detection 
    public Detection oDetection = new Detection();
    private List<Detection.Shape> nameShape;

    //------------------------ LAM CODE ------------------------------
    public bool start = false;

    public Transform gestureOnScreenPrefab;

    //private List<Gesture> trainingSet = new List<Gesture>();

    private List<Point> points = new List<Point>();
    private int strokeId = -1;

    private Vector3 virtualKeyPosition = Vector2.zero;
    private Rect drawArea;

    private RuntimePlatform platform;
    private int vertexCount = 0;

    private List<LineRenderer> gestureLinesRenderer = new List<LineRenderer>();
    private LineRenderer currentGestureLineRenderer;
    private LineRenderer sampleLineRenderer;


    private const int maxShape = 6;
    private const int nShapeUse = 3;
    //GUI
    private bool recognized;
    private string newGestureName = "";
    //------------------------ LAM CODE ------------------------------

    public GameObject oPlayer;
    public Player sPlayer;

    // destroy shape 

    private bool stopGame = true;


    private void Awake()
    {
       
    }
    private void Start()
    {
        // update coordinate monster 
        monsterPosition = new Coordinates();
        monsterVector3 = new Vector3();
        finishAllShape = true;
        indexShape = new List<int>(0);
        stateShape = new List<bool>(0);
        nameShape = new List<Detection.Shape>(0);
        nameShape.Add(Detection.Shape.Vertical);
        nameShape.Add(Detection.Shape.Horizontal);
        nameShape.Add(Detection.Shape.Left);
        nameShape.Add(Detection.Shape.Right);
        nameShape.Add(Detection.Shape.Zigzag);
        nameShape.Add(Detection.Shape.Circumflex);

        sPlayer = oPlayer.GetComponent<Player>();
        initAllShape();
        codeLam();

        highScore = 0;
        // Vector3 temp = Camera.main.ScreenToWorldPoint(new Vector3((float)Screen.width / -2.0f + 20, (float)Screen.height / -2.0f + 20, 0));
        // highscoreText.transform.position = temp;\
        float width = 0;
        float height = Screen.height;

        Vector3 vt = Camera.main.ScreenToWorldPoint(new Vector3(width, height, 1));
        vt.x += 5f;
        vt.y -= 2f;

        transform.position = vt;

        highscoreText.transform.position = vt;
    }


    public void handleStopGame()
    {
        stopGame = true;
        destroyShape();
    }

    public void handleStartGame()
    {
        stopGame = false;
        destroyShape();
    }
    void createRandomList()
    {
        if (stopGame)
        {
            return;
        }
        updateMonsterPosition();
        for (int i = 0; i < nShape; i++)
        {
            int iRand;
            while (true)
            {
                iRand = (int)(Random.Range(1.0f, 1000.0f) * 1000) % maxShape;
                if (iRand == 4)
                {
                    continue;
                }
                for (int j = 0; j < i; j++)
                {
                    if (indexShape[j] == iRand)
                    {
                        iRand = -1;
                        break;
                    }
                }
                if (iRand != -1)
                {
                    break;
                }
            }
            Vector3 temp = new Vector3(monsterPosition.getX() + 2.0f * (i - (int)(nShape / 2) - 1.0f * (nShape & 1)), monsterPosition.getY() + 3f);
            listShape[iRand].transform.position = temp;
            getShape(iRand).GetComponent<Renderer>().enabled = true;
            indexShape.Add(iRand);
            stateShape.Add(false);
        }
    }

    void updateMonsterPosition()
    {
        if (monster != null)
        {
            monsterPosition.setXY(monster.transform.position.x, monster.transform.position.y);
        }
    }

    void updatePositionShape()
    {
        updateMonsterPosition();
        for (int i = 0; i < nShape; i++)
        {
            Vector3 temp = new Vector3(monsterPosition.getX() + 2.0f * (i - (int)(nShape / 2) - 1.0f * (nShape & 1)), monsterPosition.getY() + 6f);

            if (stateShape[i] == true)
            {

                listShapeDone[indexShape[i]].transform.position = temp;
                getShapeDone(indexShape[i]).GetComponent<Renderer>().enabled = true;
                getShape(indexShape[i]).GetComponent<Renderer>().enabled = false;
            }
            else
            {
                listShape[indexShape[i]].transform.position = temp;
                getShapeDone(indexShape[i]).GetComponent<Renderer>().enabled = false;
                getShape(indexShape[i]).GetComponent<Renderer>().enabled = true;
            }
        }
    }

    void updateHighScore()
    {
        highscoreText.text = "Score: " + System.Convert.ToString(highScore);
    }
    public void destroyShape()
    {
        finishAllShape = true;

        for (int i = 0; i < nShape; i++)
        {
            getShape(indexShape[i]).GetComponent<Renderer>().enabled = false;
            getShapeDone(indexShape[i]).GetComponent<Renderer>().enabled = false;
        }
        indexShape.Clear();
        stateShape.Clear();
        nShape = 0;
    }

    int callContinueShape()
    {
        int index = -1;
        for (int i = 0; i < nShape; i++)
        {
            if (stateShape[i] == false)
            {
                index = i;
                break;
            }
        }
        return index;
    }

    void Update()
    {
        if (stopGame) {
            destroyShape();
            return;
        }
        if (finishAllShape)
        {
            nShape = Random.Range(1, 10000) % nShapeUse + 1;
            finishAllShape = false;
            createRandomList();
        }
        int iShape = callContinueShape();
        //Debug.Log(iShape);
        if (iShape == -1)
        {
            finishAllShape = true;
            highScore += nShape * 10;
            updateHighScore();
            sPlayer.handleThrowItem();
            destroyShape();
        }
        else
        {
            oDetection.setShape(nameShape[indexShape[iShape]]);
            updatePositionShape();
        }
        handleMouseEvent(iShape);
    }


    public GameObject getShape(int vt)
    {
        return listShape[vt];
    }

    public GameObject getShapeDone(int pos)
    {
        return listShapeDone[pos];
    }

    void initAllShape()
    {
        listShape = new List<GameObject>();

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

        for (int i = 0; i < maxShape; i++)
        {
            getShape(i).GetComponent<Renderer>().enabled = false;
        }

        listShapeDone = new List<GameObject>();

        listShapeDone.Add(verticalDone);
        listShapeDone.Add(horizontalDone);
        listShapeDone.Add(leftDone);
        listShapeDone.Add(rightDone);
        listShapeDone.Add(zizagDone);
        listShapeDone.Add(circumflexDone);


        listShapeDone[0] = Instantiate(verticalPrefabDone, new Vector3(0, 0, 0), Quaternion.identity);
        listShapeDone[1] = Instantiate(horizontalPrefabDone, new Vector3(0, 0, 0), Quaternion.identity);
        listShapeDone[2] = Instantiate(leftPrefabDone, new Vector3(0, 0, 0), Quaternion.identity);
        listShapeDone[3] = Instantiate(rightPrefabDone, new Vector3(0, 0, 0), Quaternion.identity);
        listShapeDone[4] = Instantiate(zizagPrefabDone, new Vector3(0, 0, 0), Quaternion.identity);
        listShapeDone[5] = Instantiate(circumflexPrefabDone, new Vector3(0, 0, 0), Quaternion.identity);

        for (int i = 0; i < 6; i++)
        {
            getShapeDone(i).GetComponent<Renderer>().enabled = false;
        }
    }

    void handleMouseEvent(int iShape)
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

            if (Input.GetMouseButtonUp(0) && iShape != -1)
            {
                recognized = true;
                //Debug.Log("check Shape");
                if (oDetection.checkShape(points))
                {
                    stateShape[iShape] = true;
                }
                currentGestureLineRenderer.SetVertexCount(0);
                //currentGestureLineRenderer.SetPosition(vertexCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 10)));
            }
        }
    }


    void codeLam()
    {

        platform = Application.platform;
        drawArea = new Rect(0, 0, Screen.width * 2, Screen.height);
    }
}