using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float lifetime = 0.05f;
    public float damage = 1;
    public GameObject parent;
    void Start()
    {
        Destroy(parent != null ? parent : gameObject, lifetime);
    }
    void OnTriggerEnter(Collider col)
    {
        Enemy e = col.GetComponent<Enemy>();
        if(e){
            e.Hit(damage);
        }
    }
}
