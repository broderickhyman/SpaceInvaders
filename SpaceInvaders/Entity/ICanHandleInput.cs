using MonoGame.Extended.Input.InputListeners;

namespace SpaceInvaders.Entity;
internal interface ICanHandleInput
{
  void KeyReleased(object sender, KeyboardEventArgs e);
}
