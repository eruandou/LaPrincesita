namespace Level
{
    public struct HintEventParam
    {
        public int eventID;
        public HintEventCommands hintEventCommand;

        public HintEventParam(int id, HintEventCommands command)
        {
            eventID = id;
            hintEventCommand = command;
        }
    }
}