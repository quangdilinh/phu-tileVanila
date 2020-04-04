using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class player : MonoBehaviour
{
    [Header("Component")]
    Rigidbody2D rb;
    Transform tf;
    public master master;
    public Animator animator;
    [Header("Moverment")]
    [SerializeField] private Transform feet;
    [SerializeField] private Transform head;
    Vector2 v;
    Vector2 temp_v;
    Vector2 temp_v1;
    [Header("State")]
    float air_resis = 1;
    bool onground = true;
    bool onlader;
    bool onceiling;
    [SerializeField] private bool occupied;
    private Vector3 m_Velocity = Vector3.zero;
    [Range(0, .7f)] [SerializeField] private float m_MovementSmoothing = .05f;
    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask lader;

    [Header("Variables")]
    
    [SerializeField] private float health = 3;
    [SerializeField] private float i_frame;
    [SerializeField] private float total_i_frame;
    [SerializeField] private bool i_timer;
    [SerializeField] private float accelaration = 0.5f;
    [SerializeField] private bool air_controll;
    [SerializeField] private float radius;
    [SerializeField] private float grav = 1f;
    [SerializeField] private float grav1 = 1f;
    [SerializeField] private float jump_speed = 10;
    [SerializeField] private float climb_speed = 10;
    [SerializeField] private float run_speed = 5;
    [SerializeField] private float timer_frame;
    [SerializeField] private float total_timer_frame;
    [SerializeField] private bool timer;
    [SerializeField] private float dirrection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tf = GetComponent<Transform>();
        timer_frame = 0;
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
        communicate();
    }
    void check()
    {
        onground = false;
        onlader = false;
        onceiling = false;
        Collider2D[] hit;
        hit = Physics2D.OverlapCircleAll(feet.position, 0.2f, ground);
        onground = hit.Length >= 1;
        hit = Physics2D.OverlapCircleAll(feet.position, radius, lader);
        onlader = hit.Length >= 1;
        hit = Physics2D.OverlapCircleAll(head.position, radius, ground);
        onceiling = hit.Length >= 1;
        if (i_timer)
        {
            i_frame++;
        }
        if (i_frame == total_i_frame)
        {
            i_timer = false;
            i_frame = 0;
        }
    }
    void run()
    {
        if (onground || air_controll)
        {
            //v.x += (100 /( Mathf.Abs(v.x)+100))*Input.GetAxisRaw("Horizontal");
            temp_v.x = run_speed * Input.GetAxisRaw("Horizontal") ;
        }
    }

    void jump()
    {
        float controlerY = Input.GetAxisRaw("Vertical");
        if (onground && controlerY == 1)
        {
            v.y = jump_speed * controlerY;
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
                if (v.y>-5)
                {
                    v.y -= grav;
                }
            }
        }

    }
    void climb()
    {
        if (onlader)
        {
            grav = 0;
            temp_v.y = climb_speed * Input.GetAxisRaw("Vertical");
        }
        else
        {
            grav=grav1;
        }
    }
    void turn()
    {
        if (temp_v.x * dirrection < 0)
        {
            timer = true;
            dirrection *= -1;
        }
        if (timer)
        {
            timer_frame++;
        }
        if (timer_frame == total_timer_frame)
        {
            timer = false;
            timer_frame = 0;
            transform.Rotate(0f, 180f, 0f);
        }
    }
    void move()
    {
        //rb.velocity = Vector3.SmoothDamp(rb.velocity, v, ref m_Velocity, m_MovementSmoothing);

        Debug.Log("x");
        Debug.Log(v.x);
        Debug.Log("y");
        Debug.Log(v.y);
        if (v.x < v.x*accelaration)
        {
            v.x += v.x * accelaration;
        }
        else
        {
            if (v.x > v.x * accelaration)
            {
                v.x -= v.x * accelaration;
            }
            else
            {
                v.x = 0;
            }
        }
        if (v.y < v.y * accelaration)
        {
            v.y += v.y * accelaration;
        }
        else
        {
            if (v.y > v.y * accelaration)
            {
                v.y -= v.y * accelaration;
            }
            else
            {
                v.y = 0;
            }
        }
        //temp_v1.x = v.x + temp_v.x;
        //temp_v1.y = v.y + temp_v.y;

        rb.velocity = temp_v1;
    }
    void animate()
    {
        animator.SetBool("is_turning", timer_frame > 0);
        animator.SetInteger("speed", (int)v.x);
        animator.SetBool("is_jumping" , v.y != 0);
        animator.SetBool("is_climing", onlader);
    }
    void communicate()
    {
        master.health = health;
    }
    void OnCollisionEnter2D(Collision2D hit)
    {

        if (hit.gameObject.layer == 10 && hit.gameObject.transform.position.y+0.1 >= tf.position.y && !i_timer)
        {
            health--;
            i_timer = true;

        }
        if (hit.gameObject.layer == 10)
        {
            v.y += (tf.position.y - hit.gameObject.transform.position.y) * 70;
            v.x += (tf.position.x - hit.gameObject.transform.position.x) * 70;
        }
        if (health==0)
        {
            //Destroy(gameObject);
        }
    }
}
