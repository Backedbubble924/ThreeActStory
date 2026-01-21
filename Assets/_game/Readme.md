# Three-Act Story Game Manager

## Overview

This project demonstrates a Unity game manager that controls story progression through three acts using an **Animator Controller** and **State Machine**. The code enforces sequential progression through the story, preventing players from skipping ahead or going backwards.

## Project Structure

The system consists of:
- **Gamemanager.cs** - Main singleton script that manages story progression
- **Start_State.cs** - State behavior for the Start state (currently unused, ready for implementation)
- **Act_1_State.cs** - State behavior for Act 1: Departure
- **Act_2_State.cs** - State behavior for Act 2: Initiation
- **Act_3_State.cs** - State behavior for Act 3: Return
- **End_State.cs** - State behavior for the End state (currently unused, ready for implementation)
- **Animator Controller** - Unity animator with states for each act

## How It Works

### The StoryAct Enum

```csharp
public enum StoryAct
{
    Start = 0,
    Departure = 1,
    Initiation = 2,
    Return = 3,
    End = 4
}
```

An **enum** (enumeration) is a special type that represents a group of named constants. Think of it as a list of options where each option has a number value.

- Instead of using confusing numbers like `0`, `1`, `2`, `3`, `4`, we can use readable names like `StoryAct.Start`, `StoryAct.Departure`, etc.
- We explicitly set values from 0 to 4 to match the Animator Controller parameter
- This makes the code more readable: `act == StoryAct.Departure` is clearer than `act == 1`
- The Start state (0) represents the initial state before any acts begin
- The End state (4) represents the final state after all acts are complete

### Singleton Pattern

```csharp
public static Gamemanager Instance { get; private set; }
```

The Gamemanager uses the **Singleton pattern**, which means:
- Only ONE instance of Gamemanager exists in the entire game
- Other scripts can access it using `Gamemanager.Instance`
- The object persists between scene changes (using `DontDestroyOnLoad`)

**Why use this?**
- Story progress needs to be tracked across multiple scenes
- Multiple scripts need to check the current story act
- We want to prevent duplicate managers from being created

### Key Press Detection

```csharp
void Update()
{
    if (Input.GetKeyDown(KeyCode.Alpha0)) TryTransition(StoryAct.Start);
    else if (Input.GetKeyDown(KeyCode.Alpha1)) TryTransition(StoryAct.Departure);
    else if (Input.GetKeyDown(KeyCode.Alpha2)) TryTransition(StoryAct.Initiation);
    else if (Input.GetKeyDown(KeyCode.Alpha3)) TryTransition(StoryAct.Return);
    else if (Input.GetKeyDown(KeyCode.Alpha4)) TryTransition(StoryAct.End);
}
```

This code runs every frame and checks if the player pressed keys 0, 1, 2, 3, or 4. When a key is pressed, it attempts to transition to the corresponding act.

**Key Mappings:**
- **0** → Start state
- **1** → Act 1: Departure
- **2** → Act 2: Initiation
- **3** → Act 3: Return
- **4** → End state

**Note:** These keypresses are for testing/development. In a real game, you'd trigger transitions through gameplay events (completing objectives, entering areas, etc.)

### Transition Validation Logic

The `TryTransition()` method enforces the rules of story progression:

```csharp
private void TryTransition(StoryAct targetAct)
{
    int currentActValue = (int)act;      // Current act as a number (1-4)
    int targetActValue = (int)targetAct; // Target act as a number (1-4)

    if (act == targetAct)
    {
        // Already in this act - do nothing
    }
    else if (targetActValue < currentActValue)
    {
        // Trying to go backwards - not allowed
    }
    else if (targetActValue > currentActValue + 1)
    {
        // Trying to skip acts - not allowed
    }
    else
    {
        // Valid transition - move to next act
        SetAct(targetAct);
    }
}
```

**Validation Rules:**
1. **Can't repeat** - If already in Act 2, pressing "2" does nothing
2. **Can't go backwards** - If in Act 3, can't go back to Act 2 or 1
3. **Can't skip ahead** - If in Act 1, pressing "3" won't jump to Act 3
4. **Must progress sequentially** - Can only move to the next act in sequence

**Example scenarios:**
- Current: Start, Press "1" ✅ Allowed (moving forward one act)
- Current: Start, Press "2" ❌ Blocked (trying to skip Act 1)
- Current: Act 1, Press "2" ✅ Allowed (moving forward one act)
- Current: Act 1, Press "3" ❌ Blocked (trying to skip Act 2)
- Current: Act 3, Press "2" ❌ Blocked (trying to go backwards)
- Current: Act 3, Press "4" ✅ Allowed (moving to End state)

### Switch Expression

```csharp
private string GetActName(StoryAct actToName)
{
    return actToName switch
    {
        StoryAct.Start => "Start state",
        StoryAct.Departure => "Act 1: Departure",
        StoryAct.Initiation => "Act 2: Initiation",
        StoryAct.Return => "Act 3: Return",
        StoryAct.End => "End state",
        _ => actToName.ToString()
    };
}
```

This uses a **switch expression** (a modern C# feature) to convert enum values to readable strings for debug logs.

- Each line checks if `actToName` matches a specific enum value
- The `=>` means "return this string"
- The `_` is a **default case** that catches anything else (should never happen with a complete enum)

**Traditional switch statement equivalent:**
```csharp
switch (actToName)
{
    case StoryAct.Start:
        return "Start state";
    case StoryAct.Departure:
        return "Act 1: Departure";
    case StoryAct.Initiation:
        return "Act 2: Initiation";
    // etc...
}
```

### Animator Integration

```csharp
public void SetAct(StoryAct newAct)
{
    act = newAct;
    storyAnimator.SetInteger("Act", (int)act);
}
```

This method updates both:
1. The internal `act` variable (for code logic)
2. The Animator Controller parameter called "Act" (for visual state changes)

**How it connects:**
- Animator Controller has an integer parameter named "Act"
- States transition when Act = 0 (Start), 1 (Departure), 2 (Initiation), 3 (Return), or 4 (End)
- This allows Unity's animator to trigger visual changes, animations, or other game events

## State Machine Behaviors

Each act has a corresponding `StateMachineBehaviour` script that defines what happens during different phases of the state:

### Currently Implemented:
- **Act_1_State.cs** - Behavior for Act 1: Departure
- **Act_2_State.cs** - Behavior for Act 2: Initiation
- **Act_3_State.cs** - Behavior for Act 3: Return

### Ready for Implementation:
- **Start_State.cs** - Behavior for the Start state (template with commented methods)
- **End_State.cs** - Behavior for the End state (template with commented methods)

Each state script has three key methods:
- **OnStateEnter** - Called when entering the state (e.g., play intro, spawn enemies, show UI)
- **OnStateUpdate** - Called every frame while in the state (e.g., check objectives, update timers)
- **OnStateExit** - Called when leaving the state (e.g., cleanup, save progress, hide UI)

The Start_State and End_State scripts are currently templates with commented-out methods. You can uncomment and implement these methods to add functionality to the Start and End states.

## Using This System in Your Game

### From Other Scripts

```csharp
// Get the current act
StoryAct currentAct = Gamemanager.Instance.GetCurrentAct();

// Check which act the player is in
if (currentAct == StoryAct.Departure)
{
    // Do something specific to Act 1
}

// Trigger a transition programmatically
Gamemanager.Instance.SetAct(StoryAct.Initiation);
```

### Adding New Story Events

Instead of using keypresses, trigger transitions based on gameplay:

```csharp
// Example: Transition when player completes an objective
public void OnObjectiveComplete()
{
    StoryAct nextAct = Gamemanager.Instance.GetCurrentAct() + 1;
    Gamemanager.Instance.SetAct(nextAct);
}
```

### Modifying State Behaviors

Edit the Act_X_State.cs files to add your own logic:

```csharp
public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
{
    Debug.Log("Entering Act 2: Initiation");

    // Add your custom code here:
    // - Spawn enemies
    // - Change music
    // - Update UI
    // - Load new scene elements
}
```

### Implementing Start and End States

The **Start_State.cs** and **End_State.cs** files are currently templates with all methods commented out. To implement them:

1. **Open the file** (Start_State.cs or End_State.cs)
2. **Uncomment the methods** you need
3. **Add your logic** inside the methods

**Example - Implementing End_State.cs:**
```csharp
using UnityEngine;

public class End_State : StateMachineBehaviour
{
    // Called when entering the End state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Game Complete! Entered End State");
        // Show credits screen
        // Display final score
        // Enable "Play Again" button
        // Save completion status
    }

    // Called when exiting the End state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Restarting game from End State");
        // Clean up credits UI
        // Reset game state
    }
}
```

**Example - Implementing Start_State.cs:**
```csharp
using UnityEngine;

public class Start_State : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Welcome! Entered Start State");
        // Show main menu
        // Load player preferences
        // Initialize game systems
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Starting the journey...");
        // Hide main menu
        // Play intro cutscene
    }
}
```

## Key Concepts Summary

| Concept | Purpose | Example |
|---------|---------|---------|
| **Enum** | Named constants for readability | `StoryAct.Departure` instead of `1` |
| **Singleton** | One global instance accessible everywhere | `Gamemanager.Instance` |
| **State Machine** | Manages different game states | Start, Acts 1-3, End |
| **Validation Logic** | Enforces game rules | Can't skip or go backwards |
| **Animator Integration** | Connects code to Unity's visual system | `SetInteger("Act", value)` |
| **Switch Expression** | Compact pattern matching | `actToName switch { ... }` |

## Testing the System

1. **Play the game** in Unity
2. **Press keys 0-4** to test transitions
3. **Watch the Console** for debug messages
4. **Observe Animator** window to see state changes

**Expected behavior (full progression):**
- Initial state (or press "0") → Start state
- Start → Press "1" → Act 1: Departure
- Act 1 → Press "2" → Act 2: Initiation
- Act 2 → Press "3" → Act 3: Return
- Act 3 → Press "4" → End state
- Any invalid transition will show an error message

**Testing invalid transitions:**
- From Start, pressing "2" should fail (must go to Act 1 first)
- From Act 1, pressing "3" should fail (must go to Act 2 first)
- From Act 2, pressing "1" should fail (cannot go backwards)

## Extension Ideas

- Add a **loading screen** between acts
- **Save/load** the current act using PlayerPrefs
- Add **story text** that displays at each state transition
- Create **conditional branches** (e.g., different Act 2 paths based on player choices)
- Implement a **dialogue system** that changes based on current act
- Add **achievements** for completing each act

## Common Issues & Solutions

**Problem:** Transitions aren't working
- Check that Animator Controller has an integer parameter named "Act"
- Verify transitions are set up with correct conditions (Act = 1, Act = 2, etc.)

**Problem:** Multiple Gamemanagers exist
- Ensure only one GameObject has the Gamemanager script in your starting scene
- Check that DontDestroyOnLoad is working

**Problem:** Can skip acts or go backwards
- Check that you're using the `TryTransition()` method, not calling `SetAct()` directly
- Verify the validation logic in TryTransition is functioning

## Learning Resources

- **Enums:** https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/enum
- **Singleton Pattern:** https://www.unity3dtips.com/unity-singleton-pattern/
- **State Machines:** https://learn.unity.com/tutorial/5c5151b9edbc2a001fd5c696
- **Switch Expressions:** https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/switch-expression
