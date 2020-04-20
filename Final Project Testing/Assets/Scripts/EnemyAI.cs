using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum FSMStates
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Dead
    }

    public FSMStates currentState;

    public float chaseDistance = 5;
    public float attackDistance = 1.5f;
    public float enemySpeed = 2f;
    public GameObject player;
    
    Transform[] wanderPoints;
    Vector3 nextDestination;
    float distanceToPlayer;

    Transform deadTransform;
    bool isDead;
    public GameObject deadEffectPrefab;
    public ParticleSystem LavaBreath;

    int currentDestinationIndex = 0;

    public Transform enemyEyes;
    public float fieldOfView = 45f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Transform[] points = GameObject.FindGameObjectWithTag("WanderPoints").GetComponentsInChildren<Transform>();
        wanderPoints = new Transform[points.Length - 1];
        for (int i = 0; i < wanderPoints.Length; i++)
        {
            wanderPoints[i] = points[i + 1];
        }

        isDead = false;
        gameObject.layer = 2;

        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenuBehavior.isGamePaused) {
            distanceToPlayer = Vector3.Distance(transform.position,
                player.transform.position);

            switch (currentState)
            {
                case FSMStates.Patrol:
                    UpdatePatrolState();
                    break;
                case FSMStates.Chase:
                    UpdateChaseState();
                    break;
                case FSMStates.Attack:
                    UpdateAttackState();
                    break;
                case FSMStates.Dead:
                    UpdateDeadState();
                    break;
            }
        }

    }

    void Initialize()
    {
        currentState = FSMStates.Patrol;
        FindStartPoint();
    }

    void UpdatePatrolState()
    {

        if (Vector3.Distance(transform.position, nextDestination) < 2.5)
        {
            FindNextPoint();
        }
        else if (IsPlayerInClearFOV())
        {
            currentState = FSMStates.Chase;
        }

        FaceTarget(nextDestination);


        transform.position = Vector3.MoveTowards
            (transform.position, nextDestination, enemySpeed * Time.deltaTime);
    }

    void UpdateChaseState()
    {

        nextDestination = player.transform.GetChild(0).transform.position;

        if (distanceToPlayer <= attackDistance)
        {
            currentState = FSMStates.Attack;
        }
        else if (!IsPlayerInClearFOV())
        {
            FindClosestPoint();
            currentState = FSMStates.Patrol;
        }

        FaceTarget(nextDestination);
        
        
        transform.position = Vector3.MoveTowards
            (transform.position, nextDestination, enemySpeed * Time.deltaTime);
            
    }

    void UpdateAttackState()
    {
        nextDestination = player.transform.GetChild(0).transform.position;

        if (distanceToPlayer <= attackDistance)
        {
            currentState = FSMStates.Attack;
        }
        else if (distanceToPlayer > attackDistance && distanceToPlayer <= chaseDistance)
        {
            currentState = FSMStates.Chase;
            if (!LavaBreath.isStopped)
            {
                LavaBreath.Stop();
            }

        }
        else if (distanceToPlayer > chaseDistance)
        {
            currentState = FSMStates.Patrol;
            if (!LavaBreath.isStopped)
            {
                LavaBreath.Stop();
            }
        }

        if (!LavaBreath.isPlaying)
        {
            LavaBreath.Play();
        }
        player.GetComponent<PlayerHealth>().TakeDamage(2);

        FaceTarget(nextDestination);
    }

    void UpdateDeadState()
    {
        isDead = true;
        deadTransform = gameObject.transform;
        Instantiate(deadEffectPrefab, transform.position, transform.rotation);
        gameObject.SetActive(false);
        Destroy(gameObject, 2);
    }

    void FindStartPoint()
    {
        int startIndex = 0;
        float currentDistance = 100000;
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            float distance = Vector3.Distance(spawnPoints[i].transform.position, transform.position);
            if (distance < currentDistance)
            {
                currentDistance = distance;
                startIndex = spawnPoints[i].GetComponent<StartPoint>().StartPointIndex;

            }

        }
        nextDestination = wanderPoints[startIndex].position;
        currentDestinationIndex = (startIndex + 1) % wanderPoints.Length;
    }

    void FindClosestPoint()
    {
        int index = 0;
        float currentDistance = 100000;

        for(int i = 0; i < wanderPoints.Length; i++)
        {
            float distance = Vector3.Distance(wanderPoints[i].position, transform.position);
            if (distance < currentDistance)
            {
                currentDistance = distance;
                index = i;

            }

        }
        nextDestination = wanderPoints[index].position;
        currentDestinationIndex = (index + 1) % wanderPoints.Length;
    }

    void FindNextPoint()
    {
        nextDestination = wanderPoints[currentDestinationIndex].position;
        currentDestinationIndex = (currentDestinationIndex + 1) 
            % wanderPoints.Length;

    }

    void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position).normalized;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp
            (transform.rotation, lookRotation, 10 * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);

        Vector3 frontRayPoint = enemyEyes.position + (enemyEyes.forward * chaseDistance);
        Vector3 leftRayPoint = Quaternion.AngleAxis(-fieldOfView / 2, enemyEyes.forward) * frontRayPoint;
        Vector3 rightRayPoint = Quaternion.AngleAxis(fieldOfView / 2, enemyEyes.forward) * frontRayPoint;

        Debug.DrawLine(enemyEyes.position, frontRayPoint, Color.cyan);
        Debug.DrawLine(enemyEyes.position, leftRayPoint, Color.yellow);
        Debug.DrawLine(enemyEyes.position, rightRayPoint, Color.yellow);
    }

    bool IsPlayerInClearFOV()
    {
        RaycastHit hit;

        Vector3 directionToPlayer = player.transform.position - enemyEyes.position;

        if (Vector3.Angle(directionToPlayer, enemyEyes.forward) <= fieldOfView)
        {
            if (Physics.Raycast(enemyEyes.position, directionToPlayer, out hit, chaseDistance - 1f))
            {
                if (hit.collider.CompareTag("PlayerCapsule"))
                {
                    print("Player in sight!");
                    return true;
                }

                return false;
            }

            return false;
        }

        return false;
    }
}
