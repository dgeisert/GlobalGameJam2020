using System.Collections;

public interface IInitTask
{
    string GetName();
    IEnumerator Execute();
}
