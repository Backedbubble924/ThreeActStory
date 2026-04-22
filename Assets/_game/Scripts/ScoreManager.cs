using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    public int score;
    public int endGameActivateThreshold;
    public static ScoreManager Instance { get; private set; }
    public GameObject EndGameCollider;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    /*void Start()
    {
        scoreText.text = "Items collected: " + score;
    }*/

    public void IncreaseScore()
    {
        score++;
        scoreText.text = "Items collected: " + score;

        if (score >= endGameActivateThreshold)
        {
            EndGameCollider.SetActive(true);
        }
    }
}
