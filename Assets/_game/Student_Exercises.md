# Three-Act Story - Student Exercises

These exercises will help you practice C# programming, Unity development, and game design patterns using the Three-Act Story system. Complete them in order, as later exercises build on earlier concepts.

---

## Easy Exercises (Beginner Level)

### Exercise 1: Implement the Start State Behavior
**Difficulty:** ⭐ Easy
**Estimated Time:** 10-15 minutes
**Learning Goals:** StateMachineBehaviour, Unity lifecycle methods, Debug logging

**Task:**
Uncomment and implement the methods in [Start_State.cs](Scripts/StoryActCode/Start_State.cs) to add functionality when the game begins.

**Requirements:**
- When entering the Start state, log "Welcome to the Three-Act Journey!"
- When exiting the Start state, log "The adventure begins..."
- Add any additional startup logic you think is appropriate

**Test:**
- Play the game and verify messages appear in Console at appropriate times
- Press "1" to transition from Start to Act 1 and verify exit message

---

### Exercise 2: Implement the End State Behavior
**Difficulty:** ⭐ Easy
**Estimated Time:** 10-15 minutes
**Learning Goals:** StateMachineBehaviour, Unity lifecycle methods

**Task:**
Uncomment and implement the methods in [End_State.cs](Scripts/StoryActCode/End_State.cs) to add functionality when the story completes.

**Requirements:**
- When entering the End state, log "Congratulations! Story Complete!"
- Add a message showing total time played (you can use `Time.time`)
- When exiting End state, log "Returning to menu..."

**Test:**
- Progress through all acts (0→1→2→3→4) and verify end messages
- Check that time is displayed correctly

---

### Exercise 3: Add a Current Act Display
**Difficulty:** ⭐ Easy
**Estimated Time:** 15-20 minutes
**Learning Goals:** UI Text, accessing singleton, string formatting

**Task:**
Create a UI Text element that displays the current story act on screen.

**Requirements:**
- Add a UI Canvas with a Text element in the top-left corner
- Create a new script called `ActDisplay.cs`
- Update the text every frame to show current act name (e.g., "Current Act: Act 1: Departure")
- Use `Gamemanager.Instance.GetCurrentAct()` and `GetActName()` methods

**Test:**
- Text should update immediately when acts change
- Text should be clearly visible and formatted nicely

**Bonus:**
- Add color coding (green for Start, yellow for acts, blue for End)
- Make the text fade in/out on transitions

---

### Exercise 4: Add Act Completion Counter
**Difficulty:** ⭐ Easy
**Estimated Time:** 15-20 minutes
**Learning Goals:** Variables, incrementing, logging

**Task:**
Track how many times each act has been entered during gameplay.

**Requirements:**
- Add a private integer variable in each Act state script (Act_1_State, Act_2_State, Act_3_State)
- Increment this counter in `OnStateEnter()`
- Log the count each time (e.g., "Entering Act 1: Departure (Visit #2)")

**Test:**
- Play through the story normally (should show Visit #1 for each)
- If you could reset and replay, it should increment (though current system doesn't support this)

---

## Moderate Exercises (Intermediate Level)

### Exercise 5: Add a Reset to Start Feature
**Difficulty:** ⭐⭐ Moderate
**Estimated Time:** 20-30 minutes
**Learning Goals:** Modifying existing code, conditional logic, enum values

**Task:**
Add a "Reset" feature that allows returning to the Start state from any act, bypassing the normal progression rules.

**Requirements:**
- Add a new key binding (R key) for Reset
- Create a new method `ResetToStart()` in Gamemanager
- This method should ALWAYS work, regardless of current act
- Log a special message: "Resetting story to beginning..."
- Update the act to Start (0)

**Test:**
- From any act, press R and verify return to Start state
- Normal progression rules should still apply for numbered keys
- Reset should work from End state

**Hint:**
You'll need to modify the `Update()` method and add the new Reset method. The Reset should bypass `TryTransition()` and call `SetAct()` directly.

---

### Exercise 6: Save and Load Story Progress
**Difficulty:** ⭐⭐ Moderate
**Estimated Time:** 30-40 minutes
**Learning Goals:** PlayerPrefs, data persistence, Unity lifecycle

**Task:**
Implement save/load functionality so the current act persists between game sessions.

**Requirements:**
- When changing acts, save the current act to PlayerPrefs with key "SavedAct"
- In `Awake()` or `Start()`, check if a saved act exists and load it
- Add a key binding (L key) to manually load the saved act
- Add a key binding (S key) to manually save current act
- Log messages when saving/loading

**Test:**
- Progress to Act 2, save, then close Unity Play mode
- Start Play mode again - should resume at Act 2
- Test that normal progression still works after loading

**Bonus:**
- Save a timestamp of when the game was last played
- Display "Welcome back! Last played: [time]" when loading

---

### Exercise 7: Add Act Transition Sound Effects
**Difficulty:** ⭐⭐ Moderate
**Estimated Time:** 25-35 minutes
**Learning Goals:** AudioSource, Resources.Load, playing audio

**Task:**
Play a sound effect when transitioning between acts.

**Requirements:**
- Add an AudioSource component to the Gamemanager GameObject
- Find or create 5 short sound effects (one for each state)
- Store them in a Resources folder
- In each state script's `OnStateEnter()`, play the appropriate sound
- Use `animator.gameObject.GetComponent<AudioSource>().PlayOneShot(clip)`

**Test:**
- Each act transition should play a distinct sound
- Sounds should not overlap or cut each other off
- Sounds should be appropriate length (1-3 seconds)

**Note:**
You can use free sound effects from freesound.org or Unity's Asset Store, or create simple beep sounds.

---

### Exercise 8: Create a Progress Bar UI
**Difficulty:** ⭐⭐ Moderate
**Estimated Time:** 30-40 minutes
**Learning Goals:** UI Image, fill amount, mathematical calculations

**Task:**
Create a visual progress bar that fills as you progress through the story.

**Requirements:**
- Add a UI Image (with Image Type: Filled, Fill Method: Horizontal)
- Create a script `ProgressBar.cs`
- Calculate progress as: `currentAct / totalActs` (0% at Start, 100% at End)
- Update the Image's `fillAmount` property based on progress
- Add background and foreground colors for the bar

**Test:**
- At Start: 0% filled
- At Act 1 (Departure): 25% filled
- At Act 2 (Initiation): 50% filled
- At Act 3 (Return): 75% filled
- At End: 100% filled

**Bonus:**
- Animate the fill smoothly using `Mathf.Lerp()`
- Add percentage text label (e.g., "75%")
- Change bar color based on progress (green→yellow→blue)

---

### Exercise 9: Add Act Duration Tracking
**Difficulty:** ⭐⭐ Moderate
**Estimated Time:** 30-40 minutes
**Learning Goals:** Time tracking, private variables, calculation

**Task:**
Track how long the player spends in each act and display it when exiting.

**Requirements:**
- Add a private float `enterTime` variable to each Act state script
- In `OnStateEnter()`, record the current time: `enterTime = Time.time`
- In `OnStateExit()`, calculate duration: `duration = Time.time - enterTime`
- Log: "Completed Act 1: Departure in 15.7 seconds"
- Format the time nicely (use `F1` or `F2` for decimal places)

**Test:**
- Stay in each act for different amounts of time
- Verify accurate time tracking in Console
- Times should be positive and reasonable

**Bonus:**
- Display the time on-screen instead of just logging
- Track cumulative total time for the entire story
- Create a "speedrun timer" that shows total elapsed time

---

## Challenging Exercises (Advanced Level)

### Exercise 10: Implement Branching Story Paths
**Difficulty:** ⭐⭐⭐ Challenging
**Estimated Time:** 60-90 minutes
**Learning Goals:** Enums, state management, complex logic, animator setup

**Task:**
Modify the system to support branching paths in Act 2, creating two different Act 2 options based on player choice.

**Requirements:**
- Add two new enum values: `Act2_PathA` and `Act2_PathB` (renumber existing values accordingly)
- Modify the Animator Controller to have two separate Act 2 states
- At the end of Act 1, present a choice (keyboard input: Q for Path A, E for Path B)
- Create two new state scripts: `Act_2A_State.cs` and `Act_2B_State.cs`
- Both paths should lead to Act 3 (Return)
- Update validation logic to handle branching paths
- Update `GetActName()` to include both paths

**Test:**
- From Act 1, press Q → should go to Act 2 Path A
- From Act 1, press E → should go to Act 2 Path B
- Both paths should allow progression to Act 3
- Cannot skip directly from Act 1 to Act 3

**Bonus:**
- Add UI buttons for choice instead of keyboard
- Display different story text for each path
- Track which path was chosen and reference it in Act 3

---

### Exercise 11: Create an Achievement System
**Difficulty:** ⭐⭐⭐ Challenging
**Estimated Time:** 60-90 minutes
**Learning Goals:** ScriptableObjects OR static class, lists, events, UI

**Task:**
Build an achievement system that unlocks achievements for completing acts and special challenges.

**Requirements:**
- Create an `Achievement` class with: name, description, isUnlocked status, unlock condition
- Create at least 5 achievements:
  - "First Steps" - Complete Act 1
  - "Midpoint Master" - Complete Act 2
  - "Journey's End" - Complete Act 3
  - "Speed Runner" - Complete Act 1 in under 30 seconds
  - "Story Complete" - Reach the End state
- Create an `AchievementManager` singleton to track achievements
- When achievements unlock, show a visual notification (UI panel or text)
- Save unlocked achievements using PlayerPrefs

**Test:**
- Achievements unlock at appropriate times
- Notifications display correctly
- Achievements persist between sessions
- Can view all achievements (unlocked and locked)

**Bonus:**
- Add a dedicated Achievement UI screen (press A to view)
- Include timestamps for when achievements were unlocked
- Add more creative achievements ("Explorer" - visit every state at least once)

---

### Exercise 12: Implement Required Collectibles System
**Difficulty:** ⭐⭐⭐ Challenging
**Estimated Time:** 90-120 minutes
**Learning Goals:** Game design, validation logic, UI, object interaction

**Task:**
Create a collectible item system where players must collect a certain number of items in each act before progressing to the next.

**Requirements:**
- Create a `Collectible` prefab (simple 3D object with a trigger collider)
- Spawn 3 collectibles in the scene for each act
- Create a `CollectibleManager` that tracks current count
- Modify `TryTransition()` to check if enough collectibles are collected before allowing progression
- Display current collectible count on UI: "Collectibles: 2/3"
- When all collectibles collected, show "Act Complete! Press [next number] to continue"
- Reset collectible count when entering a new act

**Test:**
- Cannot progress without collecting all items
- Collectible count displays correctly
- Collecting items works (OnTriggerEnter)
- Count resets when entering new act
- Can still progress normally after collecting all items

**Bonus:**
- Different collectible models/colors for each act
- Particle effects when collecting
- Sound effects for collection
- Mini-map showing collectible locations

---

### Exercise 13: Add Context-Aware Dialogue System
**Difficulty:** ⭐⭐⭐ Challenging
**Estimated Time:** 90-120 minutes
**Learning Goals:** Dictionary, UI, text parsing, game design

**Task:**
Create an NPC dialogue system where NPCs say different things based on the current story act.

**Requirements:**
- Create an `NPC` script that can be attached to GameObjects
- Create a dialogue dictionary mapping `StoryAct` enum to dialogue strings
- When player interacts with NPC (trigger or key press), show appropriate dialogue for current act
- Create a dialogue UI panel that displays NPC name and text
- Support at least one NPC with different dialogue for each of the 5 states
- Add a "Press E to Talk" prompt when near NPC

**Example Dialogue:**
```
Start: "Welcome, traveler! Your journey awaits."
Act 1: "Good luck on your departure!"
Act 2: "The trials of initiation are not easy."
Act 3: "Welcome back, hero!"
End: "You've completed your journey!"
```

**Test:**
- NPC says correct dialogue for each act
- Dialogue UI displays and hides correctly
- Can interact with multiple NPCs if created
- Dialogue updates when act changes

**Bonus:**
- Add a typing animation effect
- Support multiple dialogue lines (press Space to continue)
- Add character portraits
- Create a dialogue editor in the Inspector

---

### Exercise 14: Create a Story Text Display System
**Difficulty:** ⭐⭐⭐ Challenging
**Estimated Time:** 75-100 minutes
**Learning Goals:** UI animation, coroutines, text effects, game flow

**Task:**
When entering each act, display a large full-screen text overlay with the act title and a brief story description, then fade it out.

**Requirements:**
- Create a UI Canvas with full-screen Panel (semi-transparent black background)
- Add Text elements for title and description
- In each Act state's `OnStateEnter()`, trigger the display
- Animate the text: fade in (1 second) → hold (3 seconds) → fade out (1 second)
- Use a Coroutine for the animation timing
- Different story text for each act

**Example Story Texts:**
```
Start: "Your quiet life is about to change forever..."
Act 1: "You leave behind everything familiar, embarking on an unknown path."
Act 2: "Trials and challenges test your resolve. Will you rise to meet them?"
Act 3: "With newfound wisdom, you journey back to where it all began."
End: "Your story is complete, but the memories will last forever."
```

**Test:**
- Story text displays on entering each act
- Timing is correct (5 seconds total)
- Fade animation is smooth
- Text is readable and well-formatted
- Doesn't interfere with gameplay

**Bonus:**
- Add a "Press Space to Skip" option
- Include character images or background art
- Add sound effects or music changes
- Support multiple story text variations based on choices

---

## Extension Challenges (Open-Ended)

These challenges have no specific requirements. Be creative and demonstrate your learning!

### Challenge A: Visual State Representation
Create a visual representation of the story progression (e.g., a path with nodes, a book with chapters, a journey map). Update visuals when acts change.

### Challenge B: Multiple Story Templates
Create a system that supports different story structures (5-act, 7-act, circular journey). Make the Gamemanager flexible enough to handle different templates.

### Challenge C: Story Editor Tool
Create a custom Unity Editor window that lets designers create and edit story acts without modifying code.

### Challenge D: Multiplayer Story Sync
Research Unity Netcode/Multiplayer and synchronize story state across multiple players. When one player advances, all players advance together.

---

## Submission Guidelines

For each exercise you complete:
1. **Test thoroughly** - Make sure all requirements are met
2. **Comment your code** - Explain what each section does
3. **Take screenshots** - Show your implementation working
4. **Write a brief reflection** - What did you learn? What was challenging?

## Learning Resources

- **Unity Documentation:** https://docs.unity3d.com/
- **C# Programming Guide:** https://learn.microsoft.com/en-us/dotnet/csharp/
- **Unity Learn:** https://learn.unity.com/
- **State Machine Pattern:** https://gameprogrammingpatterns.com/state.html

## Getting Help

If you're stuck:
1. Re-read the relevant section in [Readme.md](Readme.md)
2. Check the [Exercise_Solutions.md](Exercise_Solutions.md) for hints
3. Use Debug.Log() to understand what's happening
4. Ask a classmate or instructor
5. Search Unity forums and documentation

---

**Good luck and have fun learning!** 🎮