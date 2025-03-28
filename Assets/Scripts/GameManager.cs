using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField] private Text peasantAmountText;
    [SerializeField] private Text warriorAmountText;
    [SerializeField] private Text wheatAmountText;
    [SerializeField] private Text peasantCostText;
    [SerializeField] private Text warriorCostText;
    [SerializeField] private Text raidAmountText;
    [SerializeField] private TextMeshProUGUI winStatsText;
    [SerializeField] private TextMeshProUGUI lossStatsText;
    [SerializeField] private Button peasantTrainButton;
    [SerializeField] private Button warriorTrainButton;
    [SerializeField] private Text muteButtonText;
    [SerializeField] private Image peasantTimerImage;
    [SerializeField] private Image warriorTimerImage;
    [SerializeField] private Image raidTimerImage;
    [SerializeField] private DrainTimer harvestTimer;
    [SerializeField] private DrainTimer upkeepTimer;
    [SerializeField] private RaidTimer raidTimer;
    [SerializeField] private PeasantTimer peasantTrainTimer;
    [SerializeField] private WarriorTimer warriorTrainTimer;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject gameScreen;
    [SerializeField] private GameObject gameWonScreen;
    [SerializeField] private GameObject gameLostScreen;
    [SerializeField] private AudioSource introSound;
    [SerializeField] private AudioSource gameSound;
    [SerializeField] private AudioSource fightSound;
    [SerializeField] private AudioSource trainSound;
    [SerializeField] private AudioSource clickSound;
    [SerializeField] private int peasantCount;
    [SerializeField] private int warriorCount;
    [SerializeField] private int wheatCount;
    [SerializeField] private int wheatToWin;
    [SerializeField] private int peasantsToWin;
    [SerializeField] private int wheatFromPeasant;
    [SerializeField] private int wheatToWarrior;
    [SerializeField] private int peasantCost;
    [SerializeField] private int warriorCost;
    [SerializeField] private float peasantTrainTime;
    [SerializeField] private float warriorTrainTime;
    [SerializeField] private float raidTime;
    [SerializeField] private int raidIncrement;
    [SerializeField] private int nextRaidStrength;
    [SerializeField] private StateMachine stateMachine;

    public bool peasantIsTraining = false;
    public bool warriorIsTraining = false;

    private int peasantsTrained;
    private int warriorsTrained;
    private int enemiesSlain;
    private int wheatGrown;
    private int wheatEaten;
    private float timePlayed;


    void Start()
    {
        Time.timeScale = 0;
        introSound.Play();
        DropStats();
        SetUpEnemies();
        SetUpPrices();
        SetUpTimers();
        UpdateResources();
    }

    void Update()
    {
        HarvestWheat();
        PayUpkeepCost();
        CheckPeasantTraining();
        CheckWarriorTraining();
        CheckEnemies();
        CheckButtons();
        UpdateResources();
        timePlayed += Time.deltaTime;
    }

    void HarvestWheat()
    {
        if (harvestTimer.Tick)
        {
            wheatCount += peasantCount * wheatFromPeasant;
            wheatGrown += peasantCount * wheatFromPeasant;
            CheckWin();
        }
    }

    void PayUpkeepCost()
    {
        if (upkeepTimer.Tick)
        {
            wheatCount -= warriorCount * wheatToWarrior;
            wheatEaten -= warriorCount * wheatToWarrior;
        }
    }



    void CheckPeasantTraining()
    {
        if (peasantTrainTimer.Tick)
        {
            trainSound.Play();
            peasantCount++;
            peasantsTrained++;
            CheckWin();
            peasantIsTraining = false;
        }
    }

    void CheckWarriorTraining()
    {
        if (warriorTrainTimer.Tick)
        {
            trainSound.Play();
            warriorCount++;
            warriorsTrained++;
            warriorIsTraining = false;
        }
    }

    void CheckEnemies()
    {
        if (raidTimer.Tick)
        {   
            fightSound.Play();
            if (warriorCount < nextRaidStrength)
            {
                enemiesSlain += warriorCount;
            }
            else
            {
                enemiesSlain += nextRaidStrength;
            }
            warriorCount -= nextRaidStrength;
            nextRaidStrength += raidIncrement;
            raidTimer.Tick = false;
            CheckLoss();
            SetUpEnemies();
        }
    }

    void CheckButtons()
    {
        if (wheatCount < peasantCost || peasantIsTraining)
        {
            peasantTrainButton.interactable = false;
        }
        else
        {
            peasantTrainButton.interactable = true;
        }
        if (wheatCount < warriorCost || warriorIsTraining)
        {
            warriorTrainButton.interactable = false;
        }
        else
        {
            warriorTrainButton.interactable = true;
        }
    }

    public void TrainPeasant()
    {
        wheatCount -= peasantCost;
        peasantIsTraining = true;
    }

    public void TrainWarrior()
    {
        wheatCount -= warriorCost;
        warriorIsTraining = true;
    }

    void CheckWin()
    {
        if ( peasantCount >= peasantsToWin && wheatCount >= wheatToWin)
        {
            PauseGame();
            stateMachine.ChangeScreen(gameWonScreen);
            PostStats(true);
        }
    }

    void CheckLoss()
    {
        if (warriorCount < 0)
        {
            PauseGame();            
            stateMachine.ChangeScreen(gameLostScreen);
            PostStats(false);
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void PostStats(bool win)
    {
        string pattern = $"Статистика:\n" +
            $"Сыграно: {timePlayed}с\n" +
            $"Нанято крестьян: {peasantsTrained}\n" +
            $"Нанято воинов: {warriorsTrained}\n" +
            $"Собрано пшеницы: {wheatGrown}\n" +
            $"Съедено пшеницы: {wheatEaten}\n" +
            $"Убито врагов: {enemiesSlain}";

        if (win)
        {
            winStatsText.text = pattern ;
        }
        else
        {
            lossStatsText.text = pattern;
        }
    }

    void DropStats()
    {
        peasantsTrained = 0;
        warriorsTrained = 0;
        enemiesSlain = 0;
        timePlayed = 0f;
        wheatGrown = 0;
        wheatEaten = 0;
    }

    void SetUpTimers()
    {
        raidTimer.maxTime = raidTime;
        warriorTrainTimer.maxTime = warriorTrainTime;
        peasantTrainTimer.maxTime = peasantTrainTime;
    }

    void SetUpPrices()
    {
        peasantCostText.text = peasantCost.ToString();
        warriorCostText.text = warriorCost.ToString();
    }

    void UpdateResources()
    {
        peasantAmountText.text = peasantCount.ToString();
        warriorAmountText.text = warriorCount.ToString();
        wheatAmountText.text = wheatCount.ToString();
    }

    void SetUpEnemies()
    {
        raidAmountText.text = $"Враг\nприближается!\nСила войска: {nextRaidStrength}";
    }

    public void MuteSound()
    {
        if (gameSound.isPlaying)
        {
            gameSound.Pause();
            muteButtonText.text = "Включить\nмузыку";
        }
        else
        {
            gameSound.Play();
            muteButtonText.text = "Выключить\nмузыку";
        }
    }
}
