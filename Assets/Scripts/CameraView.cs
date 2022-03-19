using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraView : MonoBehaviour
{
    Transform player, spawnPoint;
    bool isShowingPlayer;
    GameManager gameManager;
    [SerializeField]
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannel;


    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        cinemachineBasicMultiChannel = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Update()
    {
        if (gameManager.gameState == GameManager.GameState.Playing)
        {
            if (player == null)
                player = GameObject.FindWithTag("Player").transform;
            if (player != null && !isShowingPlayer)
            {
                GetComponent<CinemachineVirtualCamera>().m_LookAt = player;
                GetComponent<CinemachineVirtualCamera>().m_Follow = player;
                isShowingPlayer = true;
            }
        }
        //else
            //MenuView();
    }

    public void ShakeCamera(bool confirm)
    {
        if (player)
        {
            if(confirm)
                cinemachineBasicMultiChannel.m_AmplitudeGain = 1;
            else
                cinemachineBasicMultiChannel.m_AmplitudeGain = 0;
        }
    }
}
