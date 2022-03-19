using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class NPC : MonoBehaviour
{
    const float ENEMY_1_SPEED = 1.5f, ENEMY_1_LIFE = 50,
                ENEMY_2_SPEED = 2.5f, ENEMY_2_LIFE = 100,
                BOSS_ENEMY_SPEED = 3.5f, BOSS_ENEMY_LIFE = 150;

    const int  ENEMY_1_ATTACK = 10,
               ENEMY_2_ATTACK = 20, 
               BOSS_ENEMY_ATTACK = 30;

    private enum NPCType
    {
        enemy1,
        enemy2,
        bossEnemy
    }
    [SerializeField] private NPCType npcType;

    private enum NPCActivity
    {
        moving,
        turning,
        waiting,
        attacking
    }
    [SerializeField] private NPCActivity currentNpcActivity;

    [SerializeField]
    private float moveSpeed, speedMultiplier = 1, activityTimeThresold,
                  activityTimer, currentHealth;
    private int attackAmount;

    private bool isMoving, isTurning, isWaiting, isInAttackMode;

    [Space]
    [Header("Bodies")]
    [SerializeField]
    private GameObject ghostForm;
    [SerializeField]
    private GameObject normalForm;

    [Space]
    public Health enemyHealth;
    [Header("Health")]
    [SerializeField]
    private Image healthBar;
    [SerializeField]
    private GameObject deadVFX;

    private Transform player;
    private Rigidbody rb;
    private CharacterController controller;

    private GameManager gameManager;
    private AudioManager audioManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody>();
        audioManager = gameManager.transform.GetComponent<AudioManager>();
        switch (npcType)
        {
            case NPCType.enemy1:
                moveSpeed = ENEMY_1_SPEED;
                attackAmount = ENEMY_1_ATTACK;
                enemyHealth = new Health(npcType.ToString(), ENEMY_1_LIFE);
                break;
            case NPCType.enemy2:
                moveSpeed = ENEMY_2_SPEED;
                attackAmount = ENEMY_2_ATTACK;
                enemyHealth = new Health(npcType.ToString(), ENEMY_2_LIFE);
                break;
            case NPCType.bossEnemy:
                moveSpeed = BOSS_ENEMY_SPEED;
                attackAmount = BOSS_ENEMY_ATTACK;
                enemyHealth = new Health(npcType.ToString(), BOSS_ENEMY_LIFE);
                break;
            default:
                moveSpeed = ENEMY_1_SPEED;
                enemyHealth = new Health("default", ENEMY_1_LIFE);
                break;
        }
        activityTimeThresold = Random.Range(5.0f, 10.0f);
        transform.localRotation = Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0);
        Move();
    }

    private void Update()
    {
        if(!player)
            player = FindObjectOfType<PlayerMovement>().transform;
        if (transform.position.y > 23)
        {
            transform.position = new Vector3(transform.position.x, 23, transform.position.z);
        }
        if (gameManager.gameState == GameManager.GameState.Playing)
        {
            if (!isInAttackMode)
            {
                speedMultiplier = 1;
                //Behave normally if not in unreal mode
                currentHealth = enemyHealth.CurrentHealthAmount;
                healthBar.transform.parent.gameObject.SetActive(false);

                activityTimer += Time.deltaTime;
                if(activityTimer >= activityTimeThresold)
                {
                    SwitchActivity();
                    activityTimeThresold = Random.Range(5.0f, 10.0f);
                    activityTimer = 0;
                }
                Act();
            }
            else
            {
                Attack();
                healthBar.transform.parent.gameObject.SetActive(true);
                healthBar.fillAmount = enemyHealth.CurrentHealthAmount / enemyHealth.HealthMaxAmount;
            }
            Die();
        }
    }

    private void SwitchActivity()
    {
        int decision = Random.Range(0, 3);
        switch (decision)
        {
            case 0:
                currentNpcActivity = NPCActivity.moving;
                break;
            case 1:
                currentNpcActivity = NPCActivity.waiting;
                break;
            case 2:
                currentNpcActivity = NPCActivity.turning;
                break;
            default:
                //nothing goes here lol
                break;
        }
    }

    private void Act()
    {
        switch (currentNpcActivity)
        {
            case NPCActivity.moving:
                Move();
                break;
            case NPCActivity.turning:
                Turn();
                break;
            case NPCActivity.waiting:
                Wait();
                break;
             default:
                //nothing happens then
                break;
        }
    }

    private void Move()
    {
        isMoving = true;
        isWaiting = false;
        isTurning = false;

        transform.position += transform.TransformDirection(0, 0, moveSpeed * speedMultiplier * Time.deltaTime);

    }

    private void Turn()
    {
        isTurning = true;
        isWaiting = false;
        isMoving = false;

        transform.localRotation = Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0);
        currentNpcActivity = NPCActivity.moving;
        Act();
    }

    private void Wait()
    {
        isWaiting = true;
        isMoving = false;
        isTurning = false;

        // Play any idle animations here
    }

    public void SwitchToAttackMode(bool confirm)
    {
        isInAttackMode = confirm;
        if (!isInAttackMode)
        {
            Turn();
            ActivateNormalBody();
        }
        else
        {
            ActivateGhostBody();
        }
    }

    private void ActivateNormalBody()
    {
        normalForm.SetActive(true);
        ghostForm.SetActive(false);
        GetComponent<MeshRenderer>().enabled = true;
        //GetComponent<Collider>().enabled = true;
    }
    private void ActivateGhostBody()
    {
        normalForm.SetActive(false);
        ghostForm.SetActive(true);
        GetComponent<MeshRenderer>().enabled = false;
        //GetComponent<Collider>().enabled = false;
    }

    private void Attack()
    {
        if (isInAttackMode)
        {
            speedMultiplier = 2;
            Move();
            transform.LookAt(player);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("MapEnd"))
        {
            FlipEnemyDir();
        }

        if (col.gameObject.CompareTag("Player") && isInAttackMode)
        {
            col.gameObject.GetComponent<PlayerManager>().GetPlayerHealth().GetDamage(attackAmount);
            rb.AddForce(-transform.forward * 2f, ForceMode.Impulse);
            audioManager.PlaySFX("Ouch", col.transform.position);
        }
    }

    private void FlipEnemyDir()
    {
        transform.forward = -transform.forward;
    }

    private void Die()
    {
        if(enemyHealth.CurrentHealthAmount <= 0f)
        {
            gameManager.DeductEnemy();
            audioManager.PlaySFX("Burst", transform.position);
            Destroy(Instantiate(deadVFX, transform.position, Quaternion.identity), 3f);
            Destroy(gameObject, 0f);
        }
    }
}
