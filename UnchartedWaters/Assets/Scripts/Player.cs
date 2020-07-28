using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject head;
    public GameObject surfacePP;
    public GameObject waterPP;

    public float movementSpeed;
    public float swimSpeed;
    public float mouseSensitivity;

    private float lookX;
    private float lookY;
    public float swimAntiGravity;
    public bool underwater;
    public bool bodyUnderwater;

    public float health;
    public Vector3 startingPosition;

    public GameObject fish;
    public int score;

    public Text scoreBoard;
    public Text gameOverScore;
    public Text winScore;
    public GameObject gameOverPanel;
    public GameObject winPanel;

    public bool isDead;

    public GameObject startMenu;
    public int gameAFKTimer;
    public bool gameActive;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ChangeWaterState();
        scoreBoard.text = "0";
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        InvokeRepeating("CountTimer", 1f, 1f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameAFKTimer > 30)
        {
            Restart();
        }
        if (!isDead)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Restart();
            }
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftControl))
            {
                startMenu.SetActive(false);
                gameAFKTimer = 0;
                gameActive = true;
            }
            // print(head.transform.position.y);
            if (underwater)
            {
                if (head.transform.position.y < 300.5f)
                { underwater = true; }
                else
                {
                    underwater = false;
                    ChangeWaterState();
                }
            }
            else
            {
                if (head.transform.position.y < 300.5f)
                {
                    underwater = true;
                    ChangeWaterState();
                }
                else { underwater = false; }
            }

            if (bodyUnderwater)
            {
                if (this.transform.position.y < 300.5f)
                { bodyUnderwater = true; }
                else
                { bodyUnderwater = false; }
            }
            else
            {
                if (this.transform.position.y < 300.5f)
                { bodyUnderwater = true; }
                else { bodyUnderwater = false; }
            }

            if (!bodyUnderwater)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    rb.AddForce(transform.forward * movementSpeed);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    rb.AddForce(transform.right * -movementSpeed);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    rb.AddForce(transform.forward * -movementSpeed);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    rb.AddForce(transform.right * movementSpeed);
                }
                rb.velocity = new Vector3(rb.velocity.x / 2f, rb.velocity.y, rb.velocity.z / 2f);
            }
            else
            {
                if (Input.GetKey(KeyCode.W))
                {
                    rb.AddForce(head.transform.forward * swimSpeed);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    rb.AddForce(head.transform.right * -swimSpeed);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    rb.AddForce(head.transform.forward * -swimSpeed);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    rb.AddForce(head.transform.right * swimSpeed);
                }
                if (Input.GetKey(KeyCode.Space))
                {
                    rb.AddForce(Vector3.up * swimSpeed);
                }
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    rb.AddForce(Vector3.up * -swimSpeed);
                }

                rb.AddForce(transform.up * swimAntiGravity);
                rb.velocity = new Vector3(rb.velocity.x / 1.1f, rb.velocity.y / 1.1f, rb.velocity.z / 1.1f);           
                if (!gameActive) rb.velocity = new Vector3(0, 0, 0);
            }
        } else
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Restart();
            }
        }
    }

    void Update()
    {
        lookX += Input.GetAxis("Mouse X") * mouseSensitivity;
        lookY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        lookY = Mathf.Clamp(lookY, -70, 70);
        this.transform.localEulerAngles = new Vector3(0, lookX, 0);
        head.transform.localEulerAngles = new Vector3(lookY, 0, 0);
        if (Input.GetKeyDown(KeyCode.Space) && !bodyUnderwater && Physics.Raycast(transform.position, Vector3.down, 1.1f))
        {
            rb.AddForce(0, 350, 0);
        }
    }

    public void ChangeWaterState()
    {
        if (underwater)
        {
            surfacePP.SetActive(false);
            waterPP.SetActive(true);
        }
        else if (!underwater)
        {
            surfacePP.SetActive(true);
            waterPP.SetActive(false);
        }         
    }

    public void Hit(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        isDead = true;
        gameOverPanel.SetActive(true);
        gameOverScore.text = "Your score was: " + score.ToString();
    }

    public void Crystal()
    {
        score++;
        scoreBoard.text = score.ToString();
        if (score >= FindObjectOfType<CrystalCollection>().crystalCount)
        {
            Win();
        }
    }

    public void Restart()
    {
        gameAFKTimer = 0;
        gameActive = false;
        startMenu.SetActive(true);
        isDead = false;
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        transform.position = startingPosition;
        health = 100;
        score = 0;
        scoreBoard.text = score.ToString();
        FindObjectOfType<CrystalCollection>().Restart();
        foreach (Spawner spawner in FindObjectsOfType<Spawner>())
        {
            spawner.wipeAllFish();
        }
    }

    public void Win()
    {
        isDead = true;
        winPanel.SetActive(true);
        winScore.text = "Your score was: " + score.ToString();
    }

    public void CountTimer()
    {
        if (gameActive) gameAFKTimer++;
    }
}   
