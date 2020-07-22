using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MoneyManager : MonoBehaviour
{
    public Text MoneyCount;
    public float AmountOfMoney = 0f;

    private void Awake()
    {
        MoneyCount = GameObject.FindGameObjectWithTag("Money").GetComponent<Text>();
    }
    public void AddMoney1()
    {
        AmountOfMoney += 2f;
        MoneyCount.text = AmountOfMoney.ToString();
    }
    public void AddMoney2()
    {
        AmountOfMoney += 20f;
        MoneyCount.text = AmountOfMoney.ToString();
    }
}
