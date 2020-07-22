using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapScript : MonoBehaviour
{
    private Transform Player;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void LateUpdate()
    {
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        Vector3 NewPosition = Player.position;
        NewPosition.y = transform.position.y;
        transform.position = NewPosition;
    }
}
