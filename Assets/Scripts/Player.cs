using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    public float jumpHeight;
    public Transform groundCheck;
    bool isGrounded;
    Animator anim;
    int crHP;
    int maxHP = 3;
    bool isHit = false;
    public Main main;
    public bool key = false;
    bool canTp = true;
    public bool inWater = false;
    bool isClimbing = false;
    int coin = 0;
    int coin1 = 1;
    bool canHit = true;
    public GameObject blueGem, greenGem;
    int gemCount = 0;
    int coinSum = 0;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        crHP = maxHP;
   
    }


    void Update()
    {
        if (inWater && !isClimbing)
        {
            anim.SetInteger("State", 4);
            isGrounded = true;
            if (Input.GetAxis("Horizontal") != 0)
                Flip();
        }
        else
        {
            GroundCheck();
            if (Input.GetAxis("Horizontal") == 0 && (isGrounded) && !isClimbing)
            {
                anim.SetInteger("State", 1);
            }
            else
            {
                Flip();
                if (isGrounded && !isClimbing)
                    anim.SetInteger("State", 2);
            }
        }

    }
    void FixedUpdate()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            rb.velocity = new Vector2(0, jumpHeight);
    }

    void Flip()
    {
        if (Input.GetAxis("Horizontal") > 0)
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        if (Input.GetAxis("Horizontal") < 0)
            transform.localRotation = Quaternion.Euler(0, 180, 0);
    }

    void GroundCheck()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f);
        isGrounded = colliders.Length > 1;
        if (!isGrounded && !isClimbing)
            anim.SetInteger("State", 3);
    }
    public void RecountHP(int deltaHP)
    {

        if (deltaHP < 0 && canHit)//problema
        {
            crHP = crHP + deltaHP;
            StopCoroutine(Onhit());
            canHit = false;
            isHit = true;
            StartCoroutine(Onhit());
        }
        else if (crHP > maxHP)
        {
            crHP = crHP + deltaHP;
            crHP = maxHP;
        }
        print(crHP);

        if (crHP <= 0 && !canHit)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            Invoke("Lose", 1f);
        }

    }
    IEnumerator Onhit()
    {
        if (isHit)
            GetComponent<SpriteRenderer>().color = new Color(1f, GetComponent<SpriteRenderer>().color.g - 0.04f, GetComponent<SpriteRenderer>().color.b - 0.04f);
        else
            GetComponent<SpriteRenderer>().color = new Color(1f, GetComponent<SpriteRenderer>().color.g + 0.04f, GetComponent<SpriteRenderer>().color.b + 0.04f);
        if (GetComponent<SpriteRenderer>().color.g == 1f)
        {
            StopCoroutine(Onhit());
            canHit = true;
        }

        if (GetComponent<SpriteRenderer>().color.g <= 0)
            isHit = false;
        yield return new WaitForSeconds(0.02f);
        StartCoroutine(Onhit());
    }
    void Lose()
    {
        main.GetComponent<Main>().Lose();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "key")
        {
            Destroy(collision.gameObject);
            key = true;

        }
        if (collision.tag == "Door")
        {
            if (collision.gameObject.tag == "Door")
            {
                if (collision.GetComponent<Door>().isOpen && canTp)
                {
                    collision.GetComponent<Door>().Teleport(gameObject);
                    canTp = false;
                    StartCoroutine(TpWait());
                }
                else if (key)
                    collision.gameObject.GetComponent<Door>().Unlock();
            }
        }
       
        if (collision.tag == "coin")
        {
           
            Destroy(collision.gameObject);
            coin++;
            coinSum ++;
            print("Coins = " + coinSum);

        }
        if(collision.tag == "coin1")
        {
          
            Destroy(collision.gameObject);
            coin1++;
            coinSum += 2;

            print("Coins = " + coinSum);
        }
        if (collision.tag == "Heart")
        {
            if (crHP < maxHP)
            {
                Destroy(collision.gameObject);
                RecountHP(1);
            }




        }
        if (collision.tag == "mushroom")
        {
            Destroy(collision.gameObject);
            RecountHP(-1);
        }
        if (collision.tag == "GemBlue")
        {
            Destroy(collision.gameObject);
            StartCoroutine(NoHit());

        }
        if (collision.tag == "GemGreen")
        {
            Destroy(collision.gameObject);
            StartCoroutine(SpeedBonus());

        }
    }
    IEnumerator TpWait()
    {
        yield return new WaitForSeconds(2f);
        canTp = true;

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")

        {
            isClimbing = true;
            rb.bodyType = RigidbodyType2D.Kinematic;
            if (Input.GetAxis("Vertical") == 0)
                anim.SetInteger("State", 5);
            else
            {
                anim.SetInteger("State", 6);
                transform.Translate(Vector3.up * Input.GetAxis("Vertical") * speed * 0.5f * Time.deltaTime);
            }


        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isClimbing = false;
        rb.bodyType = RigidbodyType2D.Dynamic;

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "trampoline")
        {
            StartCoroutine(Trampoline_anim(collision.gameObject.GetComponentInParent<Animator>()));
        }
        if(collision.gameObject.tag == "quickSand")
        {
            speed *= 0.25f;
            rb.mass *= 100f;
        }
    }
    IEnumerator Trampoline_anim(Animator an)
    {
        an.SetBool("isJump", true);
        yield return new WaitForSeconds(0.5f);
        an.SetBool("isJump", false);
    }
    IEnumerator NoHit()
    {
        gemCount++;
        blueGem.SetActive(true);
        CheckGem(blueGem);
        canHit = false;
        blueGem.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(4f);
        StartCoroutine(Invisible(blueGem.GetComponent<SpriteRenderer>(), 0.02f));
        yield return new WaitForSeconds(1f);
        canHit = true;

        gemCount--;
        blueGem.SetActive(false);
        CheckGem(greenGem);
    }
    IEnumerator SpeedBonus()
    {
        gemCount++;
        greenGem.SetActive(true);
        CheckGem(greenGem);
        speed = speed * 2;
        greenGem.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(4f);
        StartCoroutine(Invisible(greenGem.GetComponent<SpriteRenderer>(), 0.02f));
        yield return new WaitForSeconds(1f);
        speed = speed/2;
        gemCount--;
        greenGem.SetActive(false);
        CheckGem(blueGem);

    }
    void CheckGem(GameObject obj)
    {
        if( gemCount == 1)
        {
            obj.transform.localPosition = new Vector3(0f, 0.6f, obj.transform.localPosition.z);
        }else if(gemCount == 2)
        {
            blueGem.transform.localPosition = new Vector3(-0.5f, 0.44f, blueGem.transform.localPosition.z);
            greenGem.transform.localPosition = new Vector3(0.5f, 0.44f, greenGem.transform.localPosition.z);
        }

    }
    IEnumerator Invisible(SpriteRenderer spr, float time)
    {
        spr.color = new Color(1f, 1f, 1f, spr.color.a - time * 2);
        yield return new WaitForSeconds(time);
        if (spr.color.a > 0)
            StartCoroutine(Invisible(spr, time));
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "quickSand")
        {
            speed *= 4f;
            rb.mass *= 0.01f;
        }
    }
}
