using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPun
{
    [Header("Info")]
    public int id;
    private int curAtackerID;
    [Header("Stats")]
    public float moveSpeed;
    public float jumpForce;
    private int curHP;
    private int maxHP;
    public int kills;
    public bool dead;
    private bool flashKillColor;
    [Header("Components")]
    public Rigidbody rig;
    public Player photonPlayer;
    public MeshRenderer mr;
    
    [PunRPC]
    public void Initialize(Player player)
    {
        id = player.ActorNumber;
        photonPlayer = player;
        GameManager.instance.player[id - 1] = this;
       // is not your view 
        if(!photonView.IsMine)
        {
            GetComponentInChildren<Camera>().gameObject.SetActive(false);
            rig.isKinematic = true;

        }
    }
    private void Update()
    {
        if (!photonView.IsMine || dead)
            return;
        Move();
        if (Input.GetKeyDown(KeyCode.Space))
            TryJump();
    }
    void Move()
    {
        // get the input axis
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        // calc a dir relative to where facing
        Vector3 dir = (transform.forward * z + transform.right * x) * moveSpeed;
        Debug.Log(dir.ToString());
        dir.y = rig.velocity.y;
        // ste velocity
        rig.velocity = dir;
    }
    void TryJump()
    {
        // create a ray facing down
        Ray ray = new Ray(transform.position, Vector3.down);
        //shoot the raycats
        if (Physics.Raycast(ray, 1.5f))
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    [PunRPC]
    public void TakeDemage(int attacekrID, int demage)
    {
        if (dead)
            return;
        curHP -= demage;
        curAtackerID = attacekrID;

        if(curHP<=0)
        {

        }
    }
    [PunRPC]
    void Die()
    {

    }
}
