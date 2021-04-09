using System.Collections;

public abstract class GMState
{
    protected private readonly GMStateSystem _system;
    public GMState(GMStateSystem system)
    {
        _system = system;
    }
    public virtual IEnumerator Start()
    {
        yield break;
    }
    public virtual IEnumerator Update()
    {
        yield break;
    }
}
