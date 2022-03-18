using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    private Rigidbody2D rd2d;

    public float speed;

    public float jumpForce;

    public Text score;

    public Text lives;

    public GameObject wintext;

    public GameObject losetext;

    private int scoreValue = 0;

    private int lifeValue = 3;

    public AudioClip musicClipOne;

    public AudioClip musicClipTwo;

    public AudioSource musicSource;

    Animator anim;

    private bool facingRight = true;

    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = ("Score =  " + scoreValue.ToString());
        wintext.SetActive(false);
        losetext.SetActive(false);
        lives.text = ("Lives =  " + lifeValue.ToString());

        musicSource.clip = musicClipOne;
        musicSource.Play();
        musicSource.loop = true;

        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("State", 1);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("State", 1);
        }

        if (Input.GetKeyUp(KeyCode.D))

        {
          anim.SetInteger("State", 0);
        }

        if (Input.GetKeyUp(KeyCode.A))

        {
          anim.SetInteger("State", 0);
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if (lifeValue <= 0)
        {
            losetext.SetActive(true);
            Destroy(this);
            anim.SetInteger("State", 0);
        }

        if (scoreValue >= 8)
        {
            wintext.SetActive(true);
            Destroy(this);
            musicSource.Stop();
            musicSource.clip = musicClipTwo;
            musicSource.loop = false;
            musicSource.Play();

        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = ("Score =  " + scoreValue.ToString());
            Destroy(collision.collider.gameObject);

            if (scoreValue == 4)
            {
                lifeValue = 3;
                lives.text = ("Lives =  " + lifeValue.ToString());
                transform.position = new Vector2(70.0f, 0.002f);
            }
        }

        if(collision.collider.tag == "Enemy")
        {
            lifeValue -= 1;
            lives.text = ("Lives =  " + lifeValue.ToString());
            Destroy(collision.collider.gameObject);

        }

    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }   
        }
    }

    void Flip()
    {
      facingRight = !facingRight;
      Vector2 Scaler = transform.localScale;
      Scaler.x = Scaler.x * -1;
      transform.localScale = Scaler;
    }

}
