using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] Enemies;
    private int index;
    private float WaitTime;
    private bool Starting;
    private void Start()
    {
        WaitTime = Random.Range(30,90);
    }
    private void Update()
    {
        if (!Starting)
        {
            StartCoroutine(SpawnEnemy());
            Starting = true;
        }
    }
    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 10)
            {
                yield return new WaitForSeconds(WaitTime);
                WaitTime = Random.Range(30, 150);
                index = Random.Range(0, Enemies.Length);
                GameObject NewEnemy = Instantiate(Enemies[index], transform.position, transform.rotation) as GameObject;
                NewEnemy.transform.position = transform.position;
            }
        }
    }
}
