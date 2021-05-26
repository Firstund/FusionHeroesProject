using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    private GameManager gameManager = null;
    private SpawnDespawnScript spawnDespawnScript = null;
    [SerializeField]
    private int tutorialChapterNum = 0;
    [SerializeField]
    private GameObject[] tutorialChapters;
    [SerializeField]
    private float autoTime = 7f;
    private float time = 0f;
    void Start()
    {
        gameManager = GameManager.Instance;
        spawnDespawnScript = GetComponent<SpawnDespawnScript>();
        gameManager.tutoIsPlaying = true;
    }


    void Update()
    {
        time += Time.deltaTime;
        if (Input.GetMouseButtonUp(0) || time > autoTime)
        {
            OnClick();
        }
    }
    private void OnClick()
    {
        if (tutorialChapterNum < tutorialChapters.Length - 1)
        {
            spawnDespawnScript.SetDeSpawnIt(tutorialChapters[tutorialChapterNum]);
            spawnDespawnScript.SetSpawnIt(tutorialChapters[tutorialChapterNum + 1]);

            spawnDespawnScript.OnClick();

            tutorialChapterNum++;
        }
        else
        {
            gameManager.tutoIsPlaying = false;
            Destroy(gameObject);
        }
        time = 0f;
    }
}
