using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    public Vector3 point;
    public GameObject throwPoint;
    PlayerHealth playerHealth;

    Vector2 movement;

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //if a movement key is being pressed ...
        if (movement != Vector2.zero)
        {
            //update horizontal/vertical accordingly
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            Sound.PlaySound(Sound.SoundType.mcWalking);
        }   //else Horizontal and Vertical will not update each frame, leaving it at last recorded value 

        animator.SetFloat("Speed", movement.sqrMagnitude);
        
        //Change rotation/orientation of throwpoint based off movement direction
        if (movement.x == 0 && movement.y == -1)        //down
        {
            throwPoint.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, 180);
        }
        else if (movement.x == 0 && movement.y == 1)    //up
        {
            throwPoint.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, 0);

        }
        else if (movement.x == -1 && movement.y == 0)   //left
        {
            throwPoint.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, 90);

        }
        else if (movement.x == 1 && movement.y == 0)    //right
        {
            throwPoint.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, 270);
        }
        /*
        if (Input.GetKeyUp(KeyCode.P))
        {
            this.GetComponent<PlayerHealth>().takeDamage(20);
        }*/
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift) && PlayerStaminaBar.instance.getCurrentStamina() > 0)
        {
            if (movement.magnitude > Mathf.Epsilon)
            {
                PlayerStaminaBar.instance.UseStamina(30f);
                rb.MovePosition(rb.position + movement.normalized * (moveSpeed * 2) * Time.fixedDeltaTime);
            }
            else
            {
                //nothing should happen here
            }
        }
        else
            rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);

        playerHealth = GetComponent<PlayerHealth>();

        if (playerHealth.getPlayerHP() <= 0)
        {
            animator.SetBool("isDead", true);
            moveSpeed = 0f;
        }
        else
            animator.SetBool("isDead", false);
    }

}
