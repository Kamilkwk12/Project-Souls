using UnityEngine;
using UnityEngine.SceneManagement;
public class GameState : MonoBehaviour
{
    BossStatus _bossStatus;

    float timer = 3;
    void Start()
    {
        _bossStatus = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossStatus>();
    }

    void Update()
    {
        if (_bossStatus.isDead) 
        {
            timer -= Time.deltaTime; 
        }
        if (timer <= 0)
        {
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }
    }
}
