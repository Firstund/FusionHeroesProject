using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatUpScript : MonoBehaviour
{
    private GameManager gameManager = null;
    [SerializeField]
    private string scriptName = ""; // 실행시킬 this내의 함수의 이름
    [SerializeField]
    private string methodName = ""; // scriptName의 이름을 가진 함수 내에서 특정 스크립트 내에서 호출할 함수의 이름
    private SaveData saveData; // 스탯 레벨을 저장하는 용도의 saveData
    [SerializeField]
    private int upgradeCost = 100; // 해당 스탯의 레벨을 올릴 때 사용해야하는 gold의 양
    [SerializeField]
    private float upgradeStat = 100f; // 해당 스탯의 레벨을 올렸을 때, 올라갈 스탯의 양
    
    private void Start()
    {
        gameManager = GameManager.Instance;
        saveData = gameManager.GetSaveData();
    }
    public void OnClick()
    {
        this.SendMessage(scriptName,SendMessageOptions.DontRequireReceiver);
    }
    public void BuildingUpgrade() // 이 함수 내에서 BuildingUpgrade를 수행
    {
       Debug.Log("Aaaaa"); 
    }
}
