using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalCollection : MonoBehaviour
{
    public int crystalCount;
    // Start is called before the first frame update
    public void Start()
    {
        crystalCount = this.transform.childCount;
    }

    public void Restart()
    {
        foreach(Crystal crystal in FindObjectsOfType<Crystal>())
        {
            // print("re enabled crystal " + crystal);
            crystal.Enable();
        }
    }
}
