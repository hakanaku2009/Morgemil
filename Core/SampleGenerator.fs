﻿namespace Morgemil.Map.Samples

module GrassSample = 
  /// <summary>
  /// Returns grass everywhere except the world borders which are TileDefinition.Default
  /// </summary>
  let GrassGenerator(world : Morgemil.Map.OverWorld, area : Morgemil.Math.Rectangle) = 
    let grass = 
      Morgemil.Map.TileDefinition
        (1, "Grassland", "Lush grassland hosts vast herds of bovine monsters", false, true)
    
    let generate (pos : Morgemil.Math.Vector2i) = 
      if pos.X <= 0 || pos.Y <= 0 || pos.X = world.Area.Width || pos.Y = world.Area.Height then 
        Morgemil.Map.TileDefinition.Default
      else grass
    Morgemil.Map.Chunk(area, 
                       area.Coordinates
                       |> Seq.map generate
                       |> Seq.toArray)