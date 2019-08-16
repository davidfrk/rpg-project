using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    string playerDataPath;
    public List<Unit> playerUnits;

    void Start()
    {
        playerDataPath = Path.Combine(Application.persistentDataPath, "PlayerData.txt");
        //Debug.Log("Saved in " + Application.persistentDataPath);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SavePlayer();
            foreach(Unit unit in playerUnits)
            {
                SaveCharacter(unit);
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadPlayer();
            foreach(Unit unit in playerUnits)
            {
                LoadCharacter(unit);
            }
        }
    }

    public void SavePlayer()
    {
        PlayerData data = new PlayerData();
        data.GetData(PlayerController.localPlayer);

        SaveSystem.SavePlayerData(data, playerDataPath);
    }

    public void LoadPlayer()
    {
        PlayerData playerData = SaveSystem.LoadPlayerData(playerDataPath);
        playerData.Apply(PlayerController.localPlayer);
    }

    public void SaveCharacter(Unit unit)
    {
        CharacterData data = new CharacterData();
        data.GetData(unit);

        string path = Path.Combine(Application.persistentDataPath, unit.name + ".txt");
        SaveSystem.SaveCharacter(data, path);
    }

    public void LoadCharacter(Unit unit)
    {
        string path = Path.Combine(Application.persistentDataPath, unit.name + ".txt");
        CharacterData data = SaveSystem.LoadCharacter(path);
        data.Apply(unit);
    }
}
