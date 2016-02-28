﻿namespace Morgemil.Math

open Newtonsoft.Json
open System

/// <summary>
/// Use this for tile-bound positioning
/// </summary>
[<JsonObject(MemberSerialization.OptIn)>]
type Vector2i = 
  { [<JsonProperty>]
    X : int
    [<JsonProperty>]
    Y : int }
  
  static member From(scalar) = 
    { Vector2i.X = scalar
      Y = scalar }
  
  static member From(x, y) = 
    { Vector2i.X = x
      Y = y }
  
  //Zero
  static member Zero = Vector2i.From(0)
  //Identity
  static member Identity = Vector2i.From(0, 1)
  //########## Operator overloads #####################################
  //Addition
  static member (+) (vec1 : Vector2i, vec2 : Vector2i) = Vector2i.From(vec1.X + vec2.X, vec1.Y + vec2.Y)
  static member (+) (vec1 : Vector2i, scalar) = Vector2i.From(vec1.X + scalar, vec1.Y + scalar)
  static member (+) (scalar : int, vec1 : Vector2i) = vec1 + scalar
  //Subtraction
  static member (-) (vec1 : Vector2i, vec2 : Vector2i) = Vector2i.From(vec1.X - vec2.X, vec1.Y - vec2.Y)
  static member (-) (vec1 : Vector2i, scalar) = Vector2i.From(vec1.X - scalar, vec1.Y - scalar)
  static member (-) (scalar, vec1 : Vector2i) = Vector2i.From(scalar - vec1.X, scalar - vec1.Y)
  //Multiplication
  static member (*) (vec1 : Vector2i, vec2 : Vector2i) = Vector2i.From(vec1.X * vec2.X, vec1.Y * vec2.Y)
  static member (*) (vec1 : Vector2i, scalar) = Vector2i.From(vec1.X * scalar, vec1.Y * scalar)
  static member (*) (scalar : int, vec1 : Vector2i) = vec1 * scalar
  //Division (Does not guard against divide by zero)
  static member (/) (vec1 : Vector2i, vec2 : Vector2i) = Vector2i.From(vec1.X / vec2.X, vec1.Y / vec2.Y)
  static member (/) (vec1 : Vector2i, scalar) = Vector2i.From(vec1.X / scalar, vec1.Y / scalar)
  //########## Member methods #########################################
  //Distance
  member this.LengthSquared = float ((this.X * this.X) + (this.Y + this.Y))
  member this.Length = System.Math.Sqrt(this.LengthSquared)
  
  ///The area as though this were a rectangle size
  member this.Area = this.X * this.Y
  
  ///Minimum (x,y) of both elements
  member this.Minimum(vec1 : Vector2i) = Vector2i.From(Math.Min(this.X, vec1.X), Math.Min(this.Y, vec1.Y))
  
  ///Maximum (x,y) of both elements
  member this.Maximum(vec1 : Vector2i) = Vector2i.From(Math.Max(this.X, vec1.X), Math.Max(this.Y, vec1.Y))