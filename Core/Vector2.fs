﻿namespace Morgemil.Math

/// <summary>
/// Utility Vector2 functions that don't fit as a type
/// </summary>
module Vector2 = 
  //Vector2i to Vector2f
  let Vector2iTof(veci : Vector2i) = Vector2f(float veci.X, float veci.Y)
  //Vector2f to Vector2i
  let Vector2fToi(vecf : Vector2f) = Vector2i(int vecf.X, int vecf.Y)
