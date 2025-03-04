using Pathfinding.BehaviourTrees;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class Monster : MonoBehaviour
{
    [SerializeField] bool Ai_on = true;
    [SerializeField] private LayerMask obstacleLayerMask;
    [SerializeField] List<Transform> waypoints = new();
    [SerializeField] private float viewDistance = 10f;
    [SerializeField] private Transform player;

    Animator animator;
    NavMeshAgent agent;
    BehaviourTree btree;

    void Awake()
    {
        obstacleLayerMask = LayerMask.GetMask("Obstacle");
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        btree = new BehaviourTree("leif");
        PrioritySelector actions = new("Agent logic");
        Sequence hunt = new("HuntPlayerSequence", 100);

        bool IsPlayerVisible()
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer < viewDistance)
            {
                if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleLayerMask))
                {
                    hunt.Reset();
                    return true;
                }
            }
            return false;
        }

        hunt.AddChild(new Leaf("SeesPlayer?", new Condition(IsPlayerVisible)));
        hunt.AddChild(new Leaf("Chase player", new MoveToTarget(transform, agent, player)));
        actions.AddChild(hunt);
        
        // Getting a random point to walk to
        RandomPoint(gameObject.transform.position, 15f, out Vector3 walkPoint);

        Leaf patrol = new Leaf("Patrolling", new PatrolStrategy(transform, agent, waypoints));
        actions.AddChild(patrol);

        btree.AddChild(actions);
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * range;
        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }
        result = Vector3.zero;
        return false;
    }

    // Update is called once per frame
    void Update()
    {

            btree.Process(); // Kör beteendeträdet
       
    }
}
