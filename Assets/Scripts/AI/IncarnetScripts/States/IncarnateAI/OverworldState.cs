using System.Collections;

// Base class for all the states
public abstract class OverworldState
{
    protected private readonly OverworldStateSystem _system;
    public OverworldState(OverworldStateSystem system)
    {
        _system = system;
    }
    public virtual IEnumerator Start()
    {
        yield break;
    }
    public virtual IEnumerator Move()
    {
        yield break;
    }
    public virtual IEnumerator Attack()
    {
        yield break;
    }
    public virtual IEnumerator Heal()
    {
        yield break;
    }
    public virtual IEnumerator Pause()
    {
        yield break;
    }
    public virtual IEnumerator Resume()
    {
        yield break;
    }
}