using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewScript : MonoBehaviour
{
    private ScrollRect scrollRect = null;
    [SerializeField]
    private string direction = "";
    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();

        switch (direction)
        {
            case "h":
                for (int i = 0; i < scrollRect.content.childCount; i++)
                {
                    scrollRect.content.sizeDelta += new Vector2(scrollRect.content.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta.x, 0);
                }
                break;
            case "v":
            for (int i = 0; i < scrollRect.content.childCount; i++)
                {
                    scrollRect.content.sizeDelta += new Vector2(0, scrollRect.content.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta.y);
                }
                break;
        }
  //aaaa
    }
}
