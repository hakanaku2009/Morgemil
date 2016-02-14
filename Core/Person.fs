﻿namespace Morgemil.Core

///This is a high level view of an entity. Typically holds any mutable data (can change each game step).
type Person = 
  { Id : int
    Race : Race
    Position : Vector2i }
  member this.Area = Rectangle(this.Position, this.Race.Size)