using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpPanel : MonoBehaviour
{
    public GameObject blueboxPanel;
    public GameObject basicEnemyPanel;
    public GameObject ninjaEnemyPanel;
    public GameObject dronePanel;
    public GameObject guardEnemyPanel;
    public GameObject joystickPanel;

    private void Start()
    {
        if(PlayerPrefs.GetInt("Level") == 1 && PlayerPrefs.GetInt("Level") < 2)
        {
            basicEnemyPanel.SetActive(true);
            joystickPanel.SetActive(false);
        }
        if (PlayerPrefs.GetInt("Level") == 11 && PlayerPrefs.GetInt("Level") < 12)
        {
            blueboxPanel.SetActive(true);
            joystickPanel.SetActive(false);
        }
        if (PlayerPrefs.GetInt("Level") == 16 && PlayerPrefs.GetInt("Level") < 17)
        {
            dronePanel.SetActive(true);
            joystickPanel.SetActive(false);
        }
        if(PlayerPrefs.GetInt("Level") == 21 && PlayerPrefs.GetInt("Level") < 22)
        {
            ninjaEnemyPanel.SetActive(true);
            joystickPanel.SetActive(false);
        }
        if (PlayerPrefs.GetInt("Level") == 31 && PlayerPrefs.GetInt("Level") < 32)
        {
            guardEnemyPanel.SetActive(true);
            joystickPanel.SetActive(false);
        }
    }
}
