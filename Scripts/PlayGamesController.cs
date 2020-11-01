using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System;

public class PlayGamesController : MonoBehaviour
{
    public static PlayGamesController Instance { set; get; }

    private void Awake()
    {
        if(Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();

        PlayGamesPlatform.InitializeInstance(config);

        PlayGamesPlatform.DebugLogEnabled = true;

        PlayGamesPlatform.Activate();

        //SignIn();
    }

    public void SignIn()
    {
        Social.localUser.Authenticate((bool success) => {

        });
    }

    public void OnLeaderboardClick()
    {
        if (Social.localUser.authenticated)
        {
            Social.ShowLeaderboardUI();
        }
        else
        {
            SignIn();
        }
    }

    private bool isSaving;
    private bool localSave;
    // Making a string out of game data

    string GameDataToString()
    {
        return JsonUtil.CollectionToJsonString(CloudVariables.ImportantValues, "myKey");
    }

    void StringToGameData(string CloudData)
    {
        int[] cloudArray = JsonUtil.JsonStringToArray(CloudData, "myKey", str => int.Parse(str));
        CloudVariables.ImportantValues = cloudArray;
    }

    public void SaveLocal()
    {
        CloudVariables.ImportantValues[0] = PlayerPrefs.GetInt("Level");
    }
    public void LoadLocal()
    {
        PlayerPrefs.SetInt("Level", CloudVariables.ImportantValues[0]);
    }

    public void SaveCloud()
    {
        if (Social.localUser.authenticated)
        {
            isSaving = true;
            localSave = false;
            SaveLocal();
            ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution("GameSave", DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, SaveGameOpened);
        }
        else
        {
            SaveLocal();
        }
    }
    public void LoadCloud()
    {
        isSaving = false;
        ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution("GameSave", DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, SaveGameOpened);
    }

    public void SaveGameOpened(SavedGameRequestStatus status,ISavedGameMetadata game)
    {
        if(status == SavedGameRequestStatus.Success)
        {
            if (isSaving)
            {
                byte[] data = System.Text.ASCIIEncoding.ASCII.GetBytes(GameDataToString());
                TimeSpan totalPlayTime;
                SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder().WithUpdatedPlayedTime(totalPlayTime).WithUpdatedDescription("Saved Game at" + DateTime.Now);
                SavedGameMetadataUpdate update = builder.Build();
                ((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(game, update, data, SavedGameWritten);
            }
            else
            {
                ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(game, SavedGameLoaded);
            }
        }
        else
        {

        }
    }
    public void SavedGameLoaded(SavedGameRequestStatus status, byte[] data)
    {
        if(status == SavedGameRequestStatus.Success)
        {
            StringToGameData(System.Text.ASCIIEncoding.ASCII.GetString(data));
        }
    }

    public void SavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        Debug.Log(status);
    }
    private void OnApplicationQuit()
    {
        SaveCloud();
    }
}
