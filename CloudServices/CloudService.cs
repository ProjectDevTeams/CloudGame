using System.Collections.Generic;
using UnityEngine;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using SaveOptions = Unity.Services.CloudSave.Models.Data.Player.SaveOptions;
using LoadOptions = Unity.Services.CloudSave.Models.Data.Player.LoadOptions;
using PublicWriteAccessClassOptions = Unity.Services.CloudSave.Models.Data.Player.PublicWriteAccessClassOptions;
using PublicReadAccessClassOptions = Unity.Services.CloudSave.Models.Data.Player.PublicReadAccessClassOptions;
using Unity.Services.Authentication;
using UnityEngine.SceneManagement;

public class CloudService : MonoBehaviour
{
    const string SCENE_TEXT = "Scene0";
    public static bool saving;
    private async void Awake()
    {
        await UnityServices.InitializeAsync();

        if (SceneManager.GetActiveScene().name == "Start")
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            LoadData();
            Debug.Log("Player Id:" + AuthenticationService.Instance.PlayerId);
        }
    }

    private void Update()
    {
        if (saving)
        {
            saving = false;
            SaveData();
        }
    }

    public async void SaveData()
    {
        var data = new Dictionary<string, object> { { "data", EconomyManager.saveData } };
        await CloudSaveService.Instance.Data.Player.SaveAsync(data);
    }

    /*public async void SaveDataPublic()
    {
        var data = new Dictionary<string, object> { {"data", EconomyManager.saveData} };
        await CloudSaveService.Instance.Data.Player.SaveAsync(data, new SaveOptions(new PublicWriteAccessClassOptions()));
    }*/

    public async void LoadData()
    {
        var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string>{"data"});

        if (playerData.TryGetValue("data", out var d))
        {
            EconomyManager.saveData = d.Value.GetAs<int[]>();
        }
    }

    /*public async void LoadDataPublic()
    {
        var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string>{"time"}, new LoadOptions(new PublicReadAccessClassOptions()));

        if (playerData.TryGetValue("time", out var keyName))
        {
            Debug.Log($"Time: {keyName.Value.GetAs<string>()}");
        }
    }*/

    public void LoadGame()
    {
        if (EconomyManager.saveData[0] != 0)
        {
            EconomyManager.currentHp = EconomyManager.saveData[0];
            SceneManager.LoadScene(SCENE_TEXT);
        }
    }
}
