using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayerScript : MonoBehaviour
{
    public CharacterController controller;
    public float speed =  6f;
    public float bulletSpeed = 100f;

    public GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            controller.Move(direction * speed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ShootBullet(gameObject.transform.forward);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ShootBullet(-gameObject.transform.right);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ShootBullet(gameObject.transform.right);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ShootBullet(-gameObject.transform.forward);
        }
    }

    void ShootBullet(Vector3 dir)
    {
        GameObject instBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        Rigidbody instFoamRB = instBullet.GetComponent<Rigidbody>();

        instFoamRB.AddForce(dir * bulletSpeed);
        Destroy(instBullet, 3f);
    }
}
