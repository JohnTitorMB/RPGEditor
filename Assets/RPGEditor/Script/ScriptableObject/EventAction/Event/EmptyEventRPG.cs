[System.Serializable]
public class EmptyEventRPG : EventRPG
{
    public EmptyEventRPG()
    {
    }

    public override void Update()
    {
        if (action != null)
            action.Update();

        if (nextEvents != null)
            nextEvents.Update();
    }
}