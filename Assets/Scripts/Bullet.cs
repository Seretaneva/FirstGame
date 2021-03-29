using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
     float speed = 2f;
    float timeToDisable = 10f;
    void Start()
    {
        StartCoroutine(SetDisabled()); 
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime); 
    }
    IEnumerator SetDisabled()
    {
        yield return new WaitForSeconds(timeToDisable);
        gameObject.SetActive(false);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(SetDisabled());
        gameObject.SetActive(false);
    }
}
