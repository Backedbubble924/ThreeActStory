# Three-Act Story - Exercise Solutions & Hints

This document provides hints, guidance, and complete solutions for the student exercises. Try to solve exercises on your own first before checking solutions!

---

## Easy Exercises - Solutions

### Exercise 1: Implement the Start State Behavior

**Hint:**
Look at how Act_1_State.cs is implemented. You'll uncomment the methods and add Debug.Log statements.

**Solution:**
```csharp
using UnityEngine;

public class Start_State : StateMachineBehaviour
{
    // Called when entering the Start state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Welcome to the Three-Act Journey!");
    }

    // Called on each Update frame (optional to implement)
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Can be left empty or add update logic if needed
    }

    // Called when exiting the Start state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("The adventure begins...");
    }
}
```

**Key Concepts:**
- `OnStateEnter()` runs once when animator enters this state
- `OnStateExit()` runs once when leaving this state
- `OnStateUpdate()` runs every frame while in this state

---

### Exercise 2: Implement the End State Behavior

**Hint:**
Use `Time.time` to get the total seconds since game started. Format it nicely with `F1` or `F2`.

**Solution:**
```csharp
using UnityEngine;

public class End_State : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Congratulations! Story Complete!");
        Debug.Log($"Total time played: {Time.time:F2} seconds");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Optional: Could add logic here
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Returning to menu...");
    }
}
```

**Alternative (more readable time format):**
```csharp
override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
{
    Debug.Log("Congratulations! Story Complete!");

    float totalSeconds = Time.time;
    int minutes = (int)(totalSeconds / 60);
    int seconds = (int)(totalSeconds % 60);
    Debug.Log($"Total time played: {minutes}m {seconds}s");
}
```

---

### Exercise 3: Add a Current Act Display

**Hint:**
- Create UI: Canvas → UI → Text (or TextMeshPro)
- New script needs a reference to the Text component
- Update text in `Update()` method
- Access Gamemanager using the singleton: `Gamemanager.Instance`

**Solution - ActDisplay.cs:**
```csharp
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays the current story act on screen
/// </summary>
public class ActDisplay : MonoBehaviour
{
    [SerializeField] private Text actText;

    void Update()
    {
        if (Gamemanager.Instance != null)
        {
            StoryAct currentAct = Gamemanager.Instance.GetCurrentAct();
            actText.text = $"Current Act: {GetActDisplayName(currentAct)}";
        }
    }

    /// <summary>
    /// Gets a display name for the act
    /// </summary>
    private string GetActDisplayName(StoryAct act)
    {
        return act switch
        {
            StoryAct.Start => "Start State",
            StoryAct.Departure => "Act 1: Departure",
            StoryAct.Initiation => "Act 2: Initiation",
            StoryAct.Return => "Act 3: Return",
            StoryAct.End => "End State",
            _ => act.ToString()
        };
    }
}
```

**Bonus Solution - With Color Coding:**
```csharp
using UnityEngine;
using UnityEngine.UI;

public class ActDisplay : MonoBehaviour
{
    [SerializeField] private Text actText;

    void Update()
    {
        if (Gamemanager.Instance != null)
        {
            StoryAct currentAct = Gamemanager.Instance.GetCurrentAct();
            actText.text = $"Current Act: {GetActDisplayName(currentAct)}";
            actText.color = GetActColor(currentAct);
        }
    }

    private string GetActDisplayName(StoryAct act)
    {
        return act switch
        {
            StoryAct.Start => "Start State",
            StoryAct.Departure => "Act 1: Departure",
            StoryAct.Initiation => "Act 2: Initiation",
            StoryAct.Return => "Act 3: Return",
            StoryAct.End => "End State",
            _ => act.ToString()
        };
    }

    private Color GetActColor(StoryAct act)
    {
        return act switch
        {
            StoryAct.Start => Color.green,
            StoryAct.Departure => Color.yellow,
            StoryAct.Initiation => Color.yellow,
            StoryAct.Return => Color.yellow,
            StoryAct.End => Color.cyan,
            _ => Color.white
        };
    }
}
```

**Setup Steps:**
1. Right-click in Hierarchy → UI → Canvas
2. Right-click Canvas → UI → Text
3. Position text in top-left (Anchor to top-left, Position X: 150, Y: -50)
4. Create ActDisplay.cs script
5. Attach script to the Text GameObject
6. Drag the Text component into the actText field in Inspector

---

### Exercise 4: Add Act Completion Counter

**Hint:**
Add a private static int variable so it persists across instances. Increment in OnStateEnter.

**Solution - Act_1_State.cs (apply same pattern to Act_2 and Act_3):**
```csharp
using UnityEngine;

public class Act_1_State : StateMachineBehaviour
{
    // Static variable persists across all instances
    private static int visitCount = 0;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        visitCount++;
        Debug.Log($"Entering Act 1: Departure (Visit #{visitCount})");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Act 1 update logic
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Exiting Act 1: Departure");
    }
}
```

**Why static?**
- Without `static`, each new instance of the state would reset the counter
- `static` means the variable is shared across all instances and persists

**Testing Note:**
The current system doesn't have a reset feature, so you'll only see Visit #1 unless you implement Exercise 5 (Reset feature).

---

## Moderate Exercises - Solutions

### Exercise 5: Add a Reset to Start Feature

**Hint:**
- Add key detection in `Update()` method
- Create new method that directly calls `SetAct(StoryAct.Start)`
- Bypass the validation in `TryTransition()`

**Solution - Add to Gamemanager.cs:**

```csharp
// Add to the Update() method, after the existing key checks:
void Update()
{
    // Test state changes with keypresses (0-4)
    if (Input.GetKeyDown(KeyCode.Alpha0)) TryTransition(StoryAct.Start);
    else if (Input.GetKeyDown(KeyCode.Alpha1)) TryTransition(StoryAct.Departure);
    else if (Input.GetKeyDown(KeyCode.Alpha2)) TryTransition(StoryAct.Initiation);
    else if (Input.GetKeyDown(KeyCode.Alpha3)) TryTransition(StoryAct.Return);
    else if (Input.GetKeyDown(KeyCode.Alpha4)) TryTransition(StoryAct.End);

    // NEW: Reset functionality
    else if (Input.GetKeyDown(KeyCode.R)) ResetToStart();
}

// NEW METHOD: Add this method to Gamemanager class
/// <summary>
/// Resets the story to the Start state, bypassing normal progression rules.
/// </summary>
public void ResetToStart()
{
    Debug.Log("Resetting story to beginning...");
    SetAct(StoryAct.Start);
}
```

**Why bypass TryTransition?**
- `TryTransition()` has validation rules that prevent going backwards
- Reset should work from ANY state (even End state back to Start)
- We call `SetAct()` directly to bypass those rules

**Alternative - With confirmation:**
```csharp
public void ResetToStart()
{
    if (act != StoryAct.Start)
    {
        Debug.Log($"Resetting story from {GetActName(act)} to beginning...");
        SetAct(StoryAct.Start);
    }
    else
    {
        Debug.Log("Already at Start state");
    }
}
```

---

### Exercise 6: Save and Load Story Progress

**Hint:**
- Use `PlayerPrefs.SetInt("SavedAct", (int)act)` to save
- Use `PlayerPrefs.GetInt("SavedAct", 0)` to load (0 is default if no save exists)
- Save in `SetAct()` method (auto-save on every transition)
- Load in `Awake()` after singleton setup

**Solution - Modify Gamemanager.cs:**

```csharp
void Awake()
{
    if (Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        storyAnimator = GetComponent<Animator>();

        // NEW: Load saved progress
        LoadProgress();
    }
    else
    {
        Destroy(gameObject);
    }
}

void Update()
{
    // Existing key checks...
    if (Input.GetKeyDown(KeyCode.Alpha0)) TryTransition(StoryAct.Start);
    else if (Input.GetKeyDown(KeyCode.Alpha1)) TryTransition(StoryAct.Departure);
    else if (Input.GetKeyDown(KeyCode.Alpha2)) TryTransition(StoryAct.Initiation);
    else if (Input.GetKeyDown(KeyCode.Alpha3)) TryTransition(StoryAct.Return);
    else if (Input.GetKeyDown(KeyCode.Alpha4)) TryTransition(StoryAct.End);
    else if (Input.GetKeyDown(KeyCode.R)) ResetToStart();

    // NEW: Manual save/load controls
    else if (Input.GetKeyDown(KeyCode.S)) SaveProgress();
    else if (Input.GetKeyDown(KeyCode.L)) LoadProgress();
}

/// <summary>
/// Saves the current story progress to PlayerPrefs.
/// </summary>
private void SaveProgress()
{
    PlayerPrefs.SetInt("SavedAct", (int)act);
    PlayerPrefs.Save(); // Force save immediately
    Debug.Log($"Progress saved: {GetActName(act)}");
}

/// <summary>
/// Loads saved story progress from PlayerPrefs.
/// </summary>
private void LoadProgress()
{
    if (PlayerPrefs.HasKey("SavedAct"))
    {
        int savedActValue = PlayerPrefs.GetInt("SavedAct", 0);
        StoryAct savedAct = (StoryAct)savedActValue;
        SetAct(savedAct);
        Debug.Log($"Progress loaded: {GetActName(savedAct)}");
    }
    else
    {
        Debug.Log("No saved progress found. Starting from beginning.");
        SetAct(StoryAct.Start);
    }
}

// MODIFY existing SetAct method to auto-save:
public void SetAct(StoryAct newAct)
{
    act = newAct;
    storyAnimator.SetInteger("Act", (int)act);

    // NEW: Auto-save on every act change
    SaveProgress();
}
```

**Bonus Solution - With Timestamp:**

```csharp
private void SaveProgress()
{
    PlayerPrefs.SetInt("SavedAct", (int)act);
    PlayerPrefs.SetString("SavedTimestamp", System.DateTime.Now.ToString());
    PlayerPrefs.Save();
    Debug.Log($"Progress saved: {GetActName(act)} at {System.DateTime.Now:HH:mm:ss}");
}

private void LoadProgress()
{
    if (PlayerPrefs.HasKey("SavedAct"))
    {
        int savedActValue = PlayerPrefs.GetInt("SavedAct", 0);
        StoryAct savedAct = (StoryAct)savedActValue;
        string timestamp = PlayerPrefs.GetString("SavedTimestamp", "Unknown time");

        SetAct(savedAct);
        Debug.Log($"Welcome back! Last played: {timestamp}");
        Debug.Log($"Progress loaded: {GetActName(savedAct)}");
    }
    else
    {
        Debug.Log("No saved progress found. Starting from beginning.");
        SetAct(StoryAct.Start);
    }
}
```

**Testing:**
1. Play game, progress to Act 2
2. Press S to save (or it auto-saves)
3. Stop Play mode
4. Start Play mode again
5. Should automatically load at Act 2

---

### Exercise 7: Add Act Transition Sound Effects

**Hint:**
- Add AudioSource component to Gamemanager GameObject
- Create Resources/Sounds folder
- Place audio files in Resources folder
- Load with `Resources.Load<AudioClip>("Sounds/filename")`
- Play with `AudioSource.PlayOneShot(clip)`

**Solution - Add to Act_1_State.cs (apply pattern to all states):**

```csharp
using UnityEngine;

public class Act_1_State : StateMachineBehaviour
{
    private AudioClip enterSound;

    // Called when state machine starts (loads the sound once)
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Entering Act 1: Departure");

        // Load sound from Resources folder (do this once)
        if (enterSound == null)
        {
            enterSound = Resources.Load<AudioClip>("Sounds/Act1Enter");
        }

        // Play the sound
        if (enterSound != null)
        {
            AudioSource audioSource = animator.gameObject.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.PlayOneShot(enterSound);
            }
            else
            {
                Debug.LogWarning("No AudioSource found on Gamemanager!");
            }
        }
        else
        {
            Debug.LogWarning("Sound file not found: Sounds/Act1Enter");
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Update logic
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Exiting Act 1: Departure");
    }
}
```

**Better Solution - Centralized Sound Manager:**

Create a new script `SoundManager.cs`:
```csharp
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    private AudioSource audioSource;

    [Header("Story Act Sounds")]
    public AudioClip startSound;
    public AudioClip act1Sound;
    public AudioClip act2Sound;
    public AudioClip act3Sound;
    public AudioClip endSound;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
        }
    }

    public static void PlayActSound(StoryAct act)
    {
        if (instance == null) return;

        AudioClip clipToPlay = act switch
        {
            StoryAct.Start => instance.startSound,
            StoryAct.Departure => instance.act1Sound,
            StoryAct.Initiation => instance.act2Sound,
            StoryAct.Return => instance.act3Sound,
            StoryAct.End => instance.endSound,
            _ => null
        };

        if (clipToPlay != null && instance.audioSource != null)
        {
            instance.audioSource.PlayOneShot(clipToPlay);
        }
    }
}
```

Then in each state script, simply call:
```csharp
override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
{
    Debug.Log("Entering Act 1: Departure");
    SoundManager.PlayActSound(StoryAct.Departure);
}
```

**Setup:**
1. Add AudioSource component to Gamemanager GameObject
2. Add SoundManager script to Gamemanager GameObject
3. Create/find 5 audio clips
4. Drag audio clips into SoundManager fields in Inspector

---

### Exercise 8: Create a Progress Bar UI

**Hint:**
- UI Image with Fill Type = Filled, Fill Method = Horizontal
- Update `fillAmount` property (0.0 to 1.0)
- Calculate: `currentActValue / 4.0f` (4 is max act value)

**Solution - ProgressBar.cs:**

```csharp
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays story progress as a visual progress bar
/// </summary>
public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Text percentageText; // Optional

    private const float MAX_ACT_VALUE = 4.0f; // End state is act 4

    void Update()
    {
        if (Gamemanager.Instance != null && fillImage != null)
        {
            // Get current act as integer (0-4)
            int currentActValue = (int)Gamemanager.Instance.GetCurrentAct();

            // Calculate progress (0.0 to 1.0)
            float progress = currentActValue / MAX_ACT_VALUE;

            // Update fill amount
            fillImage.fillAmount = progress;

            // Update percentage text if it exists
            if (percentageText != null)
            {
                percentageText.text = $"{progress * 100:F0}%";
            }
        }
    }
}
```

**Bonus Solution - Smooth Animation:**

```csharp
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Text percentageText;
    [SerializeField] private float smoothSpeed = 5f;

    private const float MAX_ACT_VALUE = 4.0f;
    private float targetFillAmount = 0f;
    private float currentFillAmount = 0f;

    void Update()
    {
        if (Gamemanager.Instance != null && fillImage != null)
        {
            // Calculate target progress
            int currentActValue = (int)Gamemanager.Instance.GetCurrentAct();
            targetFillAmount = currentActValue / MAX_ACT_VALUE;

            // Smoothly lerp to target
            currentFillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, Time.deltaTime * smoothSpeed);
            fillImage.fillAmount = currentFillAmount;

            // Update percentage text
            if (percentageText != null)
            {
                percentageText.text = $"{currentFillAmount * 100:F0}%";
            }

            // Color gradient (optional)
            fillImage.color = Color.Lerp(Color.green, Color.cyan, currentFillAmount);
        }
    }
}
```

**UI Setup:**
1. Create Canvas if not exists
2. Create UI → Image (this will be background) - make it dark gray
3. Create child UI → Image (this will be fill bar) - make it green/blue
4. Select fill Image → Image component:
   - Image Type: Filled
   - Fill Method: Horizontal
   - Fill Origin: Left
5. Attach ProgressBar script to the parent Image
6. Drag fill Image into fillImage field

---

### Exercise 9: Add Act Duration Tracking

**Hint:**
- Store `Time.time` when entering state
- Calculate `duration = Time.time - enterTime` when exiting
- Format nicely with `:F1` or `:F2`

**Solution - Act_1_State.cs (apply to all act states):**

```csharp
using UnityEngine;

public class Act_1_State : StateMachineBehaviour
{
    private float enterTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Record the time when entering this act
        enterTime = Time.time;
        Debug.Log("Entering Act 1: Departure");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Update logic
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Calculate how long was spent in this act
        float duration = Time.time - enterTime;
        Debug.Log($"Exiting Act 1: Departure");
        Debug.Log($"Completed Act 1: Departure in {duration:F2} seconds");
    }
}
```

**Bonus Solution - Total Time Tracker:**

Create a new `TimeTracker.cs` script:
```csharp
using UnityEngine;

/// <summary>
/// Tracks total time spent in the story
/// </summary>
public class TimeTracker : MonoBehaviour
{
    public static TimeTracker Instance { get; private set; }

    private float totalStoryTime = 0f;
    private float currentActStartTime = 0f;
    private bool isTracking = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Update()
    {
        if (isTracking)
        {
            totalStoryTime += Time.deltaTime;
        }
    }

    public void StartTracking()
    {
        currentActStartTime = Time.time;
        isTracking = true;
    }

    public void StopTracking()
    {
        isTracking = false;
    }

    public float GetCurrentActDuration()
    {
        return Time.time - currentActStartTime;
    }

    public float GetTotalStoryTime()
    {
        return totalStoryTime;
    }

    public string GetFormattedTotalTime()
    {
        int minutes = (int)(totalStoryTime / 60);
        int seconds = (int)(totalStoryTime % 60);
        return $"{minutes}m {seconds}s";
    }
}
```

Then modify states to use it:
```csharp
override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
{
    Debug.Log("Entering Act 1: Departure");
    TimeTracker.Instance?.StartTracking();
}

override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
{
    float actDuration = TimeTracker.Instance?.GetCurrentActDuration() ?? 0f;
    string totalTime = TimeTracker.Instance?.GetFormattedTotalTime() ?? "0m 0s";

    Debug.Log($"Completed Act 1: Departure in {actDuration:F2} seconds");
    Debug.Log($"Total story time: {totalTime}");
}
```

---

## Challenging Exercises - Solutions

### Exercise 10: Implement Branching Story Paths

**This is a complex exercise. Here's a step-by-step guide:**

**Step 1: Modify the StoryAct Enum**

```csharp
public enum StoryAct
{
    Start = 0,
    Departure = 1,
    InitiationPathA = 2,  // Renamed and split Act 2
    InitiationPathB = 3,  // New path
    Return = 4,           // Renumbered
    End = 5              // Renumbered
}
```

**Step 2: Update Gamemanager.cs**

```csharp
void Update()
{
    // Normal progression keys
    if (Input.GetKeyDown(KeyCode.Alpha0)) TryTransition(StoryAct.Start);
    else if (Input.GetKeyDown(KeyCode.Alpha1)) TryTransition(StoryAct.Departure);

    // NEW: Branch choice keys (only work from Act 1)
    else if (Input.GetKeyDown(KeyCode.Q)) TryBranchTransition(StoryAct.InitiationPathA);
    else if (Input.GetKeyDown(KeyCode.E)) TryBranchTransition(StoryAct.InitiationPathB);

    else if (Input.GetKeyDown(KeyCode.Alpha3)) TryTransition(StoryAct.Return);
    else if (Input.GetKeyDown(KeyCode.Alpha4)) TryTransition(StoryAct.End);
}

/// <summary>
/// Special transition handler for branching paths
/// </summary>
private void TryBranchTransition(StoryAct targetBranch)
{
    // Can only branch from Act 1 (Departure)
    if (act == StoryAct.Departure)
    {
        string branchName = targetBranch == StoryAct.InitiationPathA ? "Path A: The Warrior's Trial" : "Path B: The Scholar's Path";
        Debug.Log($"Choosing {branchName}");
        SetAct(targetBranch);
    }
    else if (act == StoryAct.Start)
    {
        Debug.Log("Complete Act 1: Departure before choosing a path");
    }
    else
    {
        Debug.Log("Branch choice is only available after Act 1: Departure");
    }
}

private string GetActName(StoryAct actToName)
{
    return actToName switch
    {
        StoryAct.Start => "Start state",
        StoryAct.Departure => "Act 1: Departure",
        StoryAct.InitiationPathA => "Act 2A: The Warrior's Trial",
        StoryAct.InitiationPathB => "Act 2B: The Scholar's Path",
        StoryAct.Return => "Act 3: Return",
        StoryAct.End => "End state",
        _ => actToName.ToString()
    };
}
```

**Step 3: Update TryTransition validation**

```csharp
private void TryTransition(StoryAct targetAct)
{
    int currentActValue = (int)act;
    int targetActValue = (int)targetAct;

    if (act == targetAct)
    {
        Debug.Log($"Already in {GetActName(targetAct)}");
    }
    else if (targetActValue < currentActValue)
    {
        Debug.Log($"Cannot go backwards to {GetActName(targetAct)}");
    }
    else if (targetActValue > currentActValue + 1)
    {
        // Special case: Can go from either branch (2 or 3) to Return (4)
        if ((act == StoryAct.InitiationPathA || act == StoryAct.InitiationPathB) && targetAct == StoryAct.Return)
        {
            Debug.Log($"Transitioning to {GetActName(targetAct)}");
            SetAct(targetAct);
        }
        else
        {
            Debug.Log($"Cannot skip acts. Must progress sequentially.");
        }
    }
    else
    {
        Debug.Log($"Transitioning to {GetActName(targetAct)}");
        SetAct(targetAct);
    }
}
```

**Step 4: Create new state scripts**

`Act_2A_State.cs`:
```csharp
using UnityEngine;

public class Act_2A_State : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Entering Act 2A: The Warrior's Trial");
        Debug.Log("You have chosen the path of strength and combat!");
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Exiting Act 2A: The Warrior's Trial");
    }
}
```

`Act_2B_State.cs`:
```csharp
using UnityEngine;

public class Act_2B_State : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Entering Act 2B: The Scholar's Path");
        Debug.Log("You have chosen the path of wisdom and knowledge!");
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Exiting Act 2B: The Scholar's Path");
    }
}
```

**Step 5: Update Animator Controller**
1. Open StoryStates.controller
2. Delete existing "Act 2: Initiation" state
3. Create two new states: "Act 2A: Warrior" and "Act 2B: Scholar"
4. Add transitions:
   - Act 1 → Act 2A (condition: Act equals 2)
   - Act 1 → Act 2B (condition: Act equals 3)
   - Act 2A → Act 3 (condition: Act equals 4)
   - Act 2B → Act 3 (condition: Act equals 4)
5. Attach Act_2A_State script to Act 2A state
6. Attach Act_2B_State script to Act 2B state

---

### Exercise 11: Create an Achievement System

**Full solution with achievement manager:**

**Achievement.cs:**
```csharp
using UnityEngine;

[System.Serializable]
public class Achievement
{
    public string id;
    public string title;
    public string description;
    public bool isUnlocked;
    public string unlockTimestamp;

    public Achievement(string id, string title, string description)
    {
        this.id = id;
        this.title = title;
        this.description = description;
        this.isUnlocked = false;
        this.unlockTimestamp = "";
    }

    public void Unlock()
    {
        if (!isUnlocked)
        {
            isUnlocked = true;
            unlockTimestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
```

**AchievementManager.cs:**
```csharp
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private Text notificationText;
    [SerializeField] private float notificationDuration = 3f;

    private List<Achievement> achievements = new List<Achievement>();
    private float notificationTimer = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeAchievements();
            LoadAchievements();
        }
    }

    void Update()
    {
        // Hide notification after duration
        if (notificationTimer > 0)
        {
            notificationTimer -= Time.deltaTime;
            if (notificationTimer <= 0 && notificationPanel != null)
            {
                notificationPanel.SetActive(false);
            }
        }

        // Check for achievement unlock conditions
        CheckAchievements();
    }

    private void InitializeAchievements()
    {
        achievements.Add(new Achievement("first_steps", "First Steps", "Complete Act 1: Departure"));
        achievements.Add(new Achievement("midpoint_master", "Midpoint Master", "Complete Act 2: Initiation"));
        achievements.Add(new Achievement("journeys_end", "Journey's End", "Complete Act 3: Return"));
        achievements.Add(new Achievement("speed_runner", "Speed Runner", "Complete Act 1 in under 30 seconds"));
        achievements.Add(new Achievement("story_complete", "Story Complete", "Reach the End state"));
    }

    private void CheckAchievements()
    {
        if (Gamemanager.Instance == null) return;

        StoryAct currentAct = Gamemanager.Instance.GetCurrentAct();

        // Check act completion achievements
        if (currentAct >= StoryAct.Departure)
            TryUnlockAchievement("first_steps");

        if (currentAct >= StoryAct.Initiation)
            TryUnlockAchievement("midpoint_master");

        if (currentAct >= StoryAct.Return)
            TryUnlockAchievement("journeys_end");

        if (currentAct == StoryAct.End)
            TryUnlockAchievement("story_complete");

        // Speed runner check would need time tracking from Exercise 9
    }

    public void TryUnlockAchievement(string achievementId)
    {
        Achievement achievement = achievements.Find(a => a.id == achievementId);
        if (achievement != null && !achievement.isUnlocked)
        {
            achievement.Unlock();
            ShowNotification($"Achievement Unlocked: {achievement.title}");
            SaveAchievements();
        }
    }

    private void ShowNotification(string message)
    {
        if (notificationPanel != null && notificationText != null)
        {
            notificationText.text = message;
            notificationPanel.SetActive(true);
            notificationTimer = notificationDuration;
            Debug.Log(message);
        }
    }

    private void SaveAchievements()
    {
        foreach (var achievement in achievements)
        {
            PlayerPrefs.SetInt($"Achievement_{achievement.id}", achievement.isUnlocked ? 1 : 0);
            if (achievement.isUnlocked)
            {
                PlayerPrefs.SetString($"Achievement_{achievement.id}_Time", achievement.unlockTimestamp);
            }
        }
        PlayerPrefs.Save();
    }

    private void LoadAchievements()
    {
        foreach (var achievement in achievements)
        {
            if (PlayerPrefs.HasKey($"Achievement_{achievement.id}"))
            {
                bool isUnlocked = PlayerPrefs.GetInt($"Achievement_{achievement.id}") == 1;
                if (isUnlocked)
                {
                    achievement.isUnlocked = true;
                    achievement.unlockTimestamp = PlayerPrefs.GetString($"Achievement_{achievement.id}_Time", "");
                }
            }
        }
    }

    public List<Achievement> GetAllAchievements()
    {
        return achievements;
    }
}
```

Due to space constraints, I'm providing the core structure. Students should expand this with:
- UI panel for viewing all achievements
- Visual notification effects
- Integration with time tracking for speed run achievement

---

### Exercises 12-14: Advanced Challenges

These exercises require substantial implementation. Here are architectural hints:

**Exercise 12 - Collectibles:**
- Create prefab with SphereCollider (isTrigger = true)
- CollectibleManager singleton tracks count
- Modify TryTransition to check: `if (CollectibleManager.Instance.GetCount() < 3) { block transition }`
- Reset count in OnStateEnter for each act

**Exercise 13 - Dialogue System:**
- Use Dictionary<StoryAct, string>
- OnTriggerStay + Input.GetKeyDown(KeyCode.E) for interaction
- UI Panel with Text component
- Query current act and display appropriate dialogue

**Exercise 14 - Story Text Display:**
- Full-screen Canvas with semi-transparent Panel
- Use Coroutine with CanvasGroup for fading:
```csharp
IEnumerator FadeInOutText()
{
    // Fade in over 1 second
    yield return FadeTo(1f, 1f);
    // Hold for 3 seconds
    yield return new WaitForSeconds(3f);
    // Fade out over 1 second
    yield return FadeTo(0f, 1f);
}
```

---

## General Tips for All Exercises

1. **Test frequently** - Don't write everything before testing
2. **Use Debug.Log liberally** - Understand what's happening
3. **Comment your code** - Explain non-obvious logic
4. **Follow naming conventions** - PascalCase for public, camelCase for private
5. **Handle null references** - Always check if objects exist before using them
6. **Save often** - Unity can crash; Ctrl+S frequently

## Common Mistakes to Avoid

1. **Forgetting to assign references** in Inspector
2. **Not checking for null** before accessing singletons
3. **Using = instead of ==** in conditionals
4. **Forgetting to attach scripts** to GameObjects
5. **Misspelling PlayerPrefs keys** when saving/loading
6. **Not calling base methods** when overriding (though not needed for StateMachineBehaviour)

---

**Remember:** These are learning exercises. It's okay to struggle, make mistakes, and iterate. The goal is understanding, not perfection!