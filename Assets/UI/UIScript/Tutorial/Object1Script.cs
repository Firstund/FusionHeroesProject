using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object1Script : MonoBehaviour
{
    [SerializeField]
    private Transform targetTrm = null;
    [SerializeField]
    private float speed = 1f;
    private Vector2 currentPosition = Vector2.zero;
    private Vector2 originPosition = Vector2.zero;
    private Vector2 targetPosition = Vector2.zero;
    [SerializeField]
    private float comeBackDistance = 1f;

    private float distance = 0f;
    
    void Start()
    {
        currentPosition = transform.position;
        originPosition = currentPosition;
        targetPosition = targetTrm.position;
    }

    void Update()
    {
        currentPosition = transform.position;
        
        currentPosition = Vector2.Lerp(currentPosition, targetPosition, speed * Time.deltaTime);
        distance = Vector2.Distance(currentPosition, targetPosition);

        if(distance < comeBackDistance)
        {
            currentPosition = originPosition;
        }

        transform.position = currentPosition;
    }
}
