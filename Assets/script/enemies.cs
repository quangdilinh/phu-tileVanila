using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemies : MonoBehaviour
{
    Rigidbody2D rb;
    Transform tf;
    [SerializeField] private Transform feet;
    [SerializeField] private Transform head;
    [SerializeField] private Transform face;
    Vector2 v;
    float air_resis = 1;
    bool onground = true;
    private Vector3 m_Velocity = Vector3.zero;
    [Range(0, .7f)] [SerializeField] private float m_MovementSmoothing = .05f;
    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask lader;
    [SerializeField] private LayerMask player;
    [SerializeField] private bool air_controll;
    [SerializeField] private float radius;
    bool onlader;
    bool onceiling;
    [SerializeField] private float grav;
    [SerializeField] private float grav1;
    [SerializeField] private float jump_speed;
    [SerializeField] private float climb_speed;
    [SerializeField] private float run_speed;
    [SerializeField] private float timer_tick;
    [SerializeField] private float total_timer_tick;
    [SerializeField] private bool timer;
    [SerializeField] private float dirrection;
    [SerializeField] private float run1;
    [SerializeField] private float jump1;
    [SerializeField] private float turn1;
    [SerializeField] private float clime1;
    public Animator animator;
    [SerializeField] private bool occupied;
    [SerializeField] private CapsuleCollider2D enemies_colider;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tf = GetComponent<Transform>();
        timer_tick = 0;
        v.x = 0;
        v.y = 0;
        dirrection = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        check();
        run();
        jump();
        climb();
        turn();
        move();
        animate();
    }
    void check()
    {
        onground = false;
        onlader = false;
        onceiling = false;
        Collider2D[] hit;
        hit = Physics2D.OverlapCircleAll(feet.position, 1 / 100, ground);
        onground = hit.Length >= 1;
        hit = Physics2D.OverlapCircleAll(feet.position, radius, lader);
        onlader = hit.Length >= 1;
        hit = Physics2D.OverlapCircleAll(head.position, radius, ground);
        onceiling = hit.Length >= 1;
        hit = Physics2D.OverlapCircleAll(face.position, radius, ground);
        if (hit.Length >= 1)
        {
            turn1 *= -1;
        }
    }
    void run()
    {
        v.x = 0;
        if (onground || air_controll)
        {
            //v.x += (100 /( Mathf.Abs(v.x)+100))*Input.GetAxisRaw("Horizontal");
            if (turn1!=0)
            {
                v.x = run_speed * run1 * turn1;
            }
            else
            {
                v.x = run_speed * run1;
            }
        }
    }

    void jump()
    {
        float controlerY = Input.GetAxisRaw("Vertical");
        if (onground && jump1 == 1)
        {
            v.y = jump_speed * jump1;
            onground = false;
        }
        else
        {
            if (onceiling || onground)
            {
                v.y = 0;
            }
            else
            {
                v.y -= grav;
            }
        }

    }
    void climb()
    {
        if (onlader)
        {
            grav = 0;
            v.y = climb_speed * clime1;
        }
        else
        {
            grav = grav1;
        }
    }
    void turn()
    {
        if (v.x * dirrection < 0)
        {
            timer = true;
            dirrection *= -1;
        }
        if (timer)
        {
            timer_tick++;
        }
        if (timer_tick == total_timer_tick)
        {
            timer = false;
            timer_tick = 0;
            transform.Rotate(0f, 180f, 0f);
        }
    }
    void move()
    {
        //rb.velocity = Vector3.SmoothDamp(rb.velocity, v, ref m_Velocity, m_MovementSmoothing);
        rb.velocity = v;
    }
    void animate()
    {
        animator.SetBool("is_turning", timer_tick > 0);
        animator.SetInteger("speed", (int)v.x);
        animator.SetBool("is_jumping", v.y != 0);
        animator.SetBool("is_climing", onlader);
    }
    void OnCollisionEnter2D(Collision2D hit)
    {
        if (hit.gameObject.layer == 8 && hit.gameObject.transform.position.y > tf.position.y+0.1)
        {
            Destroy(gameObject);
        }
    }
}

