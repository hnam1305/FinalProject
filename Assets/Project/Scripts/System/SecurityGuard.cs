using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SecurityGuard : MonoBehaviour
{
    public float detectionRadius = 10f; 
    public float catchDistance = 1.5f;  
    public float chaseSpeed = 3.5f;     

    private NavMeshAgent agent;
    private Animator animator;
    private Transform targetThief;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = chaseSpeed;
    }

    void Update()
    {
        if (targetThief == null)
        {
       
            DetectThief();
            if (animator != null) animator.SetFloat("Speed", 0f); 
        }
        else
        {
            agent.SetDestination(targetThief.position);

            if (animator != null) animator.SetFloat("Speed", agent.velocity.magnitude); 

            if (Vector3.Distance(transform.position, targetThief.position) <= catchDistance)
            {
                CatchThief();
            }
        }
    }

    private void DetectThief()
    {
        GameObject[] thieves = GameObject.FindGameObjectsWithTag("Thief");

        foreach (GameObject thief in thieves)
        {
            float distance = Vector3.Distance(transform.position, thief.transform.position);
            if (distance <= detectionRadius)
            {
                targetThief = thief.transform;
                Debug.Log("I found u Thief, Stay");
                break; 
            }
        }
    }

    private void CatchThief()
    {
       Debug.Log("Catch you");
        //Destroy(targetThief.gameObject); 
        //targetThief = null;
        agent.ResetPath(); 
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}