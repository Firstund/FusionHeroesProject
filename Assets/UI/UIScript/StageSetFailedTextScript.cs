using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NotificationTextScript : MonoBehaviour
{
    [SerializeField]
    private Text text = null;
    [SerializeField]
    private float destroySpeed = 1f;
    [SerializeField]
    private float moveSpeed = 1f;
    private float alpha = 1f;
     

    void Update () {
        Vector3 pos = transform.position;
        pos.y += moveSpeed;
        transform.position = pos;
        DisplayText();
    }
    void DisplayText()
    {
        alpha -= (destroySpeed);
        text.material.color = new Vector4(1, 1, 1, alpha);
        
        if(alpha <= 0f)
            Destroy(gameObject);
    }
}
