﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
  internal interface IEntity : IEntityGroup
  {
    bool Disposing { get; }
    ref RectangleF Rectangle { get; }
  }
}
