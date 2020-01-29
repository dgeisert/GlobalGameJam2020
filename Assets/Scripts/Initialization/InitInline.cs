using System;
using System.Collections;

public class InitInline: IInitTask
{
    public static IInitTask Action(string name, Action action)
    {
        return new InitInline(name, ActionToIEnumerator(action));
    }

    private static IEnumerator ActionToIEnumerator(Action action)
    {
        action();
        yield break;
    }

    private string name;
    private IEnumerator func;

    public InitInline(string name, IEnumerator func)
    {
        this.name = name;
        this.func = func;
    }

    public string GetName()
    {
        return this.name;
    }

    public IEnumerator Execute()
    {
        return this.func;
    }
}
