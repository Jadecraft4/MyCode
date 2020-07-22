using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    public Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).transform;
    }
    void Update()
    {
        if (player != null)
        {
            transform.position = player.transform.position;
        }else
        {
            player = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).transform;
        }
    }
}
