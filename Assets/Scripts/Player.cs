using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public float speed = 2;
    public Camera camera;
    public GameObject Bullet;
    public CharacterController cc;
    public Animator anim;
    float lastDash = -3;
    float dashCooldown = 2, dashDuration = 0.1f, dashSpeed = 10;
    public GameObject dashPoof, dashLine;
    public Weapon weapon;
    Weapon toEquip;
    float weaponInteractTime;
    public float health = 3;
    public float maxHealth = 3;
    bool dash = false;
    void Start()
    {
        instance = this;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        dash = (lastDash + dashDuration > Time.time);
        dashLine.SetActive(lastDash + dashDuration * 5 > Time.time);
        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized
        * speed * Time.fixedDeltaTime * (dash ? dashSpeed : 1);
        cc.SimpleMove(move);
        anim.SetFloat("Speed", move.magnitude);
        RaycastHit hit;
        if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit, 50))
        {
            transform.LookAt(new Vector3(hit.point.x, 0.5f, hit.point.z));
        }
        if (weapon)
        {
            weapon.MouseButton(Input.GetMouseButton(0), Input.GetMouseButton(1));
        }
        if (Input.GetKeyDown(KeyCode.Space) && lastDash + dashCooldown < Time.time)
        {
            lastDash = Time.time;
            dashPoof.SetActive(false);
            dashPoof.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
        if (weaponInteractTime + 0.1f < Time.time)
        {
            toEquip = null;
        }
    }

    public void GameOver()
    {
        transform.position = Vector3.up * 0.5f;
    }

    void Interact()
    {
        if (toEquip)
        {
            Weapon toUnequip = weapon;
            toEquip.Equip();
            weapon = toEquip;
            toEquip = null;
            if (toUnequip != null)
            {
                toUnequip.Unequip();
            }
        }
    }
    void OnTriggerStay(Collider col)
    {
        Weapon w = col.GetComponent<Weapon>();
        if (w != null)
        {
            toEquip = w;
            weaponInteractTime = Time.time;
        }
    }

    public void Hit()
    {
        if (!dash)
        {
            health--;
            if (health <= 0)
            {
                GameOver();
            }
        }
    }
}
