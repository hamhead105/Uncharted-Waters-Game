using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{

    private Collider collider;
    public bool isActive;
    public GameObject light;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(1, 1, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() && isActive)
        {
            other.GetComponent<Player>().Crystal();
            this.GetComponent<MeshRenderer>().enabled = false;
            isActive = false;
            //light.SetActive(false);

        }
    }

    public void Enable()
    {
        this.GetComponent<MeshRenderer>().enabled = true;
        isActive = true;
        //light.SetActive(true);

    }
}
