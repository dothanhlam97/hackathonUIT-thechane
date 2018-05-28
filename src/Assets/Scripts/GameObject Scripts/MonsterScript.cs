using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterScript : MonoBehaviour
{
    public static string PARAM_DYING = "isDying";
    public enum State { Alive, Dying, Dead }

    [SerializeField]
    private Slider HPAlive;

    [SerializeField]
    private Slider HPDying;

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
        // Debug.Log("Awake");
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        updateMonster();
    }

    void updateMonster()
    {
        moveMonster();
        updateHealthBar();
    }

    void updateHealthBar()
    {
        moveHeathBar();
        updateValueHealthBar();
    }
    void updateValueHealthBar() {

        switch (monsterState)
        {
            case State.Alive:
                {
                    HPAlive.value += valueHealth / 2;
                    break;
                }
            case State.Dying:
                {
                    if (HPDying.value + valueHealth > HPDying.maxValue)
                    {
                        monsterState = State.Alive;
                        animator.SetBool(PARAM_DYING, false);


                        HPDying.gameObject.SetActive(false);
                        HPAlive.gameObject.SetActive(true);
                    }
                    else
                    {
                        HPDying.value += valueHealth;
                    }
                    break;
                }
            case State.Dead:
                {
                    break;
                }
        }
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

    void moveHeathBar()
    {
        // Debug.Log("Move");

        switch (monsterState)
        {
            case State.Alive:
                {
                    Vector3 vt3 = HPAlive.transform.position;

                    vt3.x = transform.position.x + offsetHPBar;

                    HPAlive.transform.position = vt3;
                    break;
                }

            case State.Dying:
                {
                    Vector3 vt3 = HPDying.transform.position;

                    vt3.x = transform.position.x + offsetHPBar;

                    HPDying.transform.position = vt3;
                    break;
                }
            case State.Dead:
                {
                    break;
                }
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
                        if (HPAlive.value - damageKnife < 0)
                        {
                            HPAlive.gameObject.SetActive(false);
                            HPDying.gameObject.SetActive(true);
                            HPDying.value -= 20f;

                            monsterState = State.Dying;

                            moveHeathBar();
                            animator.SetBool(PARAM_DYING, true);
                        }
                        else
                        {
                            Vector3 vt = transform.position; //vi tri hien tai
                            vt.x = (vt.x - 1.5f);
                            transform.position = vt;

                            HPAlive.value -= damageKnife;
                        }
                        break;
                    }
                case State.Dying:
                    {
                        if (HPDying.value - damageKnife < 0)
                        {
                            HPDying.gameObject.SetActive(false);
                        }
                        else
                        {
                            HPDying.value -= damageKnife;
                        }
                        break;
                    }
                case State.Dead:
                    {
                        break;
                    }
            }
        }
    }
}
