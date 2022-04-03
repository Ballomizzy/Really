using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine;
using System.Linq;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    public bool isUnrealMode { get; private set; }

    [Header("Post Processing")]
    [SerializeField]
    private PostProcessVolume realWorld, unrealWorld;

    [Space]
    [Header("Lighting")]
    [SerializeField]
    private Light sun;

    [Header("Player")]
    [SerializeField]
    private PlayerManager player;

    [SerializeField]
    private float panicBarAmount;

    [SerializeField]
    private GameObject panicBarUI;

    [SerializeField]
    private Image panicBar;

    private GameUIManager gameUIManager;
    private GameManager gameManager;
    private WeaponManager weaponManager;
    private AudioManager audioManger;
    private CameraView vCam;

    private List<WeaponPickup> weaponPickupList = new List<WeaponPickup>();

    private void Awake()
    {
        gameUIManager = FindObjectOfType<GameUIManager>();
        gameManager = FindObjectOfType<GameManager>();
        audioManger = gameManager.GetComponent<AudioManager>();
        vCam = FindObjectOfType<CameraView>();

        player = gameManager.playerInstance;
        panicBar = gameUIManager.panicBar;
        panicBarUI = gameUIManager.panicBarUI;
        realWorld = gameManager.realWorld;
        unrealWorld = gameManager.unrealWorld;

        SetDefaultVariables();
    }

    private void SetDefaultVariables()
    {
        realWorld.gameObject.SetActive(true);
        unrealWorld.gameObject.SetActive(false);

        sun.gameObject.SetActive(true);

        NPC[] allNPCs = FindObjectsOfType<NPC>();
        for (int i = 0; i < allNPCs.Length; i++)
        {
            allNPCs[i].SwitchToAttackMode(false);
        }
        weaponPickupList = FindObjectsOfType<WeaponPickup>().ToList();
        HidePickupWeapons();
    }

    private void Update()
    {
        if(!player)
            player = gameManager.playerInstance;
        if (!weaponManager)
        {
            weaponManager = FindObjectOfType<WeaponManager>();
            weaponManager.gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchWorlds();
        }
        PanicBar();
    }

    public void SwitchWorlds()
    {
        isUnrealMode = !isUnrealMode;
        if (isUnrealMode)
        {
            realWorld.gameObject.SetActive(false);
            unrealWorld.gameObject.SetActive(true);

            sun.gameObject.SetActive(false);

            weaponManager.gameObject.SetActive(true);

            NPC[] allNPCs = FindObjectsOfType<NPC>();
            for(int i = 0; i < allNPCs.Length; i++)
            {
                allNPCs[i].SwitchToAttackMode(true);
            }
            panicBarAmount = 100;

            audioManger.SwitchMusic(true);
            vCam.ShakeCamera(true);
        }
        else
        {
            realWorld.gameObject.SetActive(true);
            unrealWorld.gameObject.SetActive(false);

            sun.gameObject.SetActive(true);

            weaponManager.gameObject.SetActive(false);

            NPC[] allNPCs = FindObjectsOfType<NPC>();
            for (int i = 0; i < allNPCs.Length; i++)
            {
                allNPCs[i].SwitchToAttackMode(false);
            }

            audioManger.SwitchMusic(false);
            vCam.ShakeCamera(false);
        }
        HidePickupWeapons();
    }

    private void HidePickupWeapons()
    {
        for (int i = 0; i < weaponPickupList.Count; i++)
        {
            weaponPickupList[i].gameObject.SetActive(isUnrealMode);
        }
    }

    public void AddPickUpWeapons(WeaponPickup weaponPickup)
    {
        weaponPickupList.Add(weaponPickup);
    }

    public void RemovePickUpWeapons(WeaponPickup weaponPickup)
    {
        weaponPickupList.Remove(weaponPickup);
    }

    private void PanicBar()
    {
        if (isUnrealMode)
        {
            if(gameManager.gameState == GameManager.GameState.Playing)
            {
                panicBarUI.gameObject.SetActive(true);
                panicBarAmount -= 5 * Time.deltaTime; //when adding level, use it here
                panicBar.fillAmount = panicBarAmount / (float)100;

                if(panicBarAmount <= 0)
                {
                    player.Die();
                    gameManager.LoseGame();
                }
            }
        }
        else
        {
            panicBarUI.gameObject.SetActive(false);
            panicBarAmount = 0;
        }
    }
}
