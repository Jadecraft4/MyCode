using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Slider HealthBar;
    private bool Stop = false;
    public bool IsBlocking = false;
    public bool IsHealing = false;
    public GameObject Player;
    private GameObject[] Enemies;
    private bool Long = false;
    private Rigidbody MyRB;
    private void Awake()
    {
        HealthBar = GameObject.FindGameObjectWithTag("Respawn").GetComponent<Slider>();
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        MyRB = this.gameObject.GetComponent<Rigidbody>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (!IsBlocking)
        {
            if (other.gameObject.name == "P_Bullet(Clone)")
            {
                if (!IsHealing)
                {
                    HealthBar.value -= 5f;
                }
                else
                {
                    HealthBar.value += 2f;
                }
            }
            if (other.gameObject.name == "R_Bullet(Clone)")
            {
                if (!IsHealing)
                {
                    HealthBar.value -= 0.5f;
                }
                else
                {
                    HealthBar.value += 2f;
                }
            }
        }
    }
    private void Update()
    {
        if (!Long && HealthBar.value <= 0)
        {
            MyRB.constraints = RigidbodyConstraints.None;
            if (!Stop)
            {
                MyRB.AddForce(0, 0, 100);
                Stop = true;
            }
            this.gameObject.GetComponent<PlayerMovement>().enabled = false;
            StartCoroutine(SpawnPlayer());
            Long = true;
        }
        
    }
    IEnumerator SpawnPlayer()
    {
        yield return new WaitForSeconds(5f);
        foreach (GameObject E in Enemies)
        {
            if (E.gameObject != null)
            {
                if (Vector3.Distance(GameObject.FindGameObjectWithTag("MainCamera").transform.position, E.transform.position) <= 20)
                {
                    Destroy(E.gameObject);
                }
            }
        }
        GameObject NewPlayer = Instantiate(Player, transform.position, transform.rotation) as GameObject;
        NewPlayer.transform.position = new Vector3(-12, 7, 60);
        NewPlayer.transform.rotation = Quaternion.Euler(0, 0, 0);
        NewPlayer.transform.name = "Player";
        NewPlayer.GetComponent<PlayerMovement>().enabled = true;
        NewPlayer.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        HealthBar.value = 40f;
        Long = false;
        Destroy(this.gameObject);
    }
}
