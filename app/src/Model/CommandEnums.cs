namespace VimReimagination.Model;
internal static class Commands
{
  const byte CategoryPageStart = 8;
  internal enum Category : short
  {
    None,
    Motion,
    Normal,
    MotionPageStart = Motion << CategoryPageStart,
    NormalPageStart = Normal << CategoryPageStart,
  }
  internal enum All : short
  {
    None,
    Row_Pattern_BigWordStart_Back = Category.MotionPageStart,
    Row_Pattern_BigWordEnd_Back,
    Row_Pattern_BigWordStart_Forth,
    Row_Pattern_BigWordEnd_Forth,
    Row_Pattern_SmallWordStart_Back,
    Row_Pattern_SmallWordEnd_Back,
    Row_Pattern_SmallWordStart_Forth,
    Row_Pattern_SmallWordEnd_Forth,
    Row_1unit_Back,
    Col_1unit_Forth,
    Col_1unit_Back,
    Row_1unit_Forth,
    Row_FullScreen_Back_StopOnEdge,
    Col_FullScreen_Forth_StopOnEdge,
    Col_FullScreen_Back_StopOnEdge,
    Row_FullScreen_Forth_StopOnEdge,
    SmallDelete = Category.NormalPageStart,
    SaveFile,
  }
  internal static All[] ListOfMotionCommands = Enum.GetValues<All>().Where(static q => q.HasFlag((All)Category.MotionPageStart)).ToArray();
  internal static All[] ListOfNormalCommands = Enum.GetValues<All>().Where(static q => q.HasFlag((All)Category.NormalPageStart)).ToArray();
  internal static bool IsMotionCommand(this All command) => command.HasFlag((All)Category.MotionPageStart);
  internal static bool IsNormalCommand(this All command) => command.HasFlag((All)Category.NormalPageStart);
}