module Morgemil.Data.Parser
open FSharp.Data
open Morgemil.Data
open Morgemil.Data
open Morgemil.Data
open Morgemil.Math

let tryParseColor (propertyName: string) (value: JsonValue): Result<Color, string> =
    match value with
    | JsonValue.Array data ->
        match data.Length with
        | 3 ->
            Color.From(
                          data.[0].AsInteger(),
                          data.[1].AsInteger(),
                          data.[2].AsInteger()
                      )
            |> Result.Ok
        | 4 ->
            Color.From(
                          data.[0].AsInteger(),
                          data.[1].AsInteger(),
                          data.[2].AsInteger(),
                          data.[3].AsInteger()
                      )
            |> Result.Ok
        | _ -> Result.Error "Expected propert %s to have 3 or 4 numbers for color array"
    | JsonValue.Record data ->
        let alpha = value |> Helper.tryParseIntProperty "a" |> Helper.resultWithDefault (int System.Byte.MaxValue)
        let red = value |> Helper.tryParseIntProperty "r" |> Helper.resultWithDefault (int System.Byte.MinValue)
        let green = value |> Helper.tryParseIntProperty "g" |> Helper.resultWithDefault (int System.Byte.MinValue)
        let blue = value |> Helper.tryParseIntProperty "b" |> Helper.resultWithDefault (int System.Byte.MinValue)
        Color.From(red, green, blue, alpha)
        |> Result.Ok
    | _  ->
        Helper.expectedProperty propertyName None
        
