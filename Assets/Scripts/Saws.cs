using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saws : MonoBehaviour
{
    public Transform from, to;
    public float speed = 1;
    Transform target;

    void Start()
    {
        target = to;
    }
    void Update()
    {
        transform.eulerAngles += Vector3.up * 3;
        transform.position += (target.position - transform.position).normalized * Time.deltaTime * speed;
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            if (target == to)
            {
                target = from;
            }
            else
            {
                target = to;
            }
        }
    }

    void OnTriggerEnter(Collider col){
        Player p = col.GetComponent<Player>();
        if(p){
            p.GameOver();
        }
    }
}
