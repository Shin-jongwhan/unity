using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemyObjs;
    public Transform[] spawnPoints;

    public float maxSpawnDelay;
    public float curSpawnDelay;

    public GameObject player;
    //public Text scoreText;
    public TextMeshProUGUI scoreText;
    public Image[] lifeImage;
    public GameObject gameOverSet;


    void Update() {
        curSpawnDelay += Time.deltaTime;
        
        if(curSpawnDelay > maxSpawnDelay) {     // 시간이 지나면 소환
            SpawnEnemy();
            maxSpawnDelay = Random.Range(0.5f, 3f);
            curSpawnDelay = 0;
        }

        // UI score update
        Player playerLogic = player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score);
    }

    
    void SpawnEnemy() {
        int ranEnemy = Random.Range(0, 3);
        int ranPoint = Random.Range(0, 9);      // 스폰 포인트 5개
        GameObject enemy = Instantiate(enemyObjs[ranEnemy], 
            spawnPoints[ranPoint].position,
            spawnPoints[ranPoint].rotation
        );
        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player;     // 프리팹은 씬에 생성된 오브젝트는 인식하지 못 한다. 그래서 씬에 항상 남아 있는 GameManager 에 player 를 public 으로 등록하고, enemy 가 생성되었을 때 player 를 넘겨준다.

        if (ranPoint == 5 || ranPoint == 6) {     // 오른쪽 소환
            enemy.transform.Rotate(Vector3.back * (Mathf.Atan2(enemyLogic.speed * (-1), -1) * Mathf.Rad2Deg + 180));      // vector3.back 은 z 축을 -1 로 회전시킨다.
            rigid.velocity = new Vector2(enemyLogic.speed * (-1), -1);
        }
        else if (ranPoint == 7 || ranPoint == 8) {     // 오른쪽 소환
        enemy.transform.Rotate(Vector3.back * (Mathf.Atan2(enemyLogic.speed, -1) * Mathf.Rad2Deg + 180));       // vector3.forward 는 z 축을 -1 로 회전시킨다.
            rigid.velocity = new Vector2(enemyLogic.speed, -1);
        }
        else {
            rigid.velocity = new Vector2(0, enemyLogic.speed * (-1));
        }
    }


    public void UpdateLifeIcon(int life) {
        // init. 모두 끔
        for (int index = 0; index < 3; index++) {
            lifeImage[index].color = new Color(1, 1, 1, 0);
        }

        // 남아 있는 목숨만큼 켬
        for (int index = 0; index < life; index++) {
            lifeImage[index].color = new Color(1, 1, 1, 1);
        }
    }


    public void RespawnPlayer() {
        Invoke("RespawnPlayerExe", 2f);       // 재생성 시간차를 두기 위함
    }


    void RespawnPlayerExe() {
        player.transform.position = Vector3.down * 4.0f;
        player.SetActive(true);

        Player playerLogic = player.GetComponent<Player>();
        playerLogic.isHit = false;
    }

    
    public void GameOver() {
        gameOverSet.SetActive(true);
    }


    public void GameRetry() {
        SceneManager.LoadScene(0);      // 0 은 build settings 에서 확인 가능한 숫자
    }
    
}
