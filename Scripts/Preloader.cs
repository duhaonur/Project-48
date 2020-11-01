using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Preloader : MonoBehaviour
{
    private void Start()
    {
        PlayerPrefs.SetInt("CloudSync", 0);
        PlayerPrefs.SetInt("CloudSaveIsActive", 0);

        if (Application.systemLanguage == SystemLanguage.English)
        {
            PlayerPrefs.SetString("Language", "En");
        }
        else if (Application.systemLanguage == SystemLanguage.Turkish)
        {
            PlayerPrefs.SetString("Language", "Tr");
        }
        else
        {
            PlayerPrefs.SetString("Language", "En");
        }

        StartCoroutine(LoadMenu());
    }

    private IEnumerator LoadMenu()
    {
        PlayGamesController.Instance.SignIn();

        yield return new WaitForSeconds(6);

        SceneManager.LoadScene("MenuScene");
    }
}
