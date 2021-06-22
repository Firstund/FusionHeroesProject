using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Security.Cryptography;
using System;

public class DataManager : MonoBehaviour
{
  
    private readonly string privateKey = "1237ruewyq7y1u23y7dywufq7y23wueyha78y23uy";
    private GameManager gameManager = null;
    private StageManager stageManager = null;
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

    // [SerializeField]
    // private GameObject saveDoneText = null;
    // [SerializeField]
    // private Transform textSpawnPosition = null;

    void Awake()
    {
        gameManager = GameManager.Instance;
        LoadGameData();
    }
    void Start()
    {
        stageManager = FindObjectOfType<StageManager>();
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
            string encryptData = File.ReadAllText(filePath);
            string FromJsonData = Decrypt(encryptData);
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
        string encryptData = Encrypt(ToJsonData);
        string filePath = Application.persistentDataPath + GameDataFileName;
        // 이미 저장된 파일이 있다면 덮어쓰기

        File.WriteAllText(filePath, encryptData);

        //올바르게 저장됐는지 확인

        Debug.Log("경로: " + filePath);
        Debug.Log("저장된 내용: " + ToJsonData);

        // Debug.Log(saveDoneText);
        // Debug.Log(textSpawnPosition);

        Instantiate(stageManager.saveDoneText, stageManager.textSpawnPosition);
        
    }
    private string Encrypt(string data)
    {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
        RijndaelManaged rm = CreateRijndaelManaged();
        ICryptoTransform ct = rm.CreateEncryptor();
        byte[] results = ct.TransformFinalBlock(bytes, 0, bytes.Length);
        return System.Convert.ToBase64String(results, 0, results.Length);
    }
    private string Decrypt(string data)
    {
        byte[] bytes = System.Convert.FromBase64String(data);
        RijndaelManaged rm = CreateRijndaelManaged();
        ICryptoTransform ct = rm.CreateDecryptor();
        byte[] resultsArray = ct.TransformFinalBlock(bytes, 0, bytes.Length);
        return System.Text.Encoding.UTF8.GetString(resultsArray);
    }

    private RijndaelManaged CreateRijndaelManaged()
    {
        byte[] keyArray = System.Text.Encoding.UTF8.GetBytes(privateKey);
        RijndaelManaged result = new RijndaelManaged();

        byte[] newKeysArray = new byte[16];
        System.Array.Copy(keyArray, 0, newKeysArray, 0, 16);

        result.Key = newKeysArray;
        result.Mode = CipherMode.ECB;
        result.Padding = PaddingMode.PKCS7;

        return result;
    }

    // 게임을 종료하면 자동 저장
    private void OnApplicationQuit()
    {
        SaveGameData();
    }
}
