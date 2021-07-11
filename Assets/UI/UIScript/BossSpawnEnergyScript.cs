using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnEnergyScript : MonoBehaviour
{
    [SerializeField]
    private float scaleSpeed = 1f;
    [SerializeField]
    private float disableVector = 1f;
    [SerializeField]
    private Vector2 currentScale = Vector2.zero;
    private Vector2 maxScale = Vector2.zero;

    private void Start()
    {
        maxScale.x = transform.localScale.x;
        maxScale.y = transform.localScale.y;
        transform.localScale = Vector2.zero;
    }
    private void Update()
    {
        SetScale();
    }
    private void SetScale()
    {
        currentScale = transform.localScale;
        if (currentScale.x < maxScale.x - disableVector && currentScale.y < maxScale.y - disableVector)
        {
            currentScale = new Vector2(Mathf.Lerp(currentScale.x, maxScale.x, Time.deltaTime * scaleSpeed), Mathf.Lerp(currentScale.y, maxScale.y, Time.deltaTime * scaleSpeed));
        }
        else
        {
            OnDisable();
            gameObject.SetActive(false);
        }
        transform.localScale = currentScale;
    }
    private void OnDisable()
    {
        currentScale = Vector2.zero;
        transform.localScale = Vector2.zero;
    }
}
