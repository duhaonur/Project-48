using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageController : MonoBehaviour
{
    private void Start()
    {
        if(PlayerPrefs.GetString("Language") == "En")
        {
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        else if(PlayerPrefs.GetString("Language") == "Tr")
        {
            this.gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }

        
    }
}
