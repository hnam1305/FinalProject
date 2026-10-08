using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public enum NPCState
{
    Wandering,
    PickingUpItem,
    GoingToCheckout,
    Leaving
}

[RequireComponent(typeof(NavMeshAgent))]
public class NPCSmartWander : MonoBehaviour
{
    public float walkSpeed = 1.5f;
    public List<PointOfInterest> availablePOIs = new List<PointOfInterest>();

    public Transform checkoutCounter;
    public Transform exitDoor;

    private NavMeshAgent agent;
    private NPCState currentState = NPCState.Wandering;
    private int itemsCollected = 0;
    public int maxItemsToBuy = 1;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = walkSpeed;
        FindNextItem();
    }

    void Update()
    {
        switch (currentState)
        {
            case NPCState.Wandering:
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    StartCoroutine(HandlePickup());
                }
                break;

            case NPCState.PickingUpItem:
                break;

            case NPCState.GoingToCheckout:
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    StartCoroutine(HandleCheckout());
                }
                break;

            case NPCState.Leaving:
                if (!agent.pathPending && agent.remainingDistance <= 1.0f)
                {
                    Destroy(gameObject);
                }
                break;
        }
    }

    private void FindNextItem() // ngẫu nhiên tìm vật phẩm để đi thanh toán
    {
        if (availablePOIs.Count == 0) return;

        currentState = NPCState.Wandering;
        Transform target = availablePOIs[Random.Range(0, availablePOIs.Count)].transform;
        agent.SetDestination(target.position);
    }

    private IEnumerator HandlePickup()// quét vật phẩm và nhặt
    {
        currentState = NPCState.PickingUpItem;
        yield return new WaitForSeconds(2f);

        itemsCollected++;

        if (itemsCollected >= maxItemsToBuy && checkoutCounter != null)
        {
            currentState = NPCState.GoingToCheckout;
            agent.SetDestination(checkoutCounter.position);
        }
        else
        {
            FindNextItem();
        }
    }

    private IEnumerator HandleCheckout()// mang đồ dã được nhặt ra thanh toán
    {
        currentState = NPCState.PickingUpItem;
        yield return new WaitForSeconds(3f);

        if (exitDoor != null)
        {
            currentState = NPCState.Leaving;
            agent.SetDestination(exitDoor.position);
        }
    }
}