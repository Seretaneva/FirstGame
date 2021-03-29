using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    public Transform door;
    public Sprite mid, top;
 
    void Start()
    {
        
    }

  
    void Update()
    {
        
    }
    public void Unlock()
    {
        isOpen = true;
        GetComponent<SpriteRenderer>().sprite = mid;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = top;
    }
   public void Teleport(GameObject player)
    {
        player.transform.position = new Vector3(door.position.x, door.position.y, player.transform.position.z);
    }
}
