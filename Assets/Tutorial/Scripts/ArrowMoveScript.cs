using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMoveScript : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10f;

    void Update()
    {
        transform.Translate(Vector2.right * Mathf.Sin(Time.time * 2.5f) * moveSpeed * Time.deltaTime);
    }
}
