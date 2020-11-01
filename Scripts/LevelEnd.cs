using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    private GameObject[] basicEnemy;
    private GameObject[] ninjaEnemy;

    public LevelEnd levelEnd { set; get; }

    public GameObject levelEndPanel;
    public GameObject player;
    public GameObject pauseMenuPanel;
    public GameObject returnButton;
    public GameObject showJoystickPanel;

    public Image backgroundImg;

    public Text levelButtonText;
    public Text levelPFText; // Text for level passed or failed
    public Text withoutUnseenText;

    private bool isLevelEnd = false;
    private bool ninjaEnemyExist = false;

    private float transition = 0.0f;

    private void Start()
    {
        levelEndPanel.SetActive(false);

        basicEnemy = GameObject.FindGameObjectsWithTag("Enemy");

        if (GameObject.FindGameObjectWithTag("NinjaEnemy") != null)
        {
            ninjaEnemy = GameObject.FindGameObjectsWithTag("NinjaEnemy");
            ninjaEnemyExist = true;
        }
    }

    private void Update()
    {
        if (!isLevelEnd)
            return;

        transition += Time.deltaTime;
        backgroundImg.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, transition);
    }

    public void NextLevelButton()
    {
        if (levelButtonText.text == "NEXT LEVEL" || levelButtonText.text == "SONRAKİ SEVİYE")
        {
            SceneManager.LoadScene("PlayScene");
            Time.timeScale = 1f;
        }
    }
    
    public void RetryButton()
    {
        if (levelButtonText.text == "RETRY" || levelButtonText.text == "TEKRAR DENE")
        {
            SceneManager.LoadScene("PlayScene");
            Time.timeScale = 1f;
            PlayerPrefs.SetInt("RetryButtonPressed", 1);
        }
    }

    public void PauseMenu()
    {
        Time.timeScale = 0f;
        pauseMenuPanel.SetActive(true);
        AdManager.Instance.RequestBanner();
        showJoystickPanel.SetActive(false);
    }

    public void ReturnGameButton()
    {
        Time.timeScale = 1f;
        pauseMenuPanel.SetActive(false);
        AdManager.Instance.DestroyBanner();
        showJoystickPanel.SetActive(true);
    }

    public void MenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
        PlayerPrefs.SetInt("ShowVideoAdMenu", PlayerPrefs.GetInt("ShowVideoAdMenu") + 1);
        AdManager.Instance.DestroyBanner();
    }

    public void LevelPassed()
    {
        for (int i = 0; i < basicEnemy.Length; i++)
        {
            GameObject go;
            go = basicEnemy[i];
            go.GetComponent<EnemyAI>().navMesh.isStopped = true;
        }
        if (ninjaEnemyExist)
        {
            for (int i = 0; i < ninjaEnemy.Length; i++)
            {
                GameObject go;
                go = ninjaEnemy[i];
                go.GetComponent<NinjaEnemyAI>().navMesh.isStopped = true;
            }
        }

        if (PlayerPrefs.GetInt("WithoutUnseen") == 0)
        {
            PlayerPrefs.SetInt("WithoutUnseenStreak", PlayerPrefs.GetInt("WithoutUnseenStreak") + 1);

            if(PlayerPrefs.GetString("Language") == "En")
            {
                withoutUnseenText.text = "YOU ESCAPED UNSEEN STREAK: x" + PlayerPrefs.GetInt("WithoutUnseenStreak");
            }
            else if (PlayerPrefs.GetString("Language") == "Tr")
            {
                withoutUnseenText.text = PlayerPrefs.GetInt("WithoutUnseenStreak") + " KEZ GÖRÜLMEDEN KAÇTIN";
            }

            Social.ReportScore(PlayerPrefs.GetInt("WithoutUnseenStreak"), GPGSIds.leaderboard_unseen_streak, (bool success) => { });
        }
        else
        {
            if (PlayerPrefs.GetString("Language") == "En")
            {
                withoutUnseenText.text = "THEY SAW YOU ESCAPING";
            }
            else if (PlayerPrefs.GetString("Language") == "Tr")
            {
                withoutUnseenText.text = "SENİ KAÇARKEN GÖRDÜLER";
            }

            PlayerPrefs.SetInt("WithoutUnseenStreak", 0);
            PlayerPrefs.SetInt("WithoutUnseen", 0);
        }

        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        PlayerPrefs.SetInt("RetryButtonPressed", 0);
        PlayerPrefs.SetInt("ShowVideoAd", PlayerPrefs.GetInt("ShowVideoAd") + 1);
        PlayerPrefs.SetInt("SaveToCloud", PlayerPrefs.GetInt("SaveToCloud") + 1);
        CloudVariables.ImportantValues[0] = PlayerPrefs.GetInt("Level");

        isLevelEnd = true;

        player.GetComponent<CapsuleCollider>().enabled = false;

        Time.timeScale = 0.5f;
        levelEndPanel.SetActive(true);

        if (PlayerPrefs.GetString("Language") == "En")
        {
            levelButtonText.text = "NEXT LEVEL";
            levelPFText.text = "LEVEL COMPLETE";
        }
        else if (PlayerPrefs.GetString("Language") == "Tr")
        {
            levelButtonText.text = "SONRAKİ SEVİYE";
            levelPFText.text = "SEVİYE GEÇİLDİ";
        }

        showJoystickPanel.SetActive(false);

        Social.ReportScore(PlayerPrefs.GetInt("Level"), GPGSIds.leaderboard_highest_level, (bool success) => { });

        if (PlayerPrefs.GetInt("SaveToCloud") == 3)
        {
            if (Social.localUser.authenticated)
            {
                PlayGamesController.Instance.SaveCloud();
                PlayerPrefs.SetInt("SaveToCloud", 0);
            }
            else
            {
                PlayGamesController.Instance.SaveLocal();
                PlayerPrefs.SetInt("SaveToCloud", 0);
            }
        }

        if(PlayerPrefs.GetInt("ShowVideoAd") == 7)
        {
            AdManager.Instance.ShowVideoAd();
            AdManager.Instance.RequestVideoAd();
            PlayerPrefs.SetInt("ShowVideoAd", 0);
        }
    }

    public void LevelFailed()
    {
        for (int i = 0; i < basicEnemy.Length; i++)
        {
            GameObject go;
            go = basicEnemy[i];
            go.GetComponent<EnemyAI>().navMesh.isStopped = true;
        }
        if (ninjaEnemyExist)
        {
            for (int i = 0; i < ninjaEnemy.Length; i++)
            {
                GameObject go;
                go = ninjaEnemy[i];
                go.GetComponent<NinjaEnemyAI>().navMesh.isStopped = true;
            }
        }

        isLevelEnd = true;

        player.GetComponent<CapsuleCollider>().enabled = false;
        PlayerPrefs.SetInt("ShowVideoAd", PlayerPrefs.GetInt("ShowVideoAd") + 1);
        PlayerPrefs.SetInt("SaveToCloud", PlayerPrefs.GetInt("SaveToCloud") + 1);
        CloudVariables.ImportantValues[0] = PlayerPrefs.GetInt("Level");
        showJoystickPanel.SetActive(false);

        if (PlayerPrefs.GetInt("WithoutUnseen") == 1)
        {
            if(PlayerPrefs.GetString("Language") == "En")
            {
                withoutUnseenText.text = "YOU GOT CAUGHT";
            }
            else if(PlayerPrefs.GetString("Language") == "Tr")
            {
                withoutUnseenText.text = "YAKALANDIN";
            }
            
            PlayerPrefs.SetInt("WithoutUnseenStreak", 0);
            PlayerPrefs.SetInt("WithoutUnseen", 0);
        }

        if (PlayerPrefs.GetInt("SaveToCloud") == 3)
        {
            if (Social.localUser.authenticated)
            {
                PlayGamesController.Instance.SaveCloud();
                PlayerPrefs.SetInt("SaveToCloud", 0);
            }
            else
            {
                PlayGamesController.Instance.SaveLocal();
                PlayerPrefs.SetInt("SaveToCloud", 0);
            }
        }

        Time.timeScale = 0.5f;
        levelEndPanel.SetActive(true);

        if (PlayerPrefs.GetString("Language") == "En")
        {
            levelButtonText.text = "RETRY";
            levelPFText.text = "LEVEL FAILED";
        }
        else if (PlayerPrefs.GetString("Language") == "Tr")
        {
            levelButtonText.text = "TEKRAR DENE";
            levelPFText.text = "SEVİYE GEÇİLEMEDİ";
        }

        if (PlayerPrefs.GetInt("ShowVideoAd") == 7)
        {
            AdManager.Instance.ShowVideoAd();
            AdManager.Instance.RequestVideoAd();
            PlayerPrefs.SetInt("ShowVideoAd", 0);
        }
    }
}
