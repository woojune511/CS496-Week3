using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; // Static instance of GameManager which allows it to be accessed by any other script.

    public GameObject player;
    GameObject playerInstance;
    PlayerHealth playerHealth;
    PlayerMoveControl playerMoveControl;
    PlayerAttack playerAttack;

    Vector2 StartingPos;
    Quaternion StartingRotate;
    public bool isStarted = false;

    public bool isBossDead = false;

    // -------------------------------[PLAYER STATUS TEXT]------------------------------
    [SerializeField]
    private Text dmgText;
    [SerializeField]
    private Text attackSpeedText;
    [SerializeField]
    private Text rangeText;
    [SerializeField]
    private Text shotSpeedText;
    [SerializeField]
    private Text speedText;
    // ---------------------------------------------------------------------------------

    private void Awake() {
        if (instance == null) 
            instance = this;
        // If instance already exists and it's not this:
        else if (instance != this)
            // Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        Time.timeScale = 0f; //  정지상태
    }
    // Start is called before the first frame update
    void Start()
    {
        Transform startPoint =  GameObject.FindGameObjectWithTag("Start").transform;
        StartingPos = startPoint.position;
        StartingRotate = startPoint.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// OnGUI is called for rendering and handling GUI events.
    /// This function can be called multiple times per frame (one call per event).
    /// </summary>
    void OnGUI()
    {
        if(!isStarted) {
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            GUILayout.Label("Demo Play");

            if(GUILayout.Button("Start!")) {
                isStarted = true;
                StartGame();
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
    }

    void StartGame() {
        Time.timeScale = 1f; // 정상 시간흐름

        Instantiate(player, StartingPos, StartingRotate);
        playerInstance = GameObject.FindWithTag("Player");
        playerMoveControl = playerInstance.GetComponent<PlayerMoveControl>();
        playerHealth = playerInstance.GetComponent<PlayerHealth>();
        playerAttack = playerInstance.GetComponent<PlayerAttack>();
        playerHealth.health = playerHealth.maxHealth; // 풀피로 시작
        UpdateStatusText();
    }

    public void GameOver()
    {
        isStarted = false;
        enabled = false;
        // GameOver 화면으로
        GameObject.Find("GameOver").transform.Find("GameOverPanel").gameObject.SetActive(true);
    }

    public void UpdateHearts() {
        playerHealth.UpdateHearts();
    }

    public void UpdateStatusText() {
        rangeText.text = $"RAN: {playerAttack.range}";
        attackSpeedText.text = $"ATS: {playerAttack.coolTime}";
        dmgText.text = $"DMG: {playerAttack.damage}";
        shotSpeedText.text = $"SHT: {playerAttack.shotSpeed}";
        speedText.text = $"SPD: {playerMoveControl.speed}";
    }
}
