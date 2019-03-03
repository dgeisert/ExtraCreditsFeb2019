using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Attack
{
    public float speed = 5;

    void Start()
    {
        transform.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }


    void OnCollisionEnter(Collision col)
    {
        Enemy e = col.collider.GetComponent<Enemy>();
        if (e)
        {
            e.Hit(damage);
        }
        Player p = col.collider.GetComponent<Player>();
        if (p)
        {
            p.Hit();
        }
        Destroy(gameObject);
    }
}
