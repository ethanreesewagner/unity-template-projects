using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class daynight : MonoBehaviour
{
    public float day;
    public float night;
    public float defaultday;
    public float defaultnight;
    private Light2D sun;
    public static daynight instance;

    void Awake() {
        defaultday = day;
        defaultnight = night;
    }
    // Start is called before the first frame update
    void Start()
    {
        
        if (sun) {return;}
        sun = GetComponent<Light2D>();
        Debug.Log(sun.intensity);
    }

    // Update is called once per frame
    void Update()
    {
       if (day > 0){
        day -= Time.deltaTime;
        sun.intensity = 1.2f;
       } 
       else if (night > 0){
        night -= Time.deltaTime;
        sun.intensity = 0.2f;
       }
       else {
        day = defaultday;
        night = defaultnight;
       }
    }
}
