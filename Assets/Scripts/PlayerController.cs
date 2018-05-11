using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public static int level = 1;
    public static bool skipUI = false;

    //Movement Handler
    public Vector2 velocity = Vector2.zero;
    public Vector2 jumpForce;
    
    //Initialization Handler
    public static PlayerController instance;
    private Animator anim;
    private Rigidbody2D body2D;
    public bool pause, dead, restart;
    bool canjump;
    private bool up, down, downRelease, right;

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
    public Text playText;

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
        if (skipUI)
        {
            Reset();
            skipUI = false;
        }
    }

    void Update()
    {
        up = Input.GetKey(KeyCode.UpArrow);
        down = Input.GetKey(KeyCode.DownArrow);
        //downRelease = Input.GetKeyUp(KeyCode.DownArrow);
        right = Input.GetKey(KeyCode.RightArrow);
    }


    void FixedUpdate () 
    {
        if(!pause && !dead)
        {
            if (up && canjump)
            {
                anim.SetBool("canJump", true);
                body2D.AddForce(jumpForce);
                canjump = false;
                //jumpSound.Play();
            }

            if (down)
            {
                anim.SetBool("canSlide", true);
                //slideSound.Play();
            }
            else
            {
                anim.SetBool("canSlide", false);
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
                ResetScene();
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
                ResetScene();
                deadSound.Play();
            }          

        }
        else if (other.gameObject.CompareTag("Door"))
        {
            gameObject.SetActive(false);
            endText.text = "Level Complete";
            nameText.text = "Smiley Run";
            ResetScene();
            winSound.Play();
            UIManager.unlockedLevel = System.Math.Max(level+1, UIManager.unlockedLevel);
        }
    }

	public void ResetScene()
	{
        pause = dead = true;
        count = 0;
        transform.position = startPos;
 
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

        playText.text = "Replay";
    }

    public void Reset()
    {
        SetCountText();
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        pause = dead = false;
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
