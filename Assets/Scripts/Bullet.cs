using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime);
    }


    void OnTriggerEnter(Collider col){
        Player p = col.GetComponent<Player>();
        if(p){
            p.GameOver();
        }
    }
}
