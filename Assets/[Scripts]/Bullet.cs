using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject impactEffect;
    public LayerMask enemiesMask;

    [Header("Bullet Stats")]
    [Range(0f,1f)]
    public float bounciness;
    public bool useGravity;
    public float damage;
    public float range;
    public int maxCollisions;
    public float maxLifetime;
    public bool destroyOnImpact = true;

    private int collisions;
    private PhysicMaterial material;

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        if(collisions > maxCollisions) 
            Impact();

        maxLifetime -= Time.deltaTime;

        if (maxLifetime <= 0)
            Impact();
    }

    private void OnCollisionEnter(Collision collision)
    {
        collisions++;

        if (collision.collider.CompareTag("Enemy") && destroyOnImpact) 
            Impact();
    }
    private void Impact()
    {
        if (impactEffect != null)
            Instantiate(impactEffect, transform.position, Quaternion.identity);

        Collider[] enemies = Physics.OverlapSphere(transform.position, range, enemiesMask);

        foreach (Collider enemy in enemies)
        {
            //enemy.GetComponent<EnemyAI>().TakeDamage(damage);
        }
    }
    private void Setup()
    {
        material = new PhysicMaterial();
        material.bounciness = bounciness;
        material.frictionCombine = PhysicMaterialCombine.Minimum;
        material.bounceCombine = PhysicMaterialCombine.Maximum;
        GetComponent<SphereCollider>().material = material;

        rb.useGravity = useGravity;
    }

}
