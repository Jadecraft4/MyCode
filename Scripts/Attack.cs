using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    // Publics
    public Transform Position;
    public GameObject Weapon;
    // Privates
    private float AttackNumber;
    private Animator animator;
    private BoxCollider boxCollider;
    private Transform mytransform;
    private bool Locker = false;

    private void Start()
    {
        boxCollider = this.GetComponent<BoxCollider>();
        animator = this.GetComponent<Animator>();
        mytransform = this.GetComponent<Transform>();
    }
    void Update()
    {
        if (mytransform.position.y <= -5)
        {
            mytransform.position = new Vector3(0, 4, 45);
        }
        if (Input.GetButtonDown("special"))
        {
            if (mytransform.parent != null)
            {
                if (mytransform.name == "sword")
                {
                    animator.SetBool("Block", true);
                    if (!GameObject.FindObjectOfType<UI>().IsBlocking)
                    {
                        GameObject.FindObjectOfType<UI>().IsBlocking = true;
                    }
                    CheckAnimator();
                }
                else if (mytransform.name == "hammer" && !Locker && !GameObject.FindObjectOfType<UI>().IsHealing)
                {
                    GameObject.FindObjectOfType<UI>().IsHealing = true;
                    Locker = true;
                    StartCoroutine(WaitTime());
                    CheckAnimator();
                }
                else if (mytransform.name == "crowbar")
                {
                    boxCollider.enabled = true;
                    animator.SetBool("CrowCrit", true);
                    GameObject.FindObjectOfType<PlayerMovement>().CriticalHit = true;
                    CheckAnimator();
                }
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (mytransform.parent != null)
            {
                switch (mytransform.name)
                {
                    case "bat":
                    case "sword":
                        animator.SetBool("SwordAttack", true);
                        CheckAnimator();
                        break;
                    case "hammer":
                        animator.SetBool("BanHammer", true);
                        CheckAnimator();
                        break;
                    case "Knife":
                    case "crowbar":
                        boxCollider.enabled = true;
                        animator.SetBool("Attack", true);
                        CheckAnimator();
                        break;
                }                
                if (mytransform.name == "Shuriken1" && AttackNumber != 5f)
                {
                    GameObject Shuriken = Instantiate(Weapon, transform.position, transform.rotation) as GameObject;
                    Shuriken.transform.position = transform.position;
                    Shuriken.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.forward * 1250));
                    Shuriken.GetComponent<Rigidbody>().AddForce(30, 0, 0);
                    AttackNumber += 1f;
                    Destroy(Shuriken, 2f);
                }
                else if (AttackNumber >= 5f)
                {
                    GameObject.FindObjectOfType<PlayerMovement>().HasWeapon = false;
                    Destroy(this.gameObject);
                }
            }
        }
        if (Input.GetButtonDown("pickup"))
        {
            if (!GameObject.FindObjectOfType<PlayerMovement>().HasWeapon)
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.SphereCast(ray, .5f, out hit, 10f, LayerMask.GetMask("Weapons")))
                {
                    hit.transform.parent = Position;
                    hit.transform.gameObject.GetComponent<BoxCollider>().enabled = false;
                    hit.transform.gameObject.GetComponent<Animator>().enabled = true;
                    GameObject.FindObjectOfType<PlayerMovement>().HasWeapon = true;
                }
            }
        }
        if (Input.GetButtonDown("throw"))
        {
            if (GameObject.FindObjectOfType<PlayerMovement>().HasWeapon && this.transform.parent != null)
            {
                switch (mytransform.name)
                {
                    case "sword":
                    case "hammer":
                    case "bat":
                    case "crowbar":
                        animator.enabled = false;
                        boxCollider.enabled = true;
                        this.gameObject.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.forward * 625));
                        this.gameObject.GetComponent<Rigidbody>().AddForce(30, 0, 0);
                        this.transform.parent = transform.parent.parent.parent.parent;
                        GameObject.FindObjectOfType<PlayerMovement>().HasWeapon = false;
                        break;
                    case "Knife":
                    case "Shuriken1":
                        animator.enabled = false;
                        boxCollider.enabled = true;
                        this.gameObject.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.forward * 1250));
                        this.gameObject.GetComponent<Rigidbody>().AddForce(30, 0, 0);
                        this.transform.parent = transform.parent.parent.parent.parent;
                        GameObject.FindObjectOfType<PlayerMovement>().HasWeapon = false;
                        break;
                }
            }
        }
    }
    public void EnableCollider()
    {
        boxCollider.enabled = true;
    }
    public void DisableCollider()
    {
        boxCollider.enabled = false;
    }
    IEnumerator WaitTime()
    {
        mytransform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        GameObject.FindObjectOfType<UI>().IsHealing = false;
        Locker = false;
        mytransform.GetChild(1).gameObject.SetActive(false);
    }
    private void CheckAnimator()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Slash"))
        {
            boxCollider.enabled = false;
            animator.SetBool("Attack", false);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Slash2"))
        {
            animator.SetBool("SwordAttack", false);
            boxCollider.enabled = false;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("BlockAnimation") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            animator.SetBool("Block", false);
            GameObject.FindObjectOfType<UI>().IsBlocking = false;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Bam") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            animator.SetBool("BanHammer", false);
            GameObject.FindObjectOfType<UI>().IsHealing = false;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("CrowBarCrit") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            boxCollider.enabled = false;
            animator.SetBool("CrowCrit", false);
            GameObject.FindObjectOfType<PlayerMovement>().CriticalHit = false;
        }
    }
}