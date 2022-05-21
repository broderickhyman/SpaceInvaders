namespace SpaceInvaders.Entity;

internal interface IEntity : IEntityGroup
{
  bool Disposing { get; }
  ref RectangleF Rectangle { get; }
}
