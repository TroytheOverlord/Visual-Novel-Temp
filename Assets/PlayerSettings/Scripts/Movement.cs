using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 10;
    public float jumpSp = 5; 
    private bool grounded = false;
    private Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float xinput = Input.GetAxis("Horizontal");

        float yvel = body.velocity.y;
        body.velocity = new Vector2(xinput * speed,yvel);


        if(Input.GetKey("space") && grounded){
            Debug.Log("space pressed");
            body.velocity = new Vector2(body.velocity.x, jumpSp);
            grounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag == "Ground"){
            grounded = true;
        }
    }
}