 using UnityEngine;
using System;

namespace PartScriptTestCS {
    public class PartScriptTestCS : MonoBehaviour {

        public float engineRevs;
        public float exhaustRate;

        ParticleSystem exhaust;


        void Start () {
            Debug.Log("Start");
            exhaust = GetComponent<ParticleSystem>();
        }
    

        void Update () {
            exhaust.emissionRate = engineRevs * exhaustRate;
        }

    }
}