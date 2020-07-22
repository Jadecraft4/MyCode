using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Spin : MonoBehaviour
{
    private MoneyManager Manager;
    private Transform Myself;
    private Transform Player;
    private Rigidbody MyRigidbody;
    private string MyName;
    private void Start()
    {
        Manager = FindObjectOfType<MoneyManager>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        Myself = this.transform;
        MyRigidbody = this.GetComponent<Rigidbody>();
        MyName = this.name;
    }
    void Update()
    {
        transform.Rotate(0, -50 * Time.deltaTime, 0);
        if (MyName == "Gold" || MyName == "Gold(Clone)" || MyName == "Coin" || MyName == "Coin(Clone)")
        {
            MyRigidbody.AddForce((Player.position - Myself.position).normalized * 3f);
        }
        if (Myself.position.y <= -5)
        {
            Myself.position = new Vector3(-12,7,60);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            switch (this.name)
            {
                case "Gold":
                case "Gold(Clone)":
                    Manager.AddMoney2();
                    Destroy(this.gameObject);
                    break;
                case "Coin":
                case "Coin(Clone)":
                    Manager.AddMoney1();
                    Destroy(this.gameObject);
                    break;
                case "FirstAidKit":
                    GameObject.FindObjectOfType<UI>().HealthBar.value += 5f;
                    Destroy(this.gameObject);
                    break;
            }
        }
    }
}
