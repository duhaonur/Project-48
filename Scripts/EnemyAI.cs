using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NinjaEnemyAI ninjaEnemyAI;
    private DroneAI drone;
    public NavMeshAgent navMesh { get; set; }
    public bool informationSendByBasicEnemy { get; set; }
    public bool playerFoundByBasicEnemy { get; set; }

    public bool stopIfEnemy { get; set; }

    private Transform player;

    private GameObject[] enemy;
    private GameObject[] ninjaEnemy;
    private GameObject[] droneEnemy;

    private Vector3 destination;

    private RaycastHit hit;

    private bool droneExist = false;
    private bool ninjaEnemyExist = false;
    private bool destinationSet = false;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Drone") != null)
        {
            droneExist = true;
        }

        if (GameObject.FindGameObjectWithTag("NinjaEnemy") != null)
        {
            ninjaEnemyExist = true;
        }

        ninjaEnemy = GameObject.FindGameObjectsWithTag("NinjaEnemy");
        droneEnemy = GameObject.FindGameObjectsWithTag("Drone");
        enemy = GameObject.FindGameObjectsWithTag("Enemy");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        navMesh = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, 10f))
        {
            if (hit.collider.gameObject.CompareTag("Player") && stopIfEnemy == false)
            {
                for (int i = 0; i < enemy.Length; i++)
                {
                    GameObject enemyScript;
                    enemyScript = enemy[i];
                    enemyScript.GetComponentInChildren<Light>().color = Color.green;
                    enemyScript.GetComponent<EnemyAI>().playerFoundByBasicEnemy = true;
                    enemyScript.GetComponent<EnemyAI>().stopIfEnemy = true;
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
            }
        }
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.green);
    }

    void FixedUpdate()
    {
        if (!playerFoundByBasicEnemy && !destinationSet)
        {
            navMesh.SetDestination(destination);
            destinationSet = true;
        }
        if(playerFoundByBasicEnemy)
        {
            navMesh.SetDestination(player.position);
            if (PlayerPrefs.GetInt("Level") <= 30)
                navMesh.speed = 10;
            else
                navMesh.speed = 17;
        }
    }

    public IEnumerator DestinationSetter(int minX, int maxX, int minZ, int maxZ)
    {
        if (!playerFoundByBasicEnemy)
        {
            destination = new Vector3(Random.Range(minX, maxX), 3, Random.Range(minZ, maxZ));
            destinationSet = false;
        }

        yield return new WaitForSeconds(10);

        yield return DestinationSetter(minX, maxX, minZ, maxZ);
    }
}
