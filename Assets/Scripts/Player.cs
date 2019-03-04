using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player instance;
    public float speed = 2;
    public Camera camera;
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
    float lastHit;
    public Image[] hearts;
    bool[] gemsCollected = new bool[4];
    public Color[] gemColors;
    public Image[] gemImages;
    bool viewShifting = false;
    public GameObject gemChime, ouch;
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
        if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit, 50, 1 << 13))
        {
            transform.LookAt(new Vector3(hit.point.x, 0, hit.point.z));
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
        if (weapon)
        {
            weapon.MouseButton(Input.GetMouseButton(0), Input.GetMouseButton(1));
        }
        if (weaponInteractTime + 0.1f < Time.time)
        {
            toEquip = null;
        }
    }
    void Update()
    {
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
    }

    public void GameOver()
    {
        Manager.instance.GameOver();
        this.enabled = false;
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
    void OnTriggerEnter(Collider col)
    {
        switch (col.name)
        {
            case "UpTrigger":
                StopAllCoroutines();
                StartCoroutine(ShiftView(Vector2Int.up * RoomGraph.instance.spacingZ));
                break;
            case "DownTrigger":
                StopAllCoroutines();
                StartCoroutine(ShiftView(Vector2Int.down * RoomGraph.instance.spacingZ));
                break;
            case "RightTrigger":
                StopAllCoroutines();
                StartCoroutine(ShiftView(Vector2Int.right * RoomGraph.instance.spacingX));
                break;
            case "LeftTrigger":
                StopAllCoroutines();
                StartCoroutine(ShiftView(Vector2Int.left * RoomGraph.instance.spacingX));
                break;
            case "Gem":
                Collect(col.GetComponent<Collectible>().gemNumber);
                Destroy(col.gameObject);
                break;
            default:
                break;
        }
    }

    IEnumerator ShiftView(Vector2Int dir)
    {
        Manager.instance.currentRoom.gameObject.SetActive(false);
        Manager.instance.currentRoom = RoomGraph.instance.rooms[Manager.instance.currentRoom.pos + dir];
        Manager.instance.currentRoom.gameObject.SetActive(true);
        yield return null;
        Manager.instance.currentRoom.Set();
        Vector3 pos = Manager.instance.currentRoom.transform.position
        + new Vector3(
            0,
            15.5f,
            -13.83f);
        while (Vector3.Distance(camera.transform.position, pos) > 0.1f)
        {
            camera.transform.position += ((pos - camera.transform.position) * Time.deltaTime * 5f);
            yield return null;
        }
        yield return null;
    }

    void Collect(int i)
    {
        gemChime.SetActive(false);
        gemChime.SetActive(true);
        gemsCollected[i] = true;
        gemImages[i].color = gemColors[i];
        bool complete = true;
        foreach (bool b in gemsCollected)
        {
            complete = complete && b;
        }
        if (complete)
        {
            Manager.instance.Victory();
        }
    }

    public void Hit()
    {
        if (!dash && lastHit + 1 < Time.time)
        {
            ouch.SetActive(false);
            ouch.SetActive(true);
            lastHit = Time.time;
            health--;
            for (int i = Mathf.Max(0, Mathf.RoundToInt(health)); i < hearts.Length; i++)
            {
                hearts[i].color = Color.black;
            }
            if (health <= 0)
            {
                GameOver();
            }
        }
    }
}
