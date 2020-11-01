using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideControl : MonoBehaviour
{
    public LevelEnd levelEnd;

    private GameObject[] droneObj;

    private void DroneControl()
    {
        droneObj = GameObject.FindGameObjectsWithTag("Drone");

        for (int i = 0; i < droneObj.Length; i++)
        {
            GameObject newDrone;
            newDrone = droneObj[i];
            newDrone.GetComponent<DroneAI>().playerFoundByDrone = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Drone")
        {
            DroneControl();
        }

        if(other.tag == "Enemy")
        {
            levelEnd.LevelFailed();
        }
        else if(other.tag == "NinjaEnemy")
        {
            levelEnd.LevelFailed();
        }
        else if(other.tag == "GuardEnemy")
        {
            levelEnd.LevelFailed();
        }

        if(other.tag == "Finish")
        {
            levelEnd.LevelPassed();
        }
    }
}
