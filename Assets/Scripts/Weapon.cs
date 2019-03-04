using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float cooldownLeft = 0.5f, cooldownRight = 2f;
    float lastLeft, lastRight;
    public GameObject[] leftAttacks, rightAttacks;
    int leftIndex, rightIndex;
    public Collider area;
    public GameObject highlight;
    public virtual void MouseButton(bool left, bool right)
    {
        if (left && lastLeft + cooldownLeft < Time.time && lastRight + cooldownLeft < Time.time && leftAttacks.Length > 0)
        {
            LeftAttack();
            lastLeft = Time.time;
        }
        if (right && lastRight + cooldownRight < Time.time && lastLeft + cooldownLeft < Time.time && rightAttacks.Length > 0)
        {
            RightAttack();
            lastRight = Time.time;
        }
    }

    public virtual void LeftAttack()
    {
        if (lastLeft + cooldownLeft * 2 < Time.time)
        {
            leftIndex = 0;
        }
        Instantiate(leftAttacks[leftIndex], transform.position + transform.forward, transform.rotation, transform);
        leftIndex++;
        if (leftIndex >= leftAttacks.Length)
        {
            leftIndex = 0;
        }
    }
    public virtual void RightAttack()
    {
        if (lastRight + cooldownRight * 2 < Time.time)
        {
            rightIndex = 0;
        }
        Instantiate(rightAttacks[rightIndex], transform.position + transform.forward, transform.rotation, transform);
        rightIndex++;
        if (rightIndex >= rightAttacks.Length)
        {
            rightIndex = 0;
        }
    }
    public void Equip()
    {
        transform.SetParent(Player.instance.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        area.enabled = false;
        highlight.SetActive(false);
    }
    public void Unequip()
    {
        transform.SetParent(null);
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        area.enabled = true;
        highlight.SetActive(true);
    }
}
