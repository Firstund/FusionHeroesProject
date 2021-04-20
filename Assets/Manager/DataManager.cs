using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private GameManager gameManager = null;
    static GameObject _container;
    static GameObject Container
    {
        get
        {
            return _container;
        }
    }

    static DataManager _instance;
    public static DataManager Instance
    {
        get
        {
            if (!_instance)
            {
                _container = new GameObject();
                _container.name = "DataManager";
                _instance = _container.AddComponent(typeof(DataManager)) as DataManager;
                DontDestroyOnLoad(_container);
            }
            return _instance;
        }
    }

    //--- 게임 데이터 파일이름 설정---
    public string GameDataFileName = "SaveData.json"; // 원하는 이름.jon
    // Start is called before the first frame update
    public SaveData _saveData;
    public SaveData saveData
    {
        get
        {
            if(_saveData == null)
            {
                LoadGameData();
                SaveGameData();
            }
            return _saveData;
        }
    }
    void Awake()
    {
        gameManager = GameManager.Instance;
        LoadGameData();
    }
    void Start()
    {
        
        SaveGameData();
    }
    private void Update()
    {
        _saveData = gameManager.GetSaveData();
    }

    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + GameDataFileName;

        if (File.Exists(filePath))
        {
            Debug.Log("불러오기 성공");
            string FromJsonData = File.ReadAllText(filePath);
            _saveData = JsonUtility.FromJson<SaveData>(FromJsonData);
        }
        else{
            Debug.Log("새로운 파일 생성");
            _saveData = new SaveData();
        }

        gameManager.SetSaveData(_saveData);
    }

    public void SaveGameData()
    {
        string ToJsonData = JsonUtility.ToJson(saveData);
        string filePath = Application.persistentDataPath + GameDataFileName;
        // 이미 저장된 파일이 있다면 덮어쓰기

        File.WriteAllText(filePath, ToJsonData);

        //올바르게 저장됐는지 확인

        Debug.Log("경로: " + filePath);
        Debug.Log("저장된 내용: " + ToJsonData);
    }

    // 게임을 종료하면 자동 저장
    private void OnApplicationQuit()
    {
        SaveGameData();
    }
}
