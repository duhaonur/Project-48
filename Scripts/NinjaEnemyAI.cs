using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NinjaEnemyAI : MonoBehaviour
{
    private DroneAI drone;
    private EnemyAI enemy;
    public NavMeshAgent navMesh { get; set; }
    public bool playerFoundByNinjaEnemy { get; set; }
    public bool informationSendByNinjaEnemy { get; set; }

    public bool stopIfNinjaEnemy { get; set; }

    private Transform player;

    private GameObject[] ninjaEnemy;
    private GameObject[] basicEnemy;
    private GameObject[] droneEnemy;

    private Vector3 destination;

    private RaycastHit hit;

    private bool basicEnemyExist = false;
    private bool droneExist = false;
    private bool destinationSet = false;

    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Drone") != null)
        {
            droneExist = true;
        }
        if (GameObject.FindGameObjectWithTag("Enemy") != null)
        {
            basicEnemyExist = true;
        }

        basicEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        droneEnemy = GameObject.FindGameObjectsWithTag("Drone");
        ninjaEnemy = GameObject.FindGameObjectsWithTag("NinjaEnemy");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        navMesh = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, 15f))
        {
            if (hit.collider.gameObject.CompareTag("Player") && stopIfNinjaEnemy == false)
            {
                for (int i = 0; i < ninjaEnemy.Length; i++)
                {
                    GameObject enemyScript;
                    enemyScript = ninjaEnemy[i];
                    enemyScript.GetComponentInChildren<Light>().color = Color.green;
                    enemyScript.GetComponent<NinjaEnemyAI>().playerFoundByNinjaEnemy = true;
                    enemyScript.GetComponent<NinjaEnemyAI>().stopIfNinjaEnemy = true;
                    PlayerPrefs.SetInt("WithoutUnseen", 1);
                }
                if (droneExist)
                {
                    for (int i = 0; i < droneEnemy.Length; i++)
                    {
                        GameObject go;
                        go = droneEnemy[i];
                        go.GetComponentInChildren<Light>().color = Color.green;
                        go.GetComponent<DroneAI>().playerFoundByDrone = true;
                        go.GetComponent<DroneAI>().stopIfDrone = true;
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

        Debug.DrawRay(transform.position, transform.forward * 15f, Color.green);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!playerFoundByNinjaEnemy && !destinationSet)
        {
            navMesh.destination = destination;
            destinationSet = true;
        }
        if(playerFoundByNinjaEnemy)
        {
            navMesh.destination = player.position;
            if (PlayerPrefs.GetInt("Level") <= 30)
                navMesh.speed = 14;
            else
                navMesh.speed = 22;
        }
    }

    public IEnumerator DestinationSetter(int minX, int maxX, int minZ, int maxZ)
    {
        if (playerFoundByNinjaEnemy == false)
        {
            destination = new Vector3(Random.Range(minX, maxX), 3, Random.Range(minZ, maxZ));
            destinationSet = false;
        }

        yield return new WaitForSeconds(10);

        yield return DestinationSetter(minX, maxX, minZ, maxZ);
    }
}
