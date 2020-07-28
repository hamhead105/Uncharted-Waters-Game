using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public float maxHeight;
    public float minHeight;

    public float speed;
    public float minSize;
    public float maxSize;

    public float visionRange;
    private Rigidbody rb;

    public float minPitch;
    public float maxPitch;

    public float despawnRange;
    public bool isAggressive;
    public float detectionRange;
    public float AttackRange;
    public float damage;

    public float xTargetDirection;
    public float yTargetDirection;

    public float xDirection;
    public float yDirection;
  

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.localScale = new Vector3(Random.Range(minSize, maxSize), Random.Range(minSize, maxSize), Random.Range(minSize, maxSize));
        rb = GetComponent<Rigidbody>();
        //this.transform.parent = GameObject.Find("NPCs").transform;
        player = FindObjectOfType<Player>().transform.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(this.transform.position,FindObjectOfType<Player>().transform.position) > despawnRange)
        {
            Destroy(gameObject);
        }
    
        xDirection = this.transform.rotation.eulerAngles.x;
        yDirection = this.transform.rotation.eulerAngles.y;
    }

    void FixedUpdate()
    {
        rb.AddForce(transform.forward * speed);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, visionRange))
        {
            TurnUp();
            if (Random.Range(0, 1) == 1)
            { TurnRight(); } else { TurnLeft(); }
        }
        if (Physics.Raycast(transform.position, transform.forward, out hit, AttackRange))
        {
            if(hit.transform.GetComponent<Player>())
            {
                hit.transform.GetComponent<Player>().Hit(damage);
            }
        }

        rb.velocity = new Vector3(rb.velocity.x / 4, rb.velocity.y / 4, rb.velocity.z / 4);

        if (Random.Range(0, 80) == 1)
        {
            if (Random.Range(1, 3) == 1)
            { TurnRight(); }
            else { TurnLeft(); }
        }
        if (Random.Range(0, 80) == 1)
        {
            if (Random.Range(1, 3) == 1)
            { TurnUp(); }
            else { TurnDown(); }
        }
        if (transform.position.y > maxHeight)
        {
            TurnDown();
            rb.AddForce(Vector3.down * speed * 5);

        } else if (transform.position.y < minHeight)
        {
            TurnUp();
        }
         this.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
        if (isAggressive && Vector3.Distance(this.transform.position, FindObjectOfType<Player>().transform.position) < detectionRange && FindObjectOfType<Player>().transform.position.y < 300)
        {
            TargetPlayer();
        }
        if (transform.position.y > 300)
        {
            rb.useGravity = true;
            rb.AddForce(0, -200, 0);
            transform.Rotate(-2, 0, 0);
        } else
        {
            rb.useGravity = false;
        }
    }

    void TurnRight()
    {
        rb.angularVelocity += new Vector3(0, Random.Range(-0.3f, -1f), 0);
    }

    void TurnLeft()
    {       
        rb.angularVelocity += new Vector3(0, Random.Range(0.3f, 1f), 0);
    }

    void TurnUp()
    {
        if (transform.localEulerAngles.x > 30)
        {
            rb.angularVelocity += new Vector3(Random.Range(0.3f, 1f), 0, 0);
        }
    }

    void TurnDown()
    {
        if (transform.localEulerAngles.x < -30)
        {
            rb.angularVelocity += new Vector3(Random.Range(-0.3f, -1f), 0, 0);
        }
    }

    void TargetPlayer()
    {
        float adjacent = Mathf.Sqrt(Mathf.Pow(player.transform.position.x - this.transform.position.x, 2) + Mathf.Pow(player.transform.position.z - this.transform.position.z, 2));
        float opposite = player.transform.position.y - this.transform.position.y;
        xTargetDirection = -Mathf.Atan2(opposite, adjacent) * Mathf.Rad2Deg;
        yTargetDirection = Mathf.Atan2(player.transform.position.x - this.transform.position.x, player.transform.position.z - this.transform.position.z) * Mathf.Rad2Deg;
        
        if (xTargetDirection < 0)
        {
            xTargetDirection = 360 + xTargetDirection;
        }
        if (yTargetDirection < 0)
        {
            yTargetDirection = 360 + yTargetDirection;
        }
   
        this.transform.eulerAngles = new Vector3 (xTargetDirection, yDirection + ((yTargetDirection - yDirection) * 0.2f), 0); 
    }

}
