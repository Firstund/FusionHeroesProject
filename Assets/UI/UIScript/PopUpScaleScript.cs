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
    [SerializeField]
    protected Vector2 currentScale = Vector2.zero;
    protected Vector2 maxScale = Vector2.zero;
    [SerializeField]
    protected bool onDisable = false;

    protected void PlusStart()
    {
        gameManager = GameManager.Instance;
        maxScale.x = transform.localScale.x;
        maxScale.y = transform.localScale.y;
        transform.localScale = Vector2.zero;
    }
    protected void SetScale()
    {
        currentScale = transform.localScale;
        if (currentScale.x < maxScale.x - disableVector  && currentScale.y < maxScale.y - disableVector && !onDisable)
        {
            currentScale = new Vector2(Mathf.Lerp(currentScale.x, maxScale.x, Time.deltaTime * scaleSpeed), Mathf.Lerp(currentScale.y, maxScale.y, Time.deltaTime * scaleSpeed));
            Debug.Log("a");
        }
        else if (currentScale.x > disableVector && currentScale.y > disableVector && onDisable)
        {
            currentScale = new Vector2(Mathf.Lerp(currentScale.x, 0f, Time.deltaTime * scaleSpeed), Mathf.Lerp(currentScale.y, 0f, Time.deltaTime * scaleSpeed));
            Debug.Log("b");
        }
        else if (!onDisable)
        {
            gameManager.SetCSt(false);
            Debug.Log("c");
        }
        else if(onDisable)
        {
            OnDisable();
            gameObject.SetActive(false);
            Debug.Log("d");
        }
        transform.localScale = currentScale;
    }
    protected void OnDisable()
    {
        Debug.Log("aa"); // error OnClickDisable이 호출되고나서 OnDisable이 호출되어야 하는데 그러질 않음. 그 결과 처음 실행 이후로 팝업이 나타나자마자 사라짐
        onDisable = false;
        currentScale = Vector2.zero;
        transform.localScale = Vector2.zero;
    }
    public void OnClickDisable()
    {
        Debug.Log("bb");
        gameManager.SetCSt(true);
        onDisable = true;
    }
}
