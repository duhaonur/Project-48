using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardEnemy : MonoBehaviour
{
    public NavMeshAgent navMesh { get; set; }

    private RaycastHit hit;

    private GameObject[] guardEnemy;

    private Transform player;

    public bool guardActive { get; set; }

    void Start()
    {
        guardEnemy = GameObject.FindGameObjectsWithTag("GuardEnemy");

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        navMesh = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, 15f))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                for (int i = 0; i < guardEnemy.Length; i++)
                {
                    GameObject go;
                    go = guardEnemy[i];
                    go.GetComponentInChildren<Light>().color = Color.green;
                    go.GetComponent<GuardEnemy>().guardActive = true;
                }
            }
        }
        Debug.DrawRay(transform.position, transform.forward * 15f, Color.green);

        if (guardActive)
        {
            navMesh.SetDestination(player.position);
        }
    }
}
