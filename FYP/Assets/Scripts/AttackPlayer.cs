using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    GameObject player;
    [SerializeField]
    GameObject parent;
    float attackDamage = 25;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Constants.Player);
        parent = this.transform.parent.parent.gameObject;
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if ((mask.value & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
    //    {
    //        collision.GetComponent<Rigidbody2D>().isKinematic = false;
    //        Vector2 difference = collision.transform.position - parent.transform.position;
    //        //difference = difference.normalized * 100;
    //        //collision.GetComponent<Rigidbody2D>().AddForce(difference, ForceMode2D.Impulse); 
    //        collision.transform.position = new Vector2(collision.transform.position.x + difference.x, collision.transform.position.y + difference.y);
    //        attackingThePlayer(20);
    //        collision.GetComponent<Rigidbody2D>().isKinematic = true;
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if ((mask.value & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
        if (collision.transform.tag == Constants.Player)
        {
            //collision.transform.GetComponent<Rigidbody2D>().isKinematic = false;
            //Vector2 difference = collision.transform.position - parent.transform.position;

            //difference = difference.normalized * 100;
            //collision.GetComponent<Rigidbody2D>().AddForce(difference, ForceMode2D.Impulse); 

            //collision.transform.position = new Vector2(collision.transform.position.x + difference.x, collision.transform.position.y + difference.y);
            attackingThePlayer(attackDamage);
            //collision.transform.GetComponent<Rigidbody2D>().isKinematic = true;

            ScoreManager.instance.subtractPoint(15f);
        }
    }
    
    void attackingThePlayer(float healthDeducted)
    {
        player.GetComponent<PlayerHealth>().takeDamage(healthDeducted);
    }
}
