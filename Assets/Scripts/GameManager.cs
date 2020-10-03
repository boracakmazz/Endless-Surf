using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //private const int COIN_SCORE_AMOUNT = 5;

    public static GameManager Instance { set; get; }

    public bool IsDead { set; get; }
    private bool isGameStarted = false;
    private PlayerController motor;

    //UI 
    public Text scoreText, coinText, modifierText;
    private float score, coinScore, modifierScore;
    private int lastScore;

    //Death menu 
    public Animator deathMenuAnim;

    //Scores on death
    public Text deadScoreText, deadCoinText;

    private void Awake()
    {
        Instance = this;
        modifierScore = 1;
        motor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        scoreText.text = score.ToString("0");
        coinText.text = coinScore.ToString("0");
        modifierText.text = "x" + modifierScore.ToString("0.0");
    }

    private void Update()
    {
        if(MobileInput.Instance.Tap && !isGameStarted)
        {
            isGameStarted = true;
            motor.startGame();
        }

        if (isGameStarted && !IsDead)  //Increase the score over time
        {
           
                score += (Time.deltaTime * modifierScore * 8);
                scoreText.text = score.ToString("0");
            
                if(lastScore != (int)score)
            {
                lastScore = (int)score;
                scoreText.text = score.ToString("0");
            }
        }
    }

    public void UpdateModifier(float modifierAmount)  //Gain points according to your speed
    { 
        modifierScore = 1.0f + modifierAmount;
        modifierText.text = "x" + modifierScore.ToString("0.0");
    }

    public void collectCoin()
    { 
        coinScore++;
        coinText.text = coinScore.ToString("0");
        coinText.text = coinScore.ToString("0");
    }

    public void onPlayButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    public void onDeath()
    {
        deathMenuAnim.SetTrigger("Dead");
        IsDead = true;
        deadScoreText.text = score.ToString("0");
        deadCoinText.text = coinScore.ToString("0");
        
    }
}
