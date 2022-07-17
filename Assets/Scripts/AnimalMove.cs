using UnityEngine;
using UnityEngine.AI;

public class AnimalMove : MonoBehaviour
{
	public float moveRange;
	public float navMeshMaxDistance;

	private NavMeshAgent navMeshAgent;
	private Animator animator;

	private Vector3 randomPosition;
	private NavMeshHit navMeshHit;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
	}

    private void Update()
	{
		if (!navMeshAgent.hasPath || (navMeshAgent.velocity.sqrMagnitude > 0.1f && navMeshAgent.remainingDistance <= 0.5f))
        {
			animator.SetBool("Walk", true);

			randomPosition = transform.position + Random.insideUnitSphere * moveRange;

			if (NavMesh.SamplePosition(randomPosition, out navMeshHit, navMeshMaxDistance, NavMesh.AllAreas))
			{
				navMeshAgent.SetDestination(navMeshHit.position);
			}
		}
	}
}
