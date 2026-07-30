using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birds : MonoBehaviour
{
    public float speed;
    public float bound;
    public Vector3 spawn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(speed * Time.deltaTime,0,0));
        if(transform.position.x > bound){
            transform.position = spawn;
        }
    }
}
