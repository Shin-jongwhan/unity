using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour

{
    public float speed;
    public float power;
    public float maxShotDelay;      // 최대
    public float curShotDelay;      // 현재

    public bool isTouchTop;
    public bool isTouchBottom;
    public bool isTouchRight;
    public bool isTouchLeft;

    public GameObject bulletObjA;
    public GameObject bulletObjB;

    Animator anim;
    

    void Awake() {
        anim = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
        Reload();
    }


    void Move(){
        float h = Input.GetAxisRaw("Horizontal");
        if ((isTouchRight && h == 1) || (isTouchLeft && h == -1))
            h = 0;

        float v = Input.GetAxisRaw("Vertical");
        if ((isTouchTop && v == 1) || (isTouchBottom && v == -1))
            v = 0;
        Vector3 curPos = transform.position;        // 현재 위치 가져옴
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;

        if (Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal")) {
            anim.SetInteger("Input", (int) h);
        }
    }


    void Fire() {       // 총알 발사
        if (!Input.GetButton("Fire1"))      // 버튼 누르지 않았으면 리턴
            return;
        
        if (curShotDelay < maxShotDelay)
            return;

        switch (power) {
            case 1 : 
                // power 1
                //transform.position / retation : 부모 오브젝트 현재 위치, 회전
                GameObject bullet = Instantiate(bulletObjA, transform.position, transform.rotation);        // 오브젝트 새로 생성
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 2 : 
                GameObject bulletR = Instantiate(bulletObjA, transform.position + Vector3.right * 0.1f, transform.rotation);        // 오브젝트 새로 생성
                GameObject bulletL = Instantiate(bulletObjA, transform.position + Vector3.left * 0.1f, transform.rotation);        // 오브젝트 새로 생성
                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                rigidR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 3 : 
                GameObject bulletRR = Instantiate(bulletObjA, transform.position + Vector3.right * 0.35f, transform.rotation);        // 오브젝트 새로 생성
                GameObject bulletCC = Instantiate(bulletObjB, transform.position, transform.rotation);        // 중앙 오브젝트 새로 생성
                GameObject bulletLL = Instantiate(bulletObjA, transform.position + Vector3.left * 0.35f, transform.rotation);        // 오브젝트 새로 생성
                Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
                rigidCC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidRR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidLL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
        }

        curShotDelay = 0;
        
    }


    void Reload() {
        curShotDelay += Time.deltaTime;
    }


    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Border"){      // 벽 부딛혔을 때 인식
            switch (collision.gameObject.name) {
                case "Top" : 
                    isTouchTop = true;
                    break;
                case "Bottom" : 
                    isTouchBottom = true;
                    break;
                case "Right" : 
                    isTouchRight = true;
                    break;
                case "Left" : 
                    isTouchLeft = true;
                    break;
            }
        }
    }


    void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Border"){      // 벽 부딛힌 게 떼졌을 때 인식
            switch (collision.gameObject.name) {
                case "Top" : 
                    isTouchTop = false;
                    break;
                case "Bottom" : 
                    isTouchBottom = false;
                    break;
                case "Right" : 
                    isTouchRight = false;
                    break;
                case "Left" : 
                    isTouchLeft = false;
                    break;
            }
        }
    }
}
