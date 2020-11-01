using UnityEngine;

public class CloudVariables : MonoBehaviour
{
    public static int[] ImportantValues { get; set; }

    private void Awake()
    {
        ImportantValues = new int[1];

        if (PlayerPrefs.HasKey("Level"))
        {
            Debug.Log("Has Save File");
            ImportantValues[0] = PlayerPrefs.GetInt("Level");
        }
        else
        {
            PlayerPrefs.SetInt("Level", 1);
            ImportantValues[0] = PlayerPrefs.GetInt("Level");
        }

        //ImportantValues[0] = PlayerPrefs.GetInt("Level");
    }
}
