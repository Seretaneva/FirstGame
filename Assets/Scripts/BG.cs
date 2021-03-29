using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG : MonoBehaviour
{
    float lenght, strposition;
    public GameObject cam;
    public float parallaxEffect;
    void Start()
    {
        strposition = transform.position.x;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = cam.transform.position.x * (1 - parallaxEffect);
        float dist = cam.transform.position.x* parallaxEffect;
        transform.position = new Vector3(strposition + dist, transform.position.y, transform.position.z);
        if (temp > lenght +  strposition)
            strposition += lenght;
        else if (temp <  strposition - lenght)
            strposition -= lenght;
    }
}
