using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class daynight : MonoBehaviour
{
    public float day;
    public float night;
    public float defaultday;
    public float defaultnight;


    // Start is called before the first frame update
    void Start()
    {
        defaultday = day;
        defaultnight = night;
        Light sun = GetComponent<Light>();
        Debug.Log(sun.intensity);
    }

    // Update is called once per frame
    void Update()
    {
       if (day > 0){
        day -= Time.deltaTime;
       } 
       else if (night > 0){
        night -= Time.deltaTime;
       }
       else {
        day = defaultday;
        night = defaultnight;
       }
    }
}
