using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopScript : MonoBehaviour
{
    // publics 
    public AudioSource Player;
    public GameObject GameUI;
    public GameObject ShopUI;
    public ScrollRect scrollRect;
    public GraphicRaycaster raycaster;
    public GameObject[] Weapons;
    public GameObject Location;
    public GameObject NoMoney;
    //privates
    private MoneyManager manager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && Vector3.Distance(this.transform.position, other.transform.position) <= 5f)
        {
            Player.volume = 0f;
            GameUI.SetActive(false);
            ShopUI.SetActive(true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            scrollRect.horizontalNormalizedPosition += Input.GetAxis("Mouse ScrollWheel") * 2f;
        }
        if (Input.GetMouseButtonDown(2))
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);

            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
                //WorldUI is my layer name
                if (results[0].gameObject.layer == LayerMask.NameToLayer("WorldUI"))
                { 
                    if (results[0].gameObject.name == "CrowBar" && manager.AmountOfMoney >= 10)
                    {
                        manager.AmountOfMoney -= 10f;
                        manager.MoneyCount.text = manager.AmountOfMoney.ToString();
                        GameObject crowbar = Instantiate(Weapons[0], Location.transform.position, Location.transform.rotation) as GameObject;
                        crowbar.transform.position = Location.transform.position;
                        crowbar.transform.name = "crowbar";
                        Location.GetComponent<AudioSource>().Play();
                    }else if (results[0].gameObject.name == "Knife" && manager.AmountOfMoney >= 5)
                    {
                        manager.AmountOfMoney -= 10f;
                        manager.MoneyCount.text = manager.AmountOfMoney.ToString();
                        GameObject knife = Instantiate(Weapons[1], Location.transform.position, Location.transform.rotation) as GameObject;
                        knife.transform.position = Location.transform.position;
                        knife.transform.name = "Knife";
                        Location.GetComponent<AudioSource>().Play();
                    }
                    else if (results[0].gameObject.name == "Bat" && manager.AmountOfMoney >= 500)
                    {
                        manager.AmountOfMoney -= 10f;
                        manager.MoneyCount.text = manager.AmountOfMoney.ToString();
                        GameObject bat = Instantiate(Weapons[2], Location.transform.position, Location.transform.rotation) as GameObject;
                        bat.transform.position = Location.transform.position;
                        bat.transform.name = "bat";
                        Location.GetComponent<AudioSource>().Play();
                    }
                    else if (results[0].gameObject.name == "Shuriken" && manager.AmountOfMoney >= 10)
                    {
                        manager.AmountOfMoney -= 10f;
                        manager.MoneyCount.text = manager.AmountOfMoney.ToString();
                        GameObject S = Instantiate(Weapons[3], Location.transform.position, Location.transform.rotation) as GameObject;
                        S.transform.position = Location.transform.position;
                        S.transform.name = "Shuriken1";
                        Location.GetComponent<AudioSource>().Play();
                    }
                    else if (results[0].gameObject.name == "Sword" && manager.AmountOfMoney >= 50)
                    {
                        manager.AmountOfMoney -= 10f;
                        manager.MoneyCount.text = manager.AmountOfMoney.ToString();
                        GameObject Sword = Instantiate(Weapons[4], Location.transform.position, Location.transform.rotation) as GameObject;
                        Sword.transform.position = Location.transform.position;
                        Sword.transform.name = "sword";
                        Location.GetComponent<AudioSource>().Play();
                    }
                    else if (results[0].gameObject.name == "Hammer" && manager.AmountOfMoney >= 100)
                    {
                        manager.AmountOfMoney -= 10f;
                        manager.MoneyCount.text = manager.AmountOfMoney.ToString();
                        GameObject Hammer = Instantiate(Weapons[5], Location.transform.position, Location.transform.rotation) as GameObject;
                        Hammer.transform.position = Location.transform.position;
                        Hammer.transform.name = "hammer";
                        Location.GetComponent<AudioSource>().Play();
                    }
                    else {
                        StartCoroutine(NotEnoughMoney());
                    }
                    results.Clear();
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Player.volume = 0.5f;
        GameUI.SetActive(true);
        ShopUI.SetActive(false);
    }
    private void Start()
    {
        manager = GameObject.FindObjectOfType<MoneyManager>();
    }
    IEnumerator NotEnoughMoney()
    {
        NoMoney.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        NoMoney.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        NoMoney.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        NoMoney.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        NoMoney.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        NoMoney.SetActive(false);
    }
}
