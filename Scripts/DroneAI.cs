using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneAI : MonoBehaviour
{
    private EnemyAI enemy;
    private NinjaEnemyAI ninjaEnemyAI;
    public bool playerFoundByDrone { get; set; }
    public bool informationSendByDrone { get; set; }
    public bool follow { get; set; }
    public bool stopIfDrone { get; set; }

    private GameObject[] drone;
    private GameObject[] basicEnemy;
    private GameObject[] ninjaEnemy;


    private bool ninjaEnemyExist = false;
    private bool basicEnemyExist = false;

    private Transform player;

    private Rigidbody rb;

    private Vector3 destination;

    private void Start()
    {
        if(GameObject.FindGameObjectWithTag("NinjaEnemy") != null)
        {
            ninjaEnemyExist = true;
        }
        if(GameObject.FindGameObjectWithTag("Enemy") != null)
        {
            basicEnemyExist = true;
        }

        ninjaEnemy = GameObject.FindGameObjectsWithTag("NinjaEnemy");
        basicEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        drone = GameObject.FindGameObjectsWithTag("Drone");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (playerFoundByDrone && stopIfDrone == false)
        {
            for (int i = 0; i < drone.Length; i++)
            {
                GameObject enemyScript;
                enemyScript = drone[i];
                enemyScript.GetComponentInChildren<Light>().color = Color.green;
                enemyScript.GetComponent<DroneAI>().playerFoundByDrone = true;
                enemyScript.GetComponent<DroneAI>().stopIfDrone = true;
                PlayerPrefs.SetInt("WithoutUnseen", 1);
            }
            if (ninjaEnemyExist)
            {
                for (int i = 0; i < ninjaEnemy.Length; i++)
                {
                    GameObject go;
                    go = ninjaEnemy[i];
                    go.GetComponentInChildren<Light>().color = Color.green;
                    go.GetComponent<NinjaEnemyAI>().playerFoundByNinjaEnemy = true;
                    go.GetComponent<NinjaEnemyAI>().stopIfNinjaEnemy = true;
                    PlayerPrefs.SetInt("WithoutUnseen", 1);
                }
            }
            if (basicEnemyExist)
            {
                for (int i = 0; i < basicEnemy.Length; i++)
                {
                    GameObject go;
                    go = basicEnemy[i];
                    go.GetComponentInChildren<Light>().color = Color.green;
                    go.GetComponent<EnemyAI>().playerFoundByBasicEnemy = true;
                    go.GetComponent<EnemyAI>().stopIfEnemy = true;
                    PlayerPrefs.SetInt("WithoutUnseen", 1);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (!playerFoundByDrone)
            rb.MovePosition(Vector3.Lerp(transform.position, destination, 1 * Time.fixedDeltaTime));
        else
            rb.MovePosition(Vector3.Lerp(new Vector3(transform.position.x, 7, transform.position.z), player.position, 1 * Time.fixedDeltaTime));
    }


    public IEnumerator DestinationSetter(int minX, int maxX, int minZ, int maxZ)
    {
        if (!playerFoundByDrone)
        {
            destination = new Vector3(Random.Range(minX, maxX), 7, Random.Range(minZ, maxZ));
        }

        yield return new WaitForSeconds(10);

        yield return DestinationSetter(minX, maxX, minZ, maxZ);
    }
}
