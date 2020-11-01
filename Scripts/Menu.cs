using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Text playButtonText;
    public Text playButtonTextTR;
    public Text cloudSyncText;

    public GameObject cloudSyncPanel;
    public GameObject cloudSyncCloseButton;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Level"))
        {
            if(PlayerPrefs.GetString("Language") == "En")
            {
                playButtonText.text = "PLAY LEVEL " + PlayerPrefs.GetInt("Level");
            }
            else if(PlayerPrefs.GetString("Language") == "Tr")
            {
                playButtonTextTR.text = "SEVİYE " + PlayerPrefs.GetInt("Level") + " OYNA";
            }
        }
        else
        {
            PlayerPrefs.SetInt("Level", 1);
            CloudVariables.ImportantValues[0] = PlayerPrefs.GetInt("Level");
            if (PlayerPrefs.GetString("Language") == "En")
            {
                playButtonText.text = "PLAY LEVEL " + PlayerPrefs.GetInt("Level");
            }
            else if (PlayerPrefs.GetString("Language") == "Tr")
            {
                playButtonTextTR.text = "SEVİYE " + PlayerPrefs.GetInt("Level") + " OYNA";
            }
        }

        PlayerPrefs.SetInt("ShowVideoAdMenu", PlayerPrefs.GetInt("ShowVideoAdMenu") + 1);

        AdManager.Instance.RequestBanner();
        
        if(PlayerPrefs.GetInt("ShowVideoAdMenu") == 4)
        {
            AdManager.Instance.ShowVideoAd();
            PlayerPrefs.SetInt("ShowVideoAdMenu", 0);
            AdManager.Instance.RequestVideoAd();
        }

        if(PlayerPrefs.GetInt("CloudSaveIsActive") == 1)
        {
            if (Social.localUser.authenticated)
            {
                PlayGamesController.Instance.SaveCloud();
            }
            else
            {
                PlayGamesController.Instance.SaveLocal();
            }
        }

        if(PlayerPrefs.GetInt("CloudSync") == 0)
        {
            StartCoroutine(CloudSync());
        }

        Social.ReportScore(PlayerPrefs.GetInt("Level"), GPGSIds.leaderboard_highest_level, (bool success) => { });
    }

    public void LeaderboardUI()
    {
        PlayGamesController.Instance.OnLeaderboardClick();
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("PlayScene");
        AdManager.Instance.DestroyBanner();
    }

    private IEnumerator CloudSync()
    {
        PlayGamesController.Instance.SignIn();

        cloudSyncPanel.SetActive(true);

        if (PlayerPrefs.GetString("Language") == "En")
        {
            cloudSyncText.text = "PLEASE WAIT";
        }
        else if (PlayerPrefs.GetString("Language") == "Tr")
        {
            cloudSyncText.text = "LÜTFEN BEKLEYİN";
        }

        yield return new WaitForSeconds(5);

        if (Social.localUser.authenticated)
        {
            PlayGamesController.Instance.LoadCloud();

            if(PlayerPrefs.GetString("Language") == "En")
            {
                cloudSyncText.text = "SYNCHRONIZING WITH THE CLOUD";
            }
            else if(PlayerPrefs.GetString("Language") == "Tr")
            {
                cloudSyncText.text = "KAYITLI DOSYALARINIZ EŞLEŞTİRİLİYOR";
            }
            

            yield return new WaitForSeconds(5);

            PlayerPrefs.SetInt("CloudSync", 1);
            PlayerPrefs.SetInt("CloudSaveIsActive", 1);

            PlayerPrefs.SetInt("Level", CloudVariables.ImportantValues[0]);

            playButtonText.text = "PLAY LEVEL " + PlayerPrefs.GetInt("Level");

            cloudSyncCloseButton.SetActive(true);
        }
        else
        {
            PlayGamesController.Instance.LoadLocal();

            cloudSyncPanel.SetActive(true);

            if (PlayerPrefs.GetString("Language") == "En")
            {
                cloudSyncText.text = "YOU'RE NOT SIGNED IN WITH PLAY GAME ACCOUNT, LOCAL SAVE IS LOADED.";
            }
            else if (PlayerPrefs.GetString("Language") == "Tr")
            {
                cloudSyncText.text = "PLAY GAME HESABINIZ İLE GİRİŞ YAPMADINIZ, KAYITLI İLERLEMENİZ YÜKLENİYOR.";
            }

            yield return new WaitForSeconds(2);

            PlayerPrefs.SetInt("CloudSync", 1);
            PlayerPrefs.SetInt("CloudSaveIsActive", 1);

            playButtonText.text = "PLAY LEVEL " + PlayerPrefs.GetInt("Level");

            cloudSyncCloseButton.SetActive(true);
        }
    }
}
