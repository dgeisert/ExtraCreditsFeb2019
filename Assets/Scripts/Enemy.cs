using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public CharacterController cc;
    public GameObject attack;
    public GameObject telegraph;
    public Animator anim;
    public float health = 1;
    public Room room;
    public float speed = 100;
    public float speedDuringAttack = 0;
    public float speedMod = 1;
    public float attackDistance = 1;
    public float telegraphTime = 1;
    public float attackTime = 0.5f;
    public float attackCooldown = 3;
    public int season, dayNight;
    public bool strict = false;
    int state = 0;
    float timer = 0;
    public GameObject projectile;
    public float[] angles;
    /*
    0 = move
    1 = telegraph look
    2 = telegraph no look
    3 = attack
    4 = wait after attack
     */
    public void Hit(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
    void FixedUpdate()
    {
        switch (state)
        {
            case 0:
                transform.LookAt(Player.instance.transform);
                cc.SimpleMove(transform.forward * Time.fixedDeltaTime * speed * speedMod);
                if (Vector3.Distance(transform.position, Player.instance.transform.position) < attackDistance)
                {
                    telegraph.SetActive(true);
                    timer = Time.time;
                    state++;
                    anim.SetInteger("State", state);
                }
                break;
            case 1:
                if (timer + telegraphTime / 2 < Time.time)
                {
                    state++;
                    anim.SetInteger("State", state);
                }
                transform.LookAt(Player.instance.transform);
                break;
            case 2:
                if (timer + telegraphTime < Time.time)
                {
                    foreach (float a in angles)
                    {
                        Instantiate(projectile, transform.position + Vector3.up * 1f + transform.forward * 0.5f, transform.rotation)
                        .transform.RotateAround(transform.position, Vector3.up, a);
                    }
                    telegraph.SetActive(false);
                    attack.SetActive(true);
                    timer = Time.time;
                    state++;
                    anim.SetInteger("State", state);
                }
                break;
            case 3:
                cc.SimpleMove(transform.forward * Time.fixedDeltaTime * speedDuringAttack);
                if (timer + attackTime < Time.time)
                {
                    attack.SetActive(false);
                    timer = Time.time;
                    state++;
                    anim.SetInteger("State", state);
                }
                break;
            case 4:
                if (timer + attackCooldown < Time.time)
                {
                    timer = Time.time;
                    state = 0;
                    anim.SetInteger("State", state);
                }
                break;
            default:
                break;
        }
    }
    void Die()
    {
        if (room)
        {
            room.enemies.Remove(this);
        }
        Destroy(gameObject);
    }
    IEnumerator Dormant()
    {
        cc.enabled = false;
        while (transform.position.y > -2)
        {
            transform.position -= Vector3.up * Time.deltaTime * 4;
            yield return null;
        }
        gameObject.SetActive(false);
    }
    IEnumerator Emerge()
    {
        cc.enabled = false;
        transform.position = new Vector3(transform.position.x, -2, transform.position.z);
        while (transform.position.y < 0)
        {
            transform.position += Vector3.up * Time.deltaTime * 4;
            yield return null;
        }
        cc.enabled = true;
    }
    void Active(bool isSeason, bool isDayNight)
    {
        if (((season - 2 == Manager.season || season + 2 == Manager.season))
        || ((!isSeason || !isDayNight) && strict))
        {
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine("Dormant");
            }
            return;
        }
        gameObject.SetActive(true);
        speedMod = 1 - (isSeason ? 0 : 0.25f) - (isDayNight ? 0 : 0.25f);
    }
    public void Set()
    {
        Active(Manager.season == season || season == -1,
        Manager.dayNight == dayNight || dayNight == -1);
    }

    void OnEnable()
    {
        attack.SetActive(false);
        telegraph.SetActive(false);
        state = 4;
        timer = Time.time;
        anim.SetInteger("State", state);
        StartCoroutine("Emerge");
    }
}
