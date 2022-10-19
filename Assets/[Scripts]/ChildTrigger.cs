using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildTrigger : MonoBehaviour
{
    public EnemyAI enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<EnemyAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && enemy != null)
        {
            enemy.playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && enemy != null)
        {
            enemy.playerInTrigger = false;
        }
    }
}
