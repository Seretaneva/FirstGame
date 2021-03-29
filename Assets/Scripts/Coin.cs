using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private string coinName;
    public int coinValue;
    void Start()
    {
        coinName = name.Substring(0, 2);
        switch(coinName)
        {
            case "Silver":
                coinValue = 1;
                break;
            case "Gold":
                coinValue = 2;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
