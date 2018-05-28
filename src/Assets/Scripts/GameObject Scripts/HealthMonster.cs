using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthMonster : MonoBehaviour {
    [SerializeField]
    Slider healBar;

    float cnt = 100;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (cnt > 0) {
            cnt -= Time.deltaTime;
            healBar.value = cnt;
        }
    }
}
