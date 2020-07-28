using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] spawnFish;

    public float maxDistance;
    public float minDistance;

    public int spawnChance;
    public int spawnMaximum;

    private Player player;

    public float spawnXZVariation;
    public float spawnYVariation;

    public int maxFish;
    public int fishCount;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnChance", 0.1f, 0.1f);
        player = FindObjectOfType<Player>();
    }

    public void SpawnChance()
    {   
        if (Vector3.Distance(this.transform.position, player.transform.position) > minDistance && Vector3.Distance(this.transform.position, player.transform.position) < maxDistance) 
        {
            if (Random.Range(1, spawnChance) == 1 && CountFish() < maxFish)
            {
                GameObject spawnedFish = Instantiate(spawnFish[Random.Range(0, spawnFish.Length)], this.transform.position, transform.rotation) as GameObject;
                spawnedFish.transform.parent = this.transform;
                spawnedFish.transform.Rotate(0, Random.Range(-180, 180), 0);
                spawnedFish.transform.position += new Vector3(Random.Range(-spawnXZVariation / 2, spawnXZVariation / 2), Random.Range(-spawnYVariation / 2, spawnYVariation / 2), (Random.Range(-spawnXZVariation / 2, spawnXZVariation / 2)));
            }
        }
    }

    public int CountFish()
    {
        return this.transform.childCount;
    }
    public void wipeAllFish()
    {
        foreach (Fish fish in GetComponentsInChildren<Fish>())
        {   
            Destroy(fish.gameObject);           
        }
    }
}
