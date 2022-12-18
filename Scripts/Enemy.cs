using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{   
    public string enemyName;
    public int enemyScore;
    public float speed;
    public int health;
    public Sprite[] sprites;

    SpriteRenderer spriteRenderer;

    public float maxShotDelay;      // 최대
    public float curShotDelay;      // 현재

    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public GameObject player;


    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        Fire();
        Reload();
    }


    void Fire() {       // 총알 발사
        if (curShotDelay < maxShotDelay)
            return;

        if (enemyName == "S") {
            GameObject bullet = Instantiate(bulletObjA, transform.position, transform.rotation);        // 오브젝트 새로 생성
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector3 dirVec = player.transform.position - transform.position;     // 플레이어를 바라보는 각도
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);        // .normalized 로 크기는 1로 만든다.
        }
        else if (enemyName == "L") {
            GameObject bulletR = Instantiate(bulletObjB, transform.position + Vector3.right * 0.3f, transform.rotation);        // 오브젝트 새로 생성
            GameObject bulletL = Instantiate(bulletObjB, transform.position + Vector3.left * 0.3f, transform.rotation);        // 오브젝트 새로 생성
            Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();

            Vector3 dirVecR = player.transform.position - (transform.position + Vector3.right * 0.3f);     // 플레이어를 바라보는 각도
            Vector3 dirVecL = player.transform.position - (transform.position + Vector3.left * 0.3f);     // 플레이어를 바라보는 각도
            rigidR.AddForce(dirVecR.normalized * 4, ForceMode2D.Impulse);
            rigidL.AddForce(dirVecL.normalized * 4, ForceMode2D.Impulse);
        }

        curShotDelay = 0;
        
    }


    void Reload() {
        curShotDelay += Time.deltaTime;
    }


    void OnHit(int dmg) {
        health -= dmg;
        spriteRenderer.sprite = sprites[1];     // 피격당했을 때의 스프라이트
        Invoke("ReturnSprite", 0.1f);       // 0.1 초 후에 원래 스프라이트로 되돌아옴

        if (health <= 0) {         // 피가 0 이하가 되면 파괴
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += enemyScore;
            Destroy(gameObject);
        }
    }


    void ReturnSprite() {
        spriteRenderer.sprite = sprites[0];      // 피격당하고 난 후 다시 원래 스프라이트로 되돌아옴
    }


    void OnTriggerEnter2D(Collider2D collision){        // 적 총알이 바깥으로 나가면 총알 삭제
        if(collision.gameObject.tag == "BorderBullet")
            Destroy(gameObject);
        else if (collision.gameObject.tag == "PlayerBullet") {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.dmg);
            Destroy(collision.gameObject);      // PlayerBullet 태그가 있는 피격된 총알도 같이 제거
        }
    }    
}
