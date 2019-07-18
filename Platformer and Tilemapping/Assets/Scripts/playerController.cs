using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour
{
    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI playerLivesText;
    public TextMeshProUGUI restartText;
    public AudioSource audioSource;
    public AudioClip gotCoin;
    public AudioClip jumpSound;
    public AudioClip playerDie;
    public AudioClip enemyHit;
    public AudioClip gameOver;
    public float maxSpeed;
    private Rigidbody2D rb2d;
    public float speed;
    public float jumpForce;
    private int score;
    private int lives;
    private bool facingRight = true;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        lives = 3;
        winText.text = "";
        restartText.text = "";
        playerScoreText.text = "Player Score: ";
        playerLivesText.text = "Lives: " + lives;
        audioSource = GetComponent<AudioSource> ();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal, 0);
        rb2d.AddForce(movement * speed);
        rb2d.velocity = Vector3.ClampMagnitude(rb2d.velocity, maxSpeed);//puts in a speed limit

        if (Input.GetKey("escape"))//exit game with escape key
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.R))//restart game with r key
        {
            SceneManager.LoadScene(0);
        }

        if (facingRight == false && moveHorizontal > 0)//make the sprite face left or right depending on direction the player is going
        {
            Flip();
        }
        else if (facingRight == true && moveHorizontal < 0)
        {
            Flip();
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(lives == 0)
        {
            return;
        }

        if (rb2d.velocity.magnitude > 0)//set running animation if moving
        {
            anim.SetInteger("State",1);
        }
        
        if (rb2d.velocity.magnitude == 0)//set idle animation if not moving
        {
            anim.SetInteger("State",0);
        }

        if (Input.GetKey(KeyCode.UpArrow) && collision.collider.tag == "Ground")
        {
            //will allow jump only if in contact with ground tile
            audioSource.PlayOneShot(jumpSound, 0.2F);
            rb2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            anim.SetInteger("State",2);
        }
    }

    //made the below method to avoid an initial collision that would happen with the OnCollisionStay2D()
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag ("Pickup"))
        {
            if( lives == 0)//helps prevent instant looping if player picks up coin upon death.
            {
                return;
            }
            audioSource.PlayOneShot(gotCoin, 0.5F);//plays a soundclip one time from the public variable
            other.gameObject.SetActive (false);
            score = score + 1;

            SetCountText();
            movePlayer();
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            if( lives == 0)
            {
                return;
            }

            anim.SetInteger("State",4);//play hurt animation

            audioSource.PlayOneShot(enemyHit, 0.7F);
            other.gameObject.SetActive(false);
            lives = lives - 1;

            SetCountText();
        }
    }

    void movePlayer()//moved out of SetCountText() to stop accidental teleporting if player touches enemy
    {
        if (score == 4)
        {
            transform.position = new Vector3(69.22f, 2.6f, 0.0f);
            lives = 3;
            SetCountText();
        }
    }

    void SetCountText()
    {

        if (score == 8)
        {
            winText.SetText("You Win!");
            restartText.SetText("Press R to try again or Escape to quit!");
            speed = 0;
            jumpForce = 0;
        }

        if (lives == 0)
        {
            anim.SetInteger("State",3);
            audioSource.PlayOneShot(playerDie, 0.2F);
            winText.SetText("You Lose!");
            speed = 0;
            jumpForce = 0;
            StartCoroutine(removePlayer());
        }

        playerScoreText.text = "Player Score: " + score.ToString();
        playerLivesText.text = "Lives: " + lives.ToString();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

    IEnumerator removePlayer()
    {
        yield return new WaitForSeconds(4);
        audioSource.PlayOneShot(gameOver, 0.9F);
        restartText.SetText("Press R to try again!");
        winText.SetText("GAME OVER!");
        gameObject.GetComponent<Renderer>().enabled = false;
    }
}
