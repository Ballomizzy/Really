using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int level, noOfEnemies = 3, initialAmtOfEnemies = 0; //false number so it does not auto win when enemy starts with 0;
    private float currentTimer, timerThresold;
    public enum GameState
    {
        NotStarted,
        Playing,
        Ended
    }
    public GameState gameState;
    private bool isGameJustStarted, isLost, isWon;

    private GameUIManager UIManager;

    [Header("Levels")]
    [SerializeField]
    private GameObject menuMap;
    [SerializeField]
    private GameObject menuView;

    [SerializeField]
    private List<LevelData> levelMaps = new List<LevelData>();

    [System.Serializable]
    private class LevelData
    {
        public int Level;
        public GameObject LevelMapGameObject;
    }

    [Space]
    [Header("Game Prefabs")]
    [SerializeField]
    private GameObject playerPrefab;
    public PlayerManager playerInstance;

    [Space]
    [Header("Post Processing")]
    [SerializeField]
    public PostProcessVolume realWorld, unrealWorld;


    private AudioManager audioManager;


    private void Awake()
    {
        UIManager = GetComponent<GameUIManager>();
        audioManager = GetComponent<AudioManager>();
        Time.timeScale = 1;
    }

    public void PressStart()
    {
        StartGame(UIManager.GetSelectedLevel());
    }

    private void StartGame(int level)
    {
        //Set UI Ready
        menuMap.SetActive(false);
        menuView.SetActive(false);
        UIManager.SwitchUI(GameUIManager.UIState.GameUI);

        audioManager.PlaySFX("Whoosh");

        //Set Game to start
        gameState = GameState.Playing;
        isGameJustStarted = true;

        //Fill variables
        SetVariables(level);
    }

    private void Update()
    {
        //Spawn world when game starts
        if (isGameJustStarted)
        {
            SetWorld();
            isGameJustStarted = false;
        }

        if (gameState == GameState.Playing)
        {
            //Reduce timer in game
            currentTimer -= Time.deltaTime;
            UIManager.UpdateTimerUI(currentTimer);

            //Check if player loses
            if (!isWon && playerInstance.GetPlayerHealth().CurrentHealthAmount <= 0f || currentTimer <= 0f)
            {
                LoseGame();
            }
            //or wins...
            if (noOfEnemies < 1 && !isLost)
            {
                WinGame();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Game");
            }
        }
    }

    private void SetWorld()
    {
        //Spawn Map
        Instantiate(levelMaps[level - 1].LevelMapGameObject, Vector3.zero, Quaternion.identity);

        //Spawn Player
        playerInstance = Instantiate(playerPrefab, GameObject.FindWithTag("SpawnPoint").transform.position, Quaternion.identity).GetComponent<PlayerManager>();

        //How many enemies?
        noOfEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        initialAmtOfEnemies = noOfEnemies;
        UIManager.enemyAmount.text = noOfEnemies.ToString() + "/" + initialAmtOfEnemies.ToString();
    }

    public void LoseGame()
    {
        isLost = true;
        //Stop Game
        gameState = GameState.Ended;
        //Display Lose
        UIManager.SwitchUI(GameUIManager.UIState.LoseUI);

        //Deactivate all npcs
        GameObject[] allNPCS = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i < allNPCS.Length; i++)
        {
            allNPCS[i].GetComponent<Rigidbody>().isKinematic = false; ;
        }
        Rigidbody playerRB = playerInstance.GetComponent<Rigidbody>();
        playerRB.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY 
                               | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        playerInstance.GetComponent<Animator>().SetTrigger("die");
        audioManager.PlaySFX("Lose");
    }

    private void WinGame()
    {
        isWon = true;
        //Stop Game
        gameState = GameState.Ended;
        //Display Win
        UIManager.SwitchUI(GameUIManager.UIState.WinUI);

        audioManager.PlaySFX("Win");
    }

    public void DeductEnemy()
    {
        noOfEnemies--;
        UIManager.enemyAmount.text = noOfEnemies.ToString() + "/" + initialAmtOfEnemies.ToString();
    }

    private void SetVariables(int levelData)
    {
        switch (levelData)
        {
            case 1:
                level = 1;
                timerThresold = 40;
                break;
            case 2:
                level = 2;
                timerThresold = 60;
                break;
            case 3:
                level = 3;
                timerThresold = 90;
                break;
            case 4:
                level = 4;
                timerThresold = 120;
                break;
            case 5:
                level = 5;
                timerThresold = 150;
                break;
            default:
                //nothing!
                break;

        }
        currentTimer = timerThresold;
        UIManager.UpdateLevelText(level);
    }

}
