namespace VimReimagination.Model;

internal enum MotionCommand
{
  None,
  Row_Pattern_BigWordStart_Back,
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
}
