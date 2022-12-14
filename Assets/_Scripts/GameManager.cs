using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct AnimationPlayer{
    public string name;
    public Animator animation;
}
public class GameManager : MonoBehaviour
{
    private float score;
    private float coinScore;
    [SerializeField]private TextMeshProUGUI scoreText;
    [SerializeField]private TextMeshProUGUI coinText;

    public static GameManager Instance{set; get;}
    private const int COIN_SCORE_AMOUNT = 5;
    public bool IsDead {set; get;}
    public bool isGameStarted;
    //Game current state
    private GameState state;
    public Animator _animator;
    public GameState gameState{get{return state;}}
    [SerializeField] private List<AnimationPlayer> _animationPlayer;

    [Header("Animators")]
    public Animator _InGameAnimator, _PauseMenuAnimator, _GameOverAnimator, _MainMenuAnimator, _StoreAnimator; 
    [Header("Animators Cross")]
    public CrossFadeSettings _InGameAnimatorSettings, _PauseMenuAnimatorSettings, _GameOverAnimatorSettings, _MainMenuAnimatorSettings, _StoreAnimatorSettings; 
    public CrossFadeSettings _InGameAnimatorHideSettings, _PauseMenuAnimatorHideSettings, _GameOverAnimatorHideSettings, _MainMenuAnimatorHideSettings, _StoreAnimatorHideSettings; 
    private AudioManager _audioManager;
    private GameStorage _gameStorage;
    [SerializeField] private TextMeshProUGUI _gameOverScoreText, _gameOverCoinText,  _menuCoinText;

    private string coinScoreKey = "coinScoreKey";

    public List<Coin> coins;
    private void Awake() {
        if(Instance == null){
            Instance = this;
        }else if(Instance != this){
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start() {
        _audioManager = FindObjectOfType<AudioManager>();
        _gameStorage = FindObjectOfType<GameStorage>();
        state = GameState.Idle;

        //updateUITextOfType is an extension method that i manually pegged to monobehavior(it's not a defualt method)
        this.updateUITextOfType("cointext", _gameStorage.getCoin().ToString());
        this.updateUITextOfType("gameplaycointext", "0");
        this.updateUITextOfType("scoretext", "0");

        this.updateUITextOfType("magnettext", _gameStorage.getMagnet().ToString());
        this.updateUITextOfType("speedboosttext", _gameStorage.getSpeedBoost().ToString());
        _audioManager.Background();
        _MainMenuAnimator.CrossFade(_MainMenuAnimatorSettings);
    }

    private void Update() {
        this.updateUITextOfType("gameplayscoretext", ((int)GameObject.FindGameObjectWithTag("Player").transform.position.x).ToString());
        score = GameObject.FindGameObjectWithTag("Player").transform.position.x;
    }

   public void GetCoin(){
        coinScore++;
        this.updateUITextOfType("gameplaycointext", ((int)coinScore).ToString());

    }
    
    public void OnDeath(){
        IsDead = true;
        state = GameState.GameLose;
        ShowGameOverScreen();
        _audioManager.Background(true);
        FindObjectOfType<PoliceAI>().enabled = false;
        _gameStorage.saveCoin(_gameStorage.getCoin() + (int)coinScore);
        this.updateUITextOfType("cointext", _gameStorage.getCoin().ToString());
        _gameOverScoreText.text = ((int)score).ToString();
        _gameOverCoinText.text =  coinScore.ToString();
        
    }

    public void StartGame(){
        FindObjectOfType<PoliceAI>().startChasing = true;
        state = GameState.GameStart;
      _InGameAnimator.CrossFade(_InGameAnimatorSettings);
      _MainMenuAnimator.CrossFade(_MainMenuAnimatorHideSettings);
        FindObjectOfType<CameraFollow>().start();

    }

   public void pause(bool shouldPause){
    if(shouldPause){
      _InGameAnimator.CrossFade(_InGameAnimatorHideSettings);
      _PauseMenuAnimator.CrossFade(_PauseMenuAnimatorSettings);
       FindObjectOfType<PlayerController>().pause(true);
    }else{
      _InGameAnimator.CrossFade(_InGameAnimatorSettings);
      _PauseMenuAnimator.CrossFade(_PauseMenuAnimatorHideSettings);   
       FindObjectOfType<PlayerController>().pause(false);
    }
    }


    public void ShowGameOverScreen(){
        _GameOverAnimator.CrossFade(_GameOverAnimatorSettings);
      _InGameAnimator.CrossFade(_InGameAnimatorHideSettings);
    }

   public void showStore(bool shouldShow){
    if(shouldShow){  
      _StoreAnimator.CrossFade(_StoreAnimatorSettings);
      _MainMenuAnimator.CrossFade(_MainMenuAnimatorHideSettings); 
    }else{
      _StoreAnimator.CrossFade(_StoreAnimatorHideSettings);
      _MainMenuAnimator.CrossFade(_MainMenuAnimatorSettings); 
    }
    }

    public void restartGame(){
        SceneManager.LoadSceneAsync("Main screen");
    } 

    // private void updateUITextOfType(string type, string newText){
    //    foreach(UIText uiText in FindObjectsOfType<UIText>()){
    //     if(uiText.name == type)
    //     uiText.updateText(newText);
    //    }
    // }
}

