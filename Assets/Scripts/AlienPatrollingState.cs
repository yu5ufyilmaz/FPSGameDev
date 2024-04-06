using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlienPatrollingState : StateMachineBehaviour
{
    private float timer;
    public float patrollingTime = 10f;

    private Transform player;
    private NavMeshAgent _agent;

    public float detectionArea = 18f;
    public float patrolSpeed = 2f;

    private List<Transform> waypointsList = new List<Transform>();
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        _agent = animator.GetComponent<NavMeshAgent>();

        _agent.speed = patrolSpeed;
        timer = 0;


        GameObject waypointCluster = GameObject.FindGameObjectWithTag("Waypoints");

        foreach (Transform t in waypointCluster.transform)
        {
            waypointsList.Add(t);
        }

        Vector3 nextPosition = waypointsList[Random.Range(0, waypointsList.Count)].position;
        _agent.SetDestination(nextPosition);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (SoundManager.instance.alienChannel.isPlaying == false )
        {
            SoundManager.instance.alienChannel.clip = SoundManager.instance.alienWalking;
            SoundManager.instance.alienChannel.PlayDelayed(1f);
        }
        
        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            _agent.SetDestination(waypointsList[Random.Range(0, waypointsList.Count)].position);
        }

        timer += Time.deltaTime;
        if (timer > patrollingTime)
        {
            animator.SetBool("isPatrolling",false);
        }
        
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < detectionArea)
        {
            animator.SetBool("isChasing",true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent.SetDestination(_agent.transform.position);
        SoundManager.instance.alienChannel.Stop();

    }
}
