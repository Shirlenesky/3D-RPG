using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            SavePlayerData();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadPlayerData();
        }
    }


    public void SavePlayerData()
    {
        Save(GameManager.Instance.playerStats.characerData, GameManager.Instance.playerStats.characerData.name);
    }

    public void LoadPlayerData()
    {
        Load(GameManager.Instance.playerStats.characerData, GameManager.Instance.playerStats.characerData.name);
    }

    public void Save(object data, string key)
    {
        var jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.Save(); 
    }
    public void Load(object data, string key)
    {
        if(PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }

}
