using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    //Movement Handler
    public Vector2 velocity = Vector2.zero;
    public Vector2 jumpForce;
    
    //Initialization Handler
    public static PlayerController instance;
    private Animator anim;
    private Rigidbody2D body2D;
    public bool pause, dead, restart;
    bool canjump;
    private bool up, down, right;

    //Reset Handler
    public List<GameObject> curryBowls;
    public List<GameObject> barrels;
    private int count;
    Vector3 startPos;

    //Attack Handler
    public BoxCollider2D attackTrigger;
    public BoxCollider2D playerTrigger;

    //BlastFX
    public GameObject barrelBlast;


    bool attacking;
    float attackTimer = 0;
    public float attackDuration = 0.3f;

    //UI  Texts
    public Text countText;
    public Text endText;
    public Text nameText;

    //Audio Handler 
    public AudioSource jumpSound, slideSound, deadSound, pickupSound, winSound, attackSound;


	private void Awake()
	{
        instance = this;
	}

	void Start()
    {
        pause = dead = true;
        restart = false;
        attacking = false;

        body2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        attackTrigger.enabled = false;

        startPos = transform.position;

        count = 0;
        endText.text = "";
        nameText.text = "Smiley Run";
        SetCountText();
    }

    void Update()
    {
        up = Input.GetKey(KeyCode.UpArrow) ? true : false;
        down = Input.GetKeyDown(KeyCode.DownArrow) ? true : false;
        right = Input.GetKeyDown(KeyCode.RightArrow) ? true : false;
    }


    void FixedUpdate () 
    {
        if(!pause || !dead)
        {
            if (up && canjump)
            {
                anim.SetBool("canJump", true);
                body2D.AddForce(jumpForce);
                canjump = false;
                jumpSound.Play();
            }

            if (down)
            {
                anim.SetTrigger("canSlide");
                slideSound.Play();
            }

            if (right && !attacking)
            {
                attackTrigger.enabled = true;
                attackTimer = attackDuration;
                anim.SetTrigger("canAttack");
                attackSound.Play();
                attacking = true;
            }

            if (attacking)
            {
                if(attackTimer > 0)
                {
                    attackTimer -= Time.deltaTime;
                }
                else
                {
                    attacking = false;
                    attackTrigger.enabled = false;
                }
            }

            body2D.transform.Translate(velocity * Time.deltaTime);

            UpdateCollider(1f, 0.2f);
        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Floor")
        {
            anim.SetBool("canJump", false);
            canjump = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("CurryBowl"))
        {
            other.gameObject.SetActive(false);
            curryBowls.Add(other.gameObject);
            count = count + 1;
            SetCountText();
            pickupSound.Play();
            restart = true;
        }
        else if (other.gameObject.CompareTag("Obstacle"))
        {
                gameObject.SetActive(false);
                endText.text = "Game Over";
                nameText.text = "Smiley Run";
                dead = true;
                deadSound.Play();
        }
        else if (other.gameObject.CompareTag("Weak_Obstacle"))
        {
            if(attackTrigger.enabled == true)
            {
                other.gameObject.SetActive(false);
                barrels.Add(other.gameObject);
                GameObject clone =  Instantiate(barrelBlast, other.gameObject.transform.position, other.gameObject.transform.rotation);
                Destroy(clone, 1);
                attackTrigger.enabled = false;
                return;
            }
            else
            {
                gameObject.SetActive(false);
                endText.text = "Game Over";
                nameText.text = "Smiley Run";
                dead = true;
                deadSound.Play();
            }          

        }
        else if (other.gameObject.CompareTag("Door"))
        {
            gameObject.SetActive(false);
            endText.text = "Level Complete";
            nameText.text = "Smiley Run";
            dead = true;
            winSound.Play();
        }
    }

	public void Reset()
	{
        count = 0;
        SetCountText();
        transform.position = startPos;

        pause = dead = false;

        if(!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
 
        if (curryBowls.Count > 0)
        {
            for(int i=0; i<curryBowls.Count; i++)
            {
                curryBowls[i].SetActive(true);
            }
            curryBowls.Clear();
        }
        if (barrels.Count > 0)
        {
            for (int i = 0; i < barrels.Count; i++)
            {
                barrels[i].SetActive(true);
            }
            barrels.Clear();
        }
    }

    void UpdateCollider(float x, float y)
    {
        Vector2 bound = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size;
        bound.x -= x;
        bound.y -= y;
        gameObject.GetComponent<BoxCollider2D>().size = bound;
    }

    void SetCountText()
    {
        countText.text = "Curry Bowls: " + count.ToString();
    }

}
