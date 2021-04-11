using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpScaleScript : MonoBehaviour
{
    [SerializeField]
    protected float scaleSpeed = 1f;
    [SerializeField]
    protected float disableVector = 1f;
    protected GameManager gameManager = null;
    protected Vector2 fristScale = Vector2.zero;
    [SerializeField]
    protected bool onDisable = false;

    protected void PlusStart()
    {
        gameManager = GameManager.Instance;
        fristScale = transform.localScale;
        transform.localScale = Vector2.zero;
    }
    protected void SetScale()
    {
        if (transform.localScale.x < fristScale.x - 0.2f && transform.localScale.y < fristScale.y - 0.2f && !onDisable)
        {
            transform.localScale = new Vector2(Mathf.Lerp(transform.localScale.x, fristScale.x, Time.deltaTime * scaleSpeed), Mathf.Lerp(transform.localScale.y, fristScale.y, Time.deltaTime * scaleSpeed));
        }
        else if (transform.localScale.x > disableVector && transform.localScale.y > disableVector && onDisable)
        {
            transform.localScale = new Vector2(Mathf.Lerp(transform.localScale.x, 0f, Time.deltaTime * scaleSpeed), Mathf.Lerp(transform.localScale.y, 0f, Time.deltaTime * scaleSpeed));
        }
        else if (!onDisable)
            gameManager.SetCSt(false);
        else
        {
            OnDisable();
            gameObject.SetActive(false);
        }
    }
    protected void OnDisable()
    {
        onDisable = false;
        transform.localScale = Vector2.zero;
    }
    public void OnClickDisable()
    {
        gameManager.SetCSt(true);
        onDisable = true;
    }
}
