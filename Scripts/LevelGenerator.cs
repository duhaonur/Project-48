using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelGenerator : MonoBehaviour
{
    public NavMeshSurface surface;

    public GameObject ground;
    public GameObject column;
    public GameObject blueColumn;
    public GameObject basicEnemy;
    public GameObject ninjaEnemy;
    public GameObject drone;
    public GameObject guardEnemy;
    public GameObject player;

    private GameObject enemySpawnPoint;
    private GameObject playerSpawnPoint;
    private GameObject[] guardSpawnPoint;

    public float columnCheckRadius = 3f;

    public int randomNumber { get; set; }

    private void Start()
    {
        surface.BuildNavMesh();
    }

    private void Awake()
    {
        if (PlayerPrefs.GetInt("Level") <= 5) {
            LevelTo5();
        }
        else if (PlayerPrefs.GetInt("Level") > 5 && PlayerPrefs.GetInt("Level") <= 10) {
            LevelTo10();
        }
        else if (PlayerPrefs.GetInt("Level") > 10 && PlayerPrefs.GetInt("Level") <= 15) {
            LevelTo15();
        }
        else if (PlayerPrefs.GetInt("Level") > 15 && PlayerPrefs.GetInt("Level") <= 20) {
            LevelTo20();
        }
        else if (PlayerPrefs.GetInt("Level") > 20 && PlayerPrefs.GetInt("Level") <= 25) {
            LevelTo25();
        }
        else if (PlayerPrefs.GetInt("Level") > 25 && PlayerPrefs.GetInt("Level") <= 30) {
            LevelTo30();
        }
        else if (PlayerPrefs.GetInt("Level") > 30 && PlayerPrefs.GetInt("Level") <= 35) {
            LevelTo35();
        }
        else if (PlayerPrefs.GetInt("Level") > 35 && PlayerPrefs.GetInt("Level") <= 40) {
            LevelTo40();
        }
        else if (PlayerPrefs.GetInt("Level") > 40 && PlayerPrefs.GetInt("Level") <= 45) {
            LevelTo45();
        }
        else if (PlayerPrefs.GetInt("Level") > 45 && PlayerPrefs.GetInt("Level") <= 50) {
            LevelTo50();
        }
        else
        {
            if(PlayerPrefs.GetInt("RetryButtonPressed") == 0)
            {
                randomNumber = Random.Range(1, 50);
            }
            else
            {
                randomNumber = PlayerPrefs.GetInt("HoldRandomNumber");
            }

            if(randomNumber <= 10)
            {
                LevelTo30();
            }
            else if(randomNumber > 10 && randomNumber <= 20)
            {
                LevelTo35();
            }
            else if(randomNumber > 20 && randomNumber <= 30)
            {
                LevelTo40();
            }
            else if(randomNumber > 30 && randomNumber <= 40)
            {
                LevelTo45();
            }
            else if(randomNumber > 40 && randomNumber <= 50)
            {
                LevelTo50();
            }
            PlayerPrefs.SetInt("HoldRandomNumber", randomNumber);
        }

        playerSpawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawnPoint");
        player.transform.position = playerSpawnPoint.transform.position;
    }

    private void ColumnSpawn(int columnsToSpawn, int maxSpawnAttemptsPerColumn, GameObject spawningGO, int maxX, int minX, int maxZ, int minZ)
    {
        for (int i = 0; i < columnsToSpawn; i++)
        {

            Vector3 position = Vector3.zero;

            bool validPosition = false;

            int spawnAttempts = 0;

            while (!validPosition && spawnAttempts < maxSpawnAttemptsPerColumn)
            {
                spawnAttempts++;

                position = new Vector3(Random.Range(minX, maxX), 0, Random.Range(minZ, maxZ));

                validPosition = true;

                Collider[] colliders = Physics.OverlapSphere(position, columnCheckRadius);

                foreach (Collider col in colliders)
                {
                    if (col.tag == "Columns")
                    {
                        validPosition = false;
                    }
                }
            }

            if (validPosition)
            {
                Instantiate(spawningGO, position + spawningGO.transform.position, Quaternion.identity);
            }
        }
    }

    private void BlueColumnSpawn(int blueColumnsToSpawn, int maxSpawnAttemptsPerBlueBox, GameObject spawningGO, int minX, int maxX, int minZ, int maxZ)
    {
        for (int i = 0; i < blueColumnsToSpawn; i++)
        {
            Vector3 position = Vector3.zero;

            bool validPosition = false;

            int spawnAttempts = 0;

            while (!validPosition && spawnAttempts < maxSpawnAttemptsPerBlueBox)
            {
                spawnAttempts++;

                position = new Vector3(Random.Range(minX, maxX), 0, Random.Range(minZ, maxZ));

                validPosition = true;

                Collider[] colliders = Physics.OverlapSphere(position, columnCheckRadius);

                foreach (Collider col in colliders)
                {
                    if (col.tag == "Columns")
                    {
                        validPosition = false;
                    }
                }
            }
            if (validPosition)
            {
                Instantiate(spawningGO, position + spawningGO.transform.position, Quaternion.identity);
            }
        }
    }

    private void NinjaEnemy(int amountOfEnemy)
    {
        enemySpawnPoint = GameObject.FindGameObjectWithTag("EnemySpawnPoint");

        for (int i = 0; i < amountOfEnemy; i++)
        {
            Instantiate(ninjaEnemy, enemySpawnPoint.transform.position, Quaternion.identity);
        }
    }
    private void BasicEnemy(int amountOfEnemy)
    {
        enemySpawnPoint = GameObject.FindGameObjectWithTag("EnemySpawnPoint");

        for (int i = 0; i < amountOfEnemy; i++)
        {
            Instantiate(basicEnemy, enemySpawnPoint.transform.position, Quaternion.identity);
        }
    }
    private void Drone(int amountOfEnemy)
    {
        enemySpawnPoint = GameObject.FindGameObjectWithTag("EnemySpawnPoint");

        for (int i = 0; i < amountOfEnemy; i++)
        {
            Instantiate(drone, enemySpawnPoint.transform.position, Quaternion.identity);
        }
    }
    private void GuardEnemy()
    {
        guardSpawnPoint = GameObject.FindGameObjectsWithTag("GuardSpawnPoint");

        for (int i = 0; i < guardSpawnPoint.Length; i++)
        {
            Transform spPosition;
            spPosition = guardSpawnPoint[i].GetComponent<Transform>();
            Instantiate(guardEnemy, spPosition.position, spPosition.transform.rotation);
        }
    }

    private void LevelTo5()
    {
        ground.GetComponent<Transform>().localScale = new Vector3(25, 1, 25);

        Instantiate(ground, new Vector3(0, 0, 0), Quaternion.identity);

        ColumnSpawn(10, 10, column, -10, 10, -8, 8);

        BasicEnemy(5);

        GameObject[] findBasicEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < findBasicEnemy.Length; i++)
        {
            GameObject go;
            go = findBasicEnemy[i];
            StartCoroutine(go.GetComponent<EnemyAI>().DestinationSetter(-5, 5, -5, 5));
        }
    }

    private void LevelTo10()
    {
        ground.GetComponent<Transform>().localScale = new Vector3(40, 1, 40);

        Instantiate(ground, new Vector3(0, 0, 0), Quaternion.identity);

        ColumnSpawn(20, 20, column, -15, 15, -15, 15);

        BasicEnemy(10);

        GameObject[] findBasicEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < findBasicEnemy.Length; i++)
        {
            GameObject go;
            go = findBasicEnemy[i];
            StartCoroutine(go.GetComponent<EnemyAI>().DestinationSetter(-15, 15, -10, 10));
        }
    }

    private void LevelTo15()
    {
        ground.GetComponent<Transform>().localScale = new Vector3(40, 1, 40);

        Instantiate(ground, new Vector3(0, 0, 0), Quaternion.identity);

        ColumnSpawn(20, 20, column, -15, 15, -15, 15);

        BlueColumnSpawn(3, 5, blueColumn, -15, 15, -15, 15);

        BasicEnemy(12);

        GameObject[] findBasicEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < findBasicEnemy.Length; i++)
        {
            GameObject go;
            go = findBasicEnemy[i];
            StartCoroutine(go.GetComponent<EnemyAI>().DestinationSetter(-15, 15, -10, 10));
        }
    }

    private void LevelTo20()
    {
        ground.GetComponent<Transform>().localScale = new Vector3(60, 1, 60);

        Instantiate(ground, new Vector3(0, 0, 0), Quaternion.identity);

        ColumnSpawn(30, 30, column, -25, 25, -22, 22);

        BlueColumnSpawn(3, 5, blueColumn, -25, 25, -22, 22);

        BasicEnemy(20);

        Drone(3);

        GameObject[] findBasicEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < findBasicEnemy.Length; i++)
        {
            GameObject go;
            go = findBasicEnemy[i];
            StartCoroutine(go.GetComponent<EnemyAI>().DestinationSetter(-25, 25, -17, 17));
        }
        GameObject[] findDroneEnemy = GameObject.FindGameObjectsWithTag("Drone");
        for (int i = 0; i < findDroneEnemy.Length; i++)
        {
            GameObject go;
            go = findDroneEnemy[i];
            StartCoroutine(go.GetComponent<DroneAI>().DestinationSetter(-25, 25, -17, 17));
        }
    }

    private void LevelTo25()
    {
        ground.GetComponent<Transform>().localScale = new Vector3(60, 1, 60);

        Instantiate(ground, new Vector3(0, 0, 0), Quaternion.identity);

        ColumnSpawn(30, 30, column, -25, 25, -22, 22);

        BlueColumnSpawn(3, 5, blueColumn, -25, 25, -22, 22);

        BasicEnemy(20);

        NinjaEnemy(3);

        GameObject[] findBasicEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < findBasicEnemy.Length; i++)
        {
            GameObject go;
            go = findBasicEnemy[i];
            StartCoroutine(go.GetComponent<EnemyAI>().DestinationSetter(-25, 25, -17, 17));
        }
        GameObject[] findNinjaEnemy = GameObject.FindGameObjectsWithTag("NinjaEnemy");
        for (int i = 0; i < findNinjaEnemy.Length; i++)
        {
            GameObject go;
            go = findNinjaEnemy[i];
            StartCoroutine(go.GetComponent<NinjaEnemyAI>().DestinationSetter(-25, 25, -17, 17));
        }
    }

    private void LevelTo30()
    {
        ground.GetComponent<Transform>().localScale = new Vector3(80, 1, 80);

        Instantiate(ground, new Vector3(0, 0, 0), Quaternion.identity);

        ColumnSpawn(40, 40, column, -35, 35, -29, 29);

        BlueColumnSpawn(5, 10, blueColumn, -35, 35, -29, 29);

        if(PlayerPrefs.GetInt("Level") <= 50)
        {
            BasicEnemy(20);

            NinjaEnemy(5);

            Drone(3);
        }

        if(PlayerPrefs.GetInt("Level") >= 50)
        {
            int howManyEnemiesToSpawn = 30;
            for (int i = 0; i < howManyEnemiesToSpawn; i++)
            {
                int randomNumber = Random.Range(1, 30);

                if(randomNumber <= 10)
                {
                    BasicEnemy(1);
                }
                else if(randomNumber <= 20 && randomNumber >= 10)
                {
                    NinjaEnemy(1);
                }
                else if(randomNumber <= 30 && randomNumber >= 20)
                {
                    Drone(1);
                }
            }
        }

        GameObject[] findBasicEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < findBasicEnemy.Length; i++)
        {
            GameObject go;
            go = findBasicEnemy[i];
            go.GetComponent<NavMeshAgent>().speed = 5;
            StartCoroutine(go.GetComponent<EnemyAI>().DestinationSetter(-35, 35, -22, 22));
        }
        GameObject[] findNinjaEnemy = GameObject.FindGameObjectsWithTag("NinjaEnemy");
        for (int i = 0; i < findNinjaEnemy.Length; i++)
        {
            GameObject go;
            go = findNinjaEnemy[i];
            go.GetComponent<NavMeshAgent>().speed = 10;
            StartCoroutine(go.GetComponent<NinjaEnemyAI>().DestinationSetter(-35, 35, -22, 22));
        }
        GameObject[] findDroneEnemy = GameObject.FindGameObjectsWithTag("Drone");
        for (int i = 0; i < findDroneEnemy.Length; i++)
        {
            GameObject go;
            go = findDroneEnemy[i];
            StartCoroutine(go.GetComponent<DroneAI>().DestinationSetter(-35, 35, -22, 22));
        }
    }

    private void LevelTo35()
    {
        ground.GetComponent<Transform>().localScale = new Vector3(80, 1, 80);

        Instantiate(ground, new Vector3(0, 0, 0), Quaternion.identity);

        ColumnSpawn(40, 40, column, -35, 35, -29, 29);

        BlueColumnSpawn(5, 10, blueColumn, -35, 35, -29, 29);

        BasicEnemy(30);

        GuardEnemy();

        GameObject[] findBasicEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < findBasicEnemy.Length; i++)
        {
            GameObject go;
            go = findBasicEnemy[i];
            go.GetComponent<NavMeshAgent>().speed = 5;
            StartCoroutine(go.GetComponent<EnemyAI>().DestinationSetter(-35, 35, -22, 22));
        }
    }

    private void LevelTo40()
    {
        ground.GetComponent<Transform>().localScale = new Vector3(100, 1, 100);

        Instantiate(ground, new Vector3(0, 0, 0), Quaternion.identity);

        ColumnSpawn(50, 50, column, -45, 45, -36, 36);

        BlueColumnSpawn(7, 10, blueColumn, -45, 45, -36, 36);

        BasicEnemy(30);

        NinjaEnemy(5);

        GuardEnemy();

        GameObject[] findBasicEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < findBasicEnemy.Length; i++)
        {
            GameObject go;
            go = findBasicEnemy[i];
            go.GetComponent<NavMeshAgent>().speed = 7;
            StartCoroutine(go.GetComponent<EnemyAI>().DestinationSetter(-45, 45, -31, 31));
        }

        GameObject[] findNinjaEnemy = GameObject.FindGameObjectsWithTag("NinjaEnemy");
        for (int i = 0; i < findNinjaEnemy.Length; i++)
        {
            GameObject go;
            go = findNinjaEnemy[i];
            go.GetComponent<NavMeshAgent>().speed = 12;
            StartCoroutine(go.GetComponent<NinjaEnemyAI>().DestinationSetter(-45, 45, -31, 31));
        }
    }

    private void LevelTo45()
    {
        ground.GetComponent<Transform>().localScale = new Vector3(100, 1, 100);

        Instantiate(ground, new Vector3(0, 0, 0), Quaternion.identity);

        ColumnSpawn(50, 50, column, -45, 45, -36, 36);

        BlueColumnSpawn(7, 10, blueColumn, -45, 45, -36, 36);

        BasicEnemy(30);

        Drone(5);

        GuardEnemy();

        GameObject[] findBasicEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < findBasicEnemy.Length; i++)
        {
            GameObject go;
            go = findBasicEnemy[i];
            StartCoroutine(go.GetComponent<EnemyAI>().DestinationSetter(-45, 45, -31, 31));
            go.GetComponent<NavMeshAgent>().speed = 7;
        }

        GameObject[] findDroneEnemy = GameObject.FindGameObjectsWithTag("Drone");
        for (int i = 0; i < findDroneEnemy.Length; i++)
        {
            GameObject go;
            go = findDroneEnemy[i];
            StartCoroutine(go.GetComponent<DroneAI>().DestinationSetter(-45, 45, -31, 31));
        }
    }

    private void LevelTo50()
    {
        ground.GetComponent<Transform>().localScale = new Vector3(100, 1, 100);

        Instantiate(ground, new Vector3(0, 0, 0), Quaternion.identity);

        ColumnSpawn(50, 50, column, -45, 45, -36, 36);

        BlueColumnSpawn(7, 10, blueColumn, -45, 45, -36, 36);

        if (PlayerPrefs.GetInt("Level") <= 50)
        {
            BasicEnemy(30);

            NinjaEnemy(5);

            Drone(5);

            GuardEnemy();
        }
        else
        {
            int howManyEnemiesToSpawn = 45;
            for (int i = 0; i < howManyEnemiesToSpawn; i++)
            {
                int randomNumber = Random.Range(1, 30);

                if (randomNumber <= 10)
                {
                    BasicEnemy(1);
                }
                else if (randomNumber <= 20 && randomNumber >= 10)
                {
                    NinjaEnemy(1);
                }
                else if (randomNumber <= 30 && randomNumber >= 20)
                {
                    Drone(1);
                }
            }
            GuardEnemy();
        }

        GameObject[] findBasicEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < findBasicEnemy.Length; i++)
        {
            GameObject go;
            go = findBasicEnemy[i];
            go.GetComponent<NavMeshAgent>().speed = 7;
            StartCoroutine(go.GetComponent<EnemyAI>().DestinationSetter(-45, 45, -31, 31));
        }

        GameObject[] findNinjaEnemy = GameObject.FindGameObjectsWithTag("NinjaEnemy");
        for (int i = 0; i < findNinjaEnemy.Length; i++)
        {
            GameObject go;
            go = findNinjaEnemy[i];
            go.GetComponent<NavMeshAgent>().speed = 12;
            StartCoroutine(go.GetComponent<NinjaEnemyAI>().DestinationSetter(-45, 45, -31, 31));
        }

        GameObject[] findDroneEnemy = GameObject.FindGameObjectsWithTag("Drone");
        for (int i = 0; i < findDroneEnemy.Length; i++)
        {
            GameObject go;
            go = findDroneEnemy[i];
            StartCoroutine(go.GetComponent<DroneAI>().DestinationSetter(-45, 45, -31, 31));
        }
    }
}
