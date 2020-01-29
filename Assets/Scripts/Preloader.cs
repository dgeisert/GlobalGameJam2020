using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Preloader : MonoBehaviour
{
    public TMPro.TextMeshPro TipText;
    public GameObject AudioInitObject;
    private uint completed = 0;

    private List<String> tips = new List<string>()
    {
        "Larger swings do more damage. Execute a large swing by moving your entire body, not just the tip of the blade.",
        "Some enemies throw smoke bombs that must be avoided rather than blocked or deflected.",
        "Stabbing with your katanas does more damage, but is harder to execute successfully.",
        "Your shinobi skills have three tiers. By gathering more shinobi focus, your abilities become stronger when activated.",
        "Claws are great at defense and fast counterattacks.",
        "The staff won't cut enemies in half, but it will send them flying!",
        "Blocking or hitting an enemy hard can force most enemies to back off.",
        "Use the slow motion earned on kills and blocks to plan out how to deal with enemies surrounding you.",
        "Stay safe in the real world! Don't chase after a retreating enemy!",
        "Wear appropriate clothing for vigorous exercise. No need for stealth.",
        "Ninja yells (kiai) will help you strike harder!",
        "Slice projectiles to send them back at the enemy that threw them.",
    };

    public void Start()
    {
        var initializer = InitPipeline.Create(configureDependencies, onTaskComplete);
        initializer.Run();

        string tip = tips[UnityEngine.Random.Range(0, tips.Count)];
        TipText.text = "Tip: " + tip;
    }
    

    private void configureDependencies(DependencyGraph<IInitTask> dg)
    {
        //var loadGameConfig = new TaskLoadGameConfig();
        var waitTask = new TaskWait(3f);
        dg.AddItem(waitTask);
        //dg.AddItem(loadGameConfig).DependsOn(waitTask);
        
    }

    private void onTaskComplete(string taskName)
    {
        completed++;
        if (completed == 2)
        {
            SceneHandler.LoadSceneByName("level_00_empty", null, false, true);
        }
        
    }
}