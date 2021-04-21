using System.Collections;

// tmp state
public class BeginState : OverworldState
{
    public BeginState(OverworldStateSystem system) : base(system)
    {
    }
    public override IEnumerator Start()
    {
        //do xyz
        yield break;
    }
}