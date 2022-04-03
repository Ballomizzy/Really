using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    private GameManager gameManager;
    private Transform player;
    private bool foundPlayer;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if(gameManager.gameState == GameManager.GameState.Playing && !foundPlayer)
        {
            player = FindObjectOfType<PlayerManager>().transform;
            foundPlayer = true;
        }

        if (foundPlayer)
        {
            FollowTarget(player);
            RotateWithTarget(player);
        }
    }

    private void FollowTarget(Transform t)
    {
        transform.position = new Vector3(t.position.x, transform.position.y, t.position.z);
    }

    private void RotateWithTarget(Transform t)
    {
        /*transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(), Time.deltaTime * 10);
        transform.up = -(t.forward);
        transform.rotation = new Quaternion(90, transform.rotation.y, 0, 1);*/
        transform.forward = t.forward;
        //transform.rotation = new Quaternion(90, 0, 0, 1);
    }
}
