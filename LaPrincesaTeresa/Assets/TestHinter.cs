using Level;
using UnityEngine;

public class TestHinter : MonoBehaviour, IHinter

{
    [field: SerializeField] public int EventIDProduced { get; private set; }
    [field: SerializeField] public HintEvent HintEventRaiser { get; private set; }

#if UNITY_EDITOR
    [ContextMenu("Test hint Start")]
    public void TestHintStart()
    {
        HintEventRaiser.Raise(new HintEventParam(EventIDProduced, HintEventCommands.StartHint));
    }

    [ContextMenu("Test hint End")]
    public void TestHintEnd()
    {
        HintEventRaiser.Raise(new HintEventParam(EventIDProduced, HintEventCommands.CompletedHint));
    }
#endif
}