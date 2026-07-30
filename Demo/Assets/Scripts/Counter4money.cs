using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Counter4money : MonoBehaviour
{ 
    public int counter4money = 0 ;
    public TMP_Text money;
    
    void changemoney(int amount)
    {
        counter4money += amount;
        money.text = $"{counter4money}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
