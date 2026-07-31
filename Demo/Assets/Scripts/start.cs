using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class start : MonoBehaviour
{
    public string SceneToLoad;


    public void labubu()
    {
        SceneManager.LoadScene(SceneToLoad);
        
    }
}

