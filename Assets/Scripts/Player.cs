using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 2;
    public Camera camera;
    public GameObject Bullet;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized * speed * Time.deltaTime;
        RaycastHit hit;
        if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit, 50))
        {
            transform.LookAt(new Vector3(hit.point.x, 0.5f, hit.point.z));
        }
        if (Input.GetMouseButtonDown(0))
        {
            Click();
        }
    }

    void Click()
    {
        Instantiate(Bullet, transform.position + transform.forward, transform.rotation);
    }

    public void GameOver()
    {
        transform.position = Vector3.up * 0.5f;
    }
}
