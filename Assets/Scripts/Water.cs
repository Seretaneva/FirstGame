using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    float timer = 0f;
    float timerHit = 0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; 
        if(timer >= 2f)
        {
            timer = 0;
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }else if(timer >= 1f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") { 
        
            collision.GetComponent<Player>().inWater = true;
          //  timerHit += Time.deltaTime;
          //  if(timerHit >= 3f)
          //  {
          //      collision.gameObject.GetComponent<Player>().RecountHP(-1);
          //  }

        }
    }
    private void nTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        
            collision.GetComponent<Player>().inWater = false;
        
    }
}
