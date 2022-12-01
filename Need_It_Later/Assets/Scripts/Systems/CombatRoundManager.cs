using System.Collections;
using System.Linq;
using Enemy;
using Singletons;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CombatRoundManager : MonoBehaviour
{
    [Foldout("Parameters", foldEverything = true, styled = true, readOnly = false)]
    public int MaxRounds;

    public string StartRoundInputName;
    public int SelfSceneBuildIndex;
    public float EndRoundDelay = 5f;

    [Foldout("References", foldEverything = true, styled = true, readOnly = false)]
    public EnemyManagerNew TargetEnemyManager;

    public PlayerInput TargetInput;

    [Foldout("Unity Events", foldEverything = true, styled = true, readOnly = false)]
    public UnityEvent<CombatRoundManager> OnStartRoundWait = new();

    public UnityEvent<CombatRoundManager> OnFinishRoundWait = new();
    public UnityEvent<CombatRoundManager> OnStartRoundIntro = new();
    public UnityEvent<CombatRoundManager> OnFinishRoundIntro = new();
    public UnityEvent<CombatRoundManager> OnStartRoundEnd = new();
    public UnityEvent<CombatRoundManager> OnFinishRoundEnd = new();
    public UnityEvent<CombatRoundManager> OnStartEndGame = new();
    public UnityEvent<CombatRoundManager> OnAllRoundEnemiesDead = new();

    [Foldout("Runtime Data", foldEverything = true, styled = true, readOnly = false)]
    public RoundState CurrentState;

    public int ElapsedRounds;
    public float ElapsedTime;

    private bool doneEnding;
    private bool doneRunning;
    private bool doneStarting;
    private bool doneWaiting;

    public Coroutine WaitInstance;
    public Coroutine EndInstance;

    // Start is called before the first frame update
    private void Start()
    {
        StartRound();
    }

    private void Update()
    {
        if (CurrentState == RoundState.Active &&
            !TargetEnemyManager.ActiveEnemies.Any() &&
            EndInstance == null)
        {
            OnAllRoundEnemiesDead.Invoke(this);
            DoFinishRoundActiveDelayed();
        }
            
    }

    public void ForceEnd()
    {
        if (WaitInstance != null)
        {
            StopCoroutine(WaitInstance);
            WaitInstance = null;
        }
    }

    public void StartRound()
    {
        if (WaitInstance != null)
        {
            StopCoroutine(WaitInstance);
            WaitInstance = null;
        }

        WaitInstance = StartCoroutine(ExecuteRounds());
    }

    public void SpawnRoundEnemies()
    {
        TargetEnemyManager.SpawnCircles(
            1,
            TargetEnemyManager._enemyPrefab.GetComponent<CircleCollider2D>().radius,
            ElapsedRounds + 1);
    }

    public void EnableRoundStartInput()
    {
        TargetInput.currentActionMap.FindAction(StartRoundInputName).Enable();
        TargetInput.currentActionMap.actions
            .Where(element => element.name != StartRoundInputName)
            .ToList()
            .ForEach(element => element.Disable());
    }

    public void DisableRoundStartInput()
    {
        TargetInput.currentActionMap.FindAction(StartRoundInputName).Disable();
        TargetInput.currentActionMap.actions
            .Where(element => element.name != StartRoundInputName)
            .ToList()
            .ForEach(element => element.Enable());
    }

    public void ResetScene()
    {
        GameManager.instance.GetComponent<SceneLoader>().DoSceneReset(SelfSceneBuildIndex);
    }

    public void DoFinishRoundWaitInput(InputAction.CallbackContext context)
    {
        if (context.started) DoFinishRoundWait();
    }

    public void DoFinishRoundWait()
    {
        doneWaiting = true;
    }

    public void DoFinishRoundStart()
    {
        doneStarting = true;
    }

    public void DoFinishRoundActive()
    {
        doneRunning = true;
    }

    public void DoFinishRoundActiveDelayed()
    {
        EndInstance = StartCoroutine(WaitFinishRoundActive());
    }

    public IEnumerator WaitFinishRoundActive()
    {
        ElapsedTime = 0f;
        while (ElapsedTime < EndRoundDelay)
        {
            ElapsedTime += Time.smoothDeltaTime;
            yield return null;
        }
        DoFinishRoundActive();
        EndInstance = null;
    }

    public void DoFinishRoundEnd()
    {
        doneEnding = true;
    }

    public IEnumerator ExecuteRounds()
    {
        ElapsedRounds = 0;

        while (true)
        {
            doneWaiting = false;
            doneStarting = false;
            doneRunning = false;
            doneEnding = false;

            // CurrentState = RoundState.Waiting;
            // OnStartRoundWait.Invoke(this);
            // while (!doneWaiting) yield return null;
            // OnFinishRoundWait.Invoke(this);

            CurrentState = RoundState.Starting;
            OnStartRoundIntro.Invoke(this);
            while (!doneStarting) yield return null;
            OnFinishRoundIntro.Invoke(this);

            CurrentState = RoundState.Active;
            while (!doneRunning) yield return null;
            
            ElapsedRounds += 1;

            if (ElapsedRounds < MaxRounds)
            {
                CurrentState = RoundState.Ending;
                OnStartRoundEnd.Invoke(this);
                while (!doneEnding) yield return null;
                OnFinishRoundEnd.Invoke(this);
            }
        }

        OnStartEndGame.Invoke(this);
    }
}

public enum RoundState
{
    Default = 0,
    Waiting = 1,
    Starting = 2,
    Active = 3,
    Ending = 4
}