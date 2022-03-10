using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    public float shotSpeed;
    public Rigidbody2D shotRB;
    public float knockback;

    // Start is called before the first frame update
    void Start()
    {
        //shotRB.velocity = transform.right * shotSpeed;
        shotRB.AddRelativeForce(transform.right * shotSpeed);
    }

    //When shot collides with another collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);
        Destroy(gameObject);

        if(collision.gameObject.CompareTag("Player"))
        {
            collision.attachedRigidbody.AddForce((transform.right + Vector3.up) * knockback, ForceMode2D.Impulse);
        }
    }
}
