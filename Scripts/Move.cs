using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class Move : MonoBehaviour
{
    // Publics
    public GameObject[] Moneys;
    public GameObject Bullet;
    public Transform GunTip;
    public string GunType;
    public float AttackDistance = 2f;
    public Animator Animator;
    public Slider Health;
    // Privates
    private int Total;
    private int RandomNumber;
    private Transform Target;
    private NavMeshAgent Nav;
    private bool IsFound = false;
    private bool WanderNow = false;
    private bool StartC = false;
    private string[] GunTypes = { "Pistol", "AK-47" };
    private bool Activated = false;
    private int[] Table = {95, 4, 1};
    private GameObject Money;
    private bool GiveMoney = false;
    private float HealthPoints = 3f;
    private Transform Myself;

    private void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
        Nav = this.gameObject.GetComponent<NavMeshAgent>();
        Myself = this.gameObject.transform;
        foreach (var item in Table)
        {
            Total += item;
        }
        RandomNumber = Random.Range(0, Total);
        for (int i = 0; i < Table.Length; i++)
        {
            if (RandomNumber <= Table[i])
            {
                if (i >= 0 && i < Moneys.Length)
                {
                    Money = Moneys[i];
                }
                else
                {
                    GiveMoney = true;
                }
            }
            else
            {
                RandomNumber -= Table[i];
            }
        }
    }
    void LateUpdate()
    {
        if (Target == null)
        {
            Target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (!Activated)
        {
            Transform[] Children;
            Children = GetComponentsInChildren<Transform>();
            foreach (Transform t in Children)
            {
                for (int i = 0; i < GunTypes.Length; i++)
                {
                    if (t.name.Contains(GunTypes[i]))
                    {
                        GunTip = t.transform.Find("Tip");
                        GunType = t.name;
                        Activated = true;
                    }
                }
            }
        }
        if (Nav.enabled == true)
        {
            if (Vector3.Distance(Target.position, this.transform.position) <= 10 && Target != null)
            {
                if (Animator.GetBool("IsWalking"))
                {
                    Animator.SetBool("IsWalking", false);
                }
                else if (Animator.GetBool("PistolWalk"))
                {
                    Animator.SetBool("PistolWalk", false);
                } 
                if (!IsFound)
                {
                    IsFound = true;
                }
                float Distance = Vector3.Distance(Target.position, transform.position);
                if (Nav.enabled == true && Target != null)
                {
                    Nav.SetDestination(Target.position);
                    Nav.speed = 3.5f;
                    Animator.SetBool("IsRunning", true);
                }
                else
                {
                    Animator.SetBool("IsRunning", false);
                    Target = Myself;
                }
                if (Distance <= AttackDistance)
                {
                    Animator.SetBool("IsRunning", false);
                }
                else
                {
                    Animator.SetBool("IsRunning", true);
                    Target = GameObject.FindGameObjectWithTag("Player").transform;
                }
            }
            if (IsFound)
            {
                if (Animator.GetBool("IsWalking"))
                {
                    Animator.SetBool("IsWalking", false);
                }
                float Distance = Vector3.Distance(Target.transform.position, transform.position);
                if (Nav.enabled && Target != null && IsFound)
                {
                    Nav.SetDestination(Target.position);
                    Nav.speed = 3.5f;
                    Animator.SetBool("IsRunning", true);
                }
                else
                {
                    Animator.SetBool("IsRunning", false);
                    Target = Myself;
                }
                if (Distance <= AttackDistance)
                {
                    Animator.SetBool("IsRunning", false);
                    Target = Myself;
                    if (GunType == "AK-47")
                    {
                        Animator.SetBool("IsShooting", true);
                    }
                    if (GunType == "Pistol")
                    {
                        Animator.SetBool("ShootPistol", true);
                    }
                    transform.LookAt(new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z));
                    if (GunType == "AK-47")
                    {
                        Myself.Rotate(0, 65, 0);
                    }
                    if (GunType == "Pistol")
                    {
                        Myself.Rotate(0, 50, 0);
                    }
                }
                else if (Distance >= AttackDistance)
                {
                    Animator.SetBool("IsShooting", false);
                    Animator.SetBool("ShootPistol", false);
                    Animator.SetBool("IsRunning", true);
                    Target = GameObject.FindGameObjectWithTag("Player").transform;
                }
            }
            if (!IsFound)
            {
                if (!StartC)
                {
                    StartIt();
                    StartC = true;
                }
            }
            if (WanderNow)
            {
                if (!IsFound)
                {
                    // wander
                    if (Nav.enabled)
                    {
                        Vector3 RandomNav = Random.insideUnitSphere * 20f;
                        RandomNav += transform.position;
                        Nav.SetDestination(RandomNav);
                        Nav.speed = 0.5f;
                        if (GunType == "AK-47")
                        {
                            Animator.SetBool("IsWalking", true);
                            if (Vector3.Distance(transform.position, RandomNav) <= 1)
                            {
                                Animator.SetBool("IsWalking", false);
                                RandomNav = transform.position;
                                WanderNow = false;
                                StartC = false;
                            }
                        }
                        if (GunType == "Pistol")
                        {
                            Animator.SetBool("PistolWalk", true);
                            if (Vector3.Distance(transform.position, RandomNav) <= 1)
                            {
                                Animator.SetBool("PistolWalk", false);
                                RandomNav = transform.position;
                                WanderNow = false;
                                StartC = false;
                            }
                        }
                    }
                }
            }
        }
    }
    IEnumerator Wander(float _RandomNumber)
    {
        if (!IsFound)
        {
            yield return new WaitForSeconds(_RandomNumber);
            WanderNow = true;
        }
    }
    public void StartIt()
    {
        if (!IsFound)
        {
            StartCoroutine(Wander(Random.Range(4.5f, 10f)));
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Knife" || other.gameObject.name == "Shuriken1(Clone)" || other.gameObject.name == "sword")
        {
            HealthPoints -= 1.5f;
            Health.value = HealthPoints;
            if (HealthPoints <= 0)
            {
                Nav.enabled = false;
                Animator.enabled = false;
                Health.GetComponentInParent<Canvas>().enabled = false;
                Destroy(this.gameObject, 5f);
                if (!GiveMoney)
                {
                    GameObject NewMoney = Instantiate(Money, this.gameObject.transform.position, this.gameObject.transform.rotation) as GameObject;
                    NewMoney.GetComponent<Rigidbody>().AddForce(0, 15, 0);
                }
                else
                {
                    MoneyMoneyMoney();
                }
            }
        }
        else if (other.gameObject.name == "hammer")
        {
            HealthPoints -= 3f;
            Health.value = HealthPoints;
            if (HealthPoints <= 0)
            {
                Nav.enabled = false;
                Animator.enabled = false;
                Health.GetComponentInParent<Canvas>().enabled = false;
                Destroy(this.gameObject, 5f);
                if (!GiveMoney)
                {
                    GameObject NewMoney = Instantiate(Money, this.gameObject.transform.position, this.gameObject.transform.rotation) as GameObject;
                    NewMoney.GetComponent<Rigidbody>().AddForce(0, 15, 0);
                }
                else
                {
                    MoneyMoneyMoney();
                }
            }
        }else if (other.gameObject.name == "bat")
        {
            HealthPoints -= 3f;
            Health.value = HealthPoints;
            if (HealthPoints <= 0)
            {
                Nav.enabled = false;
                Animator.enabled = false;
                Health.GetComponentInParent<Canvas>().enabled = false;
                Destroy(this.gameObject, 5f);
                Money = Moneys[1];
                MoneyMoneyMoney();
            }
        }else if (other.gameObject.name == "crowbar")
        {
            if (GameObject.FindObjectOfType<PlayerMovement>().CriticalHit)
            {
                HealthPoints -= 3f;
                Health.value = HealthPoints;
                GameObject.FindObjectOfType<UI>().HealthBar.value -= 4f;
                if (HealthPoints <= 0)
                {
                    Nav.enabled = false;
                    Animator.enabled = false;
                    Health.GetComponentInParent<Canvas>().enabled = false;
                    Destroy(this.gameObject, 5f);
                    if (!GiveMoney)
                    {
                        GameObject NewMoney = Instantiate(Money, this.gameObject.transform.position, this.gameObject.transform.rotation) as GameObject;
                        NewMoney.GetComponent<Rigidbody>().AddForce(0, 15, 0);
                    }
                    else
                    {
                        MoneyMoneyMoney();
                    }
                }
            }
            else
            {
                HealthPoints -= 1.5f;
                Health.value = HealthPoints;
                if (HealthPoints <= 0)
                {
                    Nav.enabled = false;
                    Animator.enabled = false;
                    Health.GetComponentInParent<Canvas>().enabled = false;
                    Destroy(this.gameObject, 5f);
                    if (!GiveMoney)
                    {
                        GameObject NewMoney = Instantiate(Money, this.gameObject.transform.position, this.gameObject.transform.rotation) as GameObject;
                        NewMoney.GetComponent<Rigidbody>().AddForce(0, 15, 0);
                    }
                    else
                    {
                        MoneyMoneyMoney();
                    }
                }
            }
        }
    }
    public void Shoot()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            GameObject NewBullet = Instantiate(Bullet, transform.position, transform.rotation) as GameObject;
            NewBullet.transform.position = GunTip.position;
            NewBullet.GetComponent<Rigidbody>().velocity = (GameObject.FindGameObjectWithTag("Player").transform.position - NewBullet.transform.position).normalized * 20;
            Destroy(NewBullet, 0.5f);
        }
    }
    public void MoneyMoneyMoney()
    {
        for (int i = 0; i <= 5; i++)
        {
            GameObject NewMoney = Instantiate(Money, this.gameObject.transform.position, this.gameObject.transform.rotation) as GameObject;
            NewMoney.GetComponent<Rigidbody>().AddForce(Random.Range(10, 25), Random.Range(10, 25), Random.Range(10, 25));
        }
    }
}
