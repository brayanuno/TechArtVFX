using UnityEngine;

public class GameOverDisplay : MonoBehaviour
{

    GameObject gameOverPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        EventsPractice.onPlayerDeath += DisplayGameOver;
    }

    private void OnDisable()
    {
        EventsPractice.onPlayerDeath -= DisplayGameOver;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DisplayGameOver()
    {

    }
}
