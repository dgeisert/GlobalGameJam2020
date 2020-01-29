using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InitPipeline
{
    public static InitPipeline Create(Action<DependencyGraph<IInitTask>> configure, Action<string> onTaskComplete = null)
    {
        var graph = new DependencyGraph<IInitTask>();
        configure(graph);

        var runner = AsyncRunner.GetRunner("InitRunner");
        return new InitPipeline(graph, runner, onTaskComplete);
    }

    private DependencyGraph<IInitTask> dg;
    private AsyncRunner runner;
    private Action<string> onTaskComplete;
    private List<IInitTask> completed = new List<IInitTask>();
    private bool started = false;

    public InitPipeline(DependencyGraph<IInitTask> dg, AsyncRunner runner, Action<string> onTaskComplete = null)
    {
        this.dg = dg;
        this.runner = runner;
        this.onTaskComplete = onTaskComplete;
    }

    public void Run()
    {
        if (started)
        {
            throw new InvalidOperationException("Init already running.");
        }
        else
        {
            started = true;
            RunTasks(dg.Roots);
        }
    }

    private void RunTasks(List<IInitTask> tasks)
    {
        foreach (var task in tasks)
        {
            if (IsReadyToExecute(task))
            {
                runner.Run(RunTask(task));
            }
        }
    }

    private IEnumerator RunTask(IInitTask task)
    {
        yield return runner.Run(task.Execute());

        completed.Add(task);
        if (onTaskComplete != null)
        {
            onTaskComplete(task.GetName());
        }

        var nextTasks = dg.GetDependents(task);
        RunTasks(nextTasks);
    }

    private bool IsReadyToExecute(IInitTask task)
    {
        return dg.GetDependencies(task).All(dep => completed.Contains(dep));
    }
}
