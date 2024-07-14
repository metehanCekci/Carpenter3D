using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class SceneReloader : MonoBehaviour
{
    // Singleton instance
    public static SceneReloader Instance { get; private set; }

    // A class to store the initial state of each object
    [System.Serializable]
    public class ObjectState
    {
        public GameObject gameObject;
        public Vector3 initialPosition;
        public Quaternion initialRotation;
        public Vector3 initialScale;
        public bool initialIsFollowing;
        public bool initialIsAttacking;
        public float initialHp;
        public float initialSpeed;

        public ObjectState(GameObject obj)
        {
            gameObject = obj;
            initialPosition = obj.transform.position;
            initialRotation = obj.transform.rotation;
            initialScale = obj.transform.localScale;

            FollowScript followScript = obj.GetComponent<FollowScript>();
            if (followScript != null)
            {
                initialIsFollowing = followScript.isFollowing;
            }
            else
            {
                initialIsFollowing = false;
            }

            EnemyAttack enemyAttack = obj.GetComponent<EnemyAttack>();
            if (enemyAttack != null)
            {
                initialIsAttacking = enemyAttack.isAttacking;
            }

            EnemyHealthScript healthScript = obj.GetComponent<EnemyHealthScript>();
            if (healthScript != null)
            {
                initialHp = healthScript.hp;
            }

            NavMeshAgent agent = obj.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                initialSpeed = agent.speed;
            }
        }
    }

    // List to store initial states of all objects
    public List<ObjectState> initialStates = new List<ObjectState>();

    [SerializeField] Canvas deathMenu;
    public GlobalHpBar hp;
    public ResetBoss rb;
    public PlayerMovement mv;

    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Initialize()
    {
        // Find all objects with the tag "CheckpointObject"
        Tags[] checkpointObjects = FindObjectsOfType<Tags>();

        // Store their initial states
        foreach (Tags obj in checkpointObjects)
        {
            if (obj.HasTag("Reset"))
            {
                initialStates.Add(new ObjectState(obj.gameObject));
            }
            if (obj.HasTag("Enemy"))
            {
                FollowScript followScript = obj.GetComponent<FollowScript>();
                if (followScript != null)
                {
                    followScript.isFollowing = false;
                }
                NavMeshAgent agent = obj.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    agent.speed = followScript.maxSpeed;
                }
            }
            if (obj.HasTag("Music"))
            {
                musicSwapper musicSwapper = obj.GetComponent<musicSwapper>();
                if (musicSwapper != null)
                {
                    musicSwapper.isCombat = false;
                }
            }
            if (obj.HasTag("FightTrigger"))
            {
                EnemyAlert enemyAlert = obj.GetComponent<EnemyAlert>();
                if (enemyAlert != null)
                {
                    enemyAlert.resetTrigger();
                }
            }
            if (obj.HasTag("Creature"))
            {
                EnemyAttack enemyAttack = obj.GetComponent<EnemyAttack>();
                if (enemyAttack != null)
                {
                    enemyAttack.isAttacking = false;
                    
                }

                EnemyHealthScript healthScript = obj.GetComponent<EnemyHealthScript>();
                if (healthScript != null)
                {
                    healthScript.hp = healthScript.maxhp;
                }

                GlobalHpBar globalHpBar = obj.GetComponent<GlobalHpBar>();
                if (globalHpBar != null)
                {
                    globalHpBar.hp = globalHpBar.maxHp;
                }
            }

            if(obj.HasTag("ResetPlayer"))
            {
                BonfireManager(obj.gameObject);
            }
        }
    }

    // Function to reset all objects to their initial states
    public void ResetToCheckpoint()
    {
        deathMenu.gameObject.SetActive(false);
        hp.hp = hp.maxHp;
        hp.isDead = false;
        mv.enabled = true;

        try
        {
            rb.Reset();
        }
        catch { }

        Time.timeScale = 1;

        foreach (ObjectState state in initialStates)
        {
            state.gameObject.transform.position = state.initialPosition;
            state.gameObject.transform.rotation = state.initialRotation;
            state.gameObject.transform.localScale = state.initialScale;
            state.gameObject.SetActive(true);

            FollowScript followScript = state.gameObject.GetComponent<FollowScript>();
            if (followScript != null)
            {
                followScript.isFollowing = state.initialIsFollowing;
            }

            NavMeshAgent agent = state.gameObject.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.speed = state.initialSpeed;
            }

            Rigidbody rb = state.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            Animator animator = state.gameObject.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Rebind();
                animator.Update(0f);
            }

            EnemyAttack enemyAttack = state.gameObject.GetComponent<EnemyAttack>();
            if (enemyAttack != null)
            {
                enemyAttack.isAttacking = state.initialIsAttacking;
            }

            EnemyHealthScript healthScript = state.gameObject.GetComponent<EnemyHealthScript>();
            if (healthScript != null)
            {
                healthScript.hp = state.initialHp;
            }

            GlobalHpBar globalHpBar = state.gameObject.GetComponent<GlobalHpBar>();
            if (globalHpBar != null)
            {
                globalHpBar.hp = state.initialHp;
            }

            EnemyAlert enemyAlert = state.gameObject.GetComponent<EnemyAlert>();
            if (enemyAlert != null)
            {
                enemyAlert.enabled = true;
            }

            musicSwapper musicSwapper = state.gameObject.GetComponent<musicSwapper>();
            if (musicSwapper != null)
            {
                musicSwapper.isCombat = false;
            }

            Initialize();
        }
    }

public void BonfireManager(GameObject Player)
{



    ReadJson.Instance.ReadSaveFile();

    if (ReadJson.Instance.saveFile == null)
    {
        Debug.LogError("SaveFile is null");
        return;
    }

    int ID = ReadJson.Instance.saveFile.LastBonfireID;

    if (ReadJson.Instance.saveFile.bonfires == null || ReadJson.Instance.saveFile.bonfires.Count == 0)
    {
        Debug.LogError("Bonfires list is null or empty");
        return;
    }

    if (ID < 0 || ID >= ReadJson.Instance.saveFile.bonfires.Count)
    {
        Debug.LogError($"Invalid Bonfire ID: {ID}");
        return;
    }

    Player.transform.position = ReadJson.Instance.saveFile.bonfires[ID].BonfirePos;
    Debug.Log($"Player position set to bonfire ID: {ID} at position: {ReadJson.Instance.saveFile.bonfires[ID].BonfirePos}");
}

}
