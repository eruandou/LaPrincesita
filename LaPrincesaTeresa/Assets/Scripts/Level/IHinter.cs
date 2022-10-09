namespace Level
{
    public interface IHinter
    {
        int EventIDProduced { get; }
        HintEvent HintEventRaiser { get; }
    }
}