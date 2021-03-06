namespace Morgemil.Models

open System


type [<AttributeUsage(AttributeTargets.Class)>] RecordAttribute() =
    inherit Attribute()

type [<AttributeUsage(AttributeTargets.Property)>] RecordIdAttribute() =
    inherit Attribute()

type [<AttributeUsage(AttributeTargets.Property)>] MeasureByAttribute(name: string) =
    inherit Attribute()
    member this.Name = name

type [<Struct>] TileFeatureID =
    | TileFeatureID of int64

    member this.Key =
        let (TileFeatureID rowID) = this
        rowID

type [<Struct>] RaceID =
    | RaceID of int64

    member this.Key =
        let (RaceID rowID) = this
        rowID

type [<Struct>] RaceModifierID =
    | RaceModifierID of int64

    member this.Key =
        let (RaceModifierID rowID) = this
        rowID

type [<Struct>] TileID =
    | TileID of int64

    member this.Key =
        let (TileID rowID) = this
        rowID


type [<Struct>] CharacterID =
    | CharacterID of int64

    member this.Key =
        let (CharacterID characterID) = this
        characterID

type [<Struct>] ItemID =
    | ItemID of int64

    member this.Key =
        let (ItemID rowID) = this
        rowID

type [<Struct>] FloorGenerationParameterID =
    | FloorGenerationParameterID of int64

    member this.Key =
        let (FloorGenerationParameterID rowID) = this
        rowID

type [<Struct>] MonsterGenerationParameterID =
    | MonsterGenerationParameterID of int64

    member this.Key =
        let (MonsterGenerationParameterID rowID) = this
        rowID

type [<Struct>] PlayerID =
    | PlayerID of int64

    member this.Key =
        let (PlayerID rowID) = this
        rowID

[<Measure>]
type TileDistance

[<Measure>]
type Weight

[<Measure>]
type HandSlot
