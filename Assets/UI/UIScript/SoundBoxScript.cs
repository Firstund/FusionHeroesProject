using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBoxScript : MonoBehaviour
{
    [SerializeField]
    private float playTime = 3f;
    void Start()
    {
        Invoke("Destroye", playTime);
    }
    void Destroye()
    {
        Destroy(gameObject);
    }

}
