using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agentMesh;
    public Transform player;
    public LayerMask whatIsGround, whatPlayer;

    //Devriye(Patrolling)
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkRange;
    //Sald�r�(Attack)
    public float cooldownAttakcs;
    public bool attacked;
    //Player ile bulundu�u durum
    public float sightZone, attackRange;
    public bool playerInZone, playerInAttackRange;

    //Ba�lang��ta playerin konumunu al�yorum ve d��man�n hareketlerinin s�nrlar�n� belirleyen NevMeshAgent tan�ml�yorum.
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agentMesh = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //D��man�n alan�nda m�, e�er alan�ndaysa ate� edece�i mesafedemi diye kontrol etme.
        playerInZone = Physics.CheckSphere(transform.position, sightZone, whatPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatPlayer);

        if(!playerInZone && !playerInAttackRange)
        {
            Patrolling();
        }
        if (playerInZone && !playerInAttackRange)
        {
            CatchAndFollowPlayer();
        }
        if (playerInZone && playerInAttackRange)
        {
            AttackPlayer();
        }


    }

    private void Patrolling()
    {
        if(!walkPointSet) 
        {
            SearchWalkPoint();
        }
        if(walkPointSet)
        {
            agentMesh.SetDestination(walkPoint);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Y�r�y�� noktas�na ula��ld� m�
        if(distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        //Rastgele y�r�me noktas� belirleme
        float randZ = Random.Range(-walkRange, walkRange);
        float randX = Random.Range(-walkRange, walkRange);
        walkPoint = new Vector3(transform.position.x + randX, transform.position.y, transform.position.z + randZ);

        if (Physics.Raycast(walkPoint, transform.up, 2f, whatIsGround)) ;
        walkPointSet = true;
    }
    private void CatchAndFollowPlayer()
    {
        agentMesh.SetDestination(player.position);
    }
    private void AttackPlayer()
    {
        agentMesh.SetDestination(transform.position);
        transform.LookAt(player);
        if(!attacked)
        {
            //Sald�r� kodu buraya yaz�lacak
          


            //Sald�r� aras� s�re
            attacked = true;
            Invoke(nameof(ResetAttack), cooldownAttakcs);
        }
    }

  

    private void ResetAttack()
    {
        attacked = false;    
    }

}
