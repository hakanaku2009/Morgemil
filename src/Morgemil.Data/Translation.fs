module Morgemil.Data.Translation

open System
open Morgemil.Core
open Morgemil.Data.DTO
open Morgemil.Models
open Morgemil.Math

let private ZeroColorDto(): DTO.Color =
    {
        A = Byte.MinValue
        B = Byte.MinValue
        G = Byte.MinValue
        R = Byte.MinValue
    }

let ColorToDto (color: Color): DTO.Color =
    {
        A = color.A
        B = color.B
        G = color.G
        R = color.R
    }

///DTO to Color
let ColorFromDto (color: DTO.Color): Color =
    {
        A = color.A
        B = color.B
        G = color.G
        R = color.R
    }
    
///DTO to Vector2i
let Vector2iFromDto (vec: DTO.Vector2i): Vector2i =
    Vector2i.create(vec.X, vec.Y)
    
///DTO to Rectangle
let RectangleFromDto (rectangle: DTO.Rectangle): Rectangle =
    Rectangle.create(Vector2iFromDto rectangle.Position, Vector2iFromDto rectangle.Size)
    
///DTO to Color Option
let ColorOptionFromDto (color: DTO.Color): Color option =
    if color.A = Byte.MinValue then
        None
    else
        Some <|
        {
            A = color.A
            B = color.B
            G = color.G
            R = color.R
        }

///DTO to TileRepresentation
let TileRepresentationToDto (tileRepresentation: TileRepresentation): DTO.TileRepresentation =
    {
        AnsiCharacter = (int)(Char.GetNumericValue tileRepresentation.AnsiCharacter)
        ForegroundColor = tileRepresentation.ForegroundColor |> Option.map ColorToDto |> Option.defaultValue (ZeroColorDto())
        BackGroundColor = tileRepresentation.BackGroundColor |> Option.map ColorToDto |> Option.defaultValue (ZeroColorDto())
    }
    
///DTO to TileRepresentation
let TileRepresentationFromDto (tileRepresentation: DTO.TileRepresentation): TileRepresentation =
    {
        AnsiCharacter = (char)tileRepresentation.AnsiCharacter
        ForegroundColor = tileRepresentation.ForegroundColor |> ColorOptionFromDto
        BackGroundColor = tileRepresentation.BackGroundColor |> ColorOptionFromDto
    }
    
///DTO to Tile    
let TileFromDto (tile: DTO.Tile): Tile =
    let tileType  =
        match tile.TileType with
        | DTO.TileType.Void -> TileType.Void
        | DTO.TileType.Ground -> TileType.Ground
        | DTO.TileType.Solid -> TileType.Solid
        | _ -> TileType.Ground
    {
        ID = TileID tile.ID
        Name = tile.Name
        Description = tile.Description
        Representation = TileRepresentationFromDto tile.Representation
        BlocksSight = tile.BlocksSight
        BlocksMovement = tile.BlocksMovement
        TileType = tileType
    }
    
///DTO to Race
let RaceFromDto (getRaceModifierByID: RaceModifierID -> RaceModifier) (race: DTO.Race) : Race =
    {
        Race.ID = RaceID race.ID
        Noun = race.Noun
        Adjective = race.Adjective
        Description = race.Description
        PossibleRaceModifiers = race.PossibleRaceModifiers |> List.map (RaceModifierID >> getRaceModifierByID)
    }

///DTO to RaceModifier    
let RaceModifierFromDto (raceModifier: DTO.RaceModifier) : RaceModifier =
    {
        RaceModifier.ID = RaceModifierID raceModifier.ID
        Noun = raceModifier.Noun
        Adjective = raceModifier.Adjective
        Description = raceModifier.Description
    }
    
///DTO to Item    
let ItemFromDto (item: DTO.Item) : Item =
    {
        Item.ID = ItemID item.ID
        Noun = item.Noun
        ItemType =
            match item.ItemType with
            | DTO.ItemType.Weapon -> ItemType.Weapon
            | DTO.ItemType.Wearable -> ItemType.Wearable
            | DTO.ItemType.Consumable | _ -> ItemType.Consumable
        IsUnique = item.IsUnique
        SubItem =
            match item.ItemType with
            | DTO.ItemType.Weapon ->
                {
                    Weapon.Weight = item.Weapon.Head.Weight * 1M<Weight>
                    Weapon.BaseRange = item.Weapon.Head.BaseRange * 1<TileDistance>
                    Weapon.HandCount = item.Weapon.Head.HandCount * 1<HandSlot>
                    Weapon.RangeType =
                        match item.Weapon.Head.RangeType with
                        | DTO.WeaponRangeType.Melee -> WeaponRangeType.Melee
                        | DTO.WeaponRangeType.Ranged | _ -> WeaponRangeType.Ranged
                } |> SubItem.Weapon
            | DTO.ItemType.Wearable ->
                {
                    Wearable.Weight = item.Wearable.Head.Weight * 1M<Weight>
                    Wearable.WearableType =
                        match item.Wearable.Head.WearableType with
                        | DTO.WearableType.Head -> WearableType.Head
                        | DTO.WearableType.Feet -> WearableType.Feet
                        | DTO.WearableType.Hand -> WearableType.Hand
                        | DTO.WearableType.Chest -> WearableType.Chest
                        | DTO.WearableType.Legs -> WearableType.Legs
                        | DTO.WearableType.Neck -> WearableType.Neck
                        | DTO.WearableType.Cloak -> WearableType.Cloak
                        | DTO.WearableType.Waist -> WearableType.Waist
                        | DTO.WearableType.Shield -> WearableType.Shield
                        | DTO.WearableType.Fingers | _ -> WearableType.Fingers
                } |> SubItem.Wearable
            | DTO.ItemType.Consumable | _ ->
                {
                    Consumable.Uses = item.Consumable.Head.Uses
                } |> SubItem.Consumable        
    }

///DTO to MonsterGenerationParameter
let MonsterGenerationParameterFromDto (monsterGenerationParameter: DTO.MonsterGenerationParameter) : MonsterGenerationParameter =
    {
        MonsterGenerationParameter.ID = MonsterGenerationParameterID monsterGenerationParameter.ID
        GenerationRatios =
            monsterGenerationParameter.GenerationRatios
            |> List.map(fun t -> {
                RaceModifierLink.Ratio = t.Ratio
                RaceModifierLink.RaceID = RaceID t.RaceID
                RaceModifierLink.RaceModifierID = match t.RaceModifierID.HasValue with | true -> t.RaceModifierID.Value |> RaceModifierID |> Some | false -> None
            })
    }

///DTO to FloorGenerationParameter
let FloorGenerationParameterFromDto (getTilebyID: TileID -> Tile) (floorGenerationParameter: DTO.FloorGenerationParameter) : FloorGenerationParameter =
    {
        FloorGenerationParameter.ID = FloorGenerationParameterID floorGenerationParameter.ID
        DefaultTile = floorGenerationParameter.DefaultTile |> TileID |> getTilebyID
        Tiles = floorGenerationParameter.Tiles |> Seq.map (TileID >> getTilebyID) |> Seq.toArray
        SizeRange = floorGenerationParameter.SizeRange |> RectangleFromDto
        Strategy =
            match floorGenerationParameter.Strategy with
            | DTO.FloorGenerationStrategy.OpenFloor | _ -> FloorGenerationStrategy.OpenFloor
    }    
    
///DTO to Phase2
let TranslateFromDtosToPhase2 (dtos: RawDtoPhase0): RawDtoPhase2 =
    let tiles = dtos.Tiles.Item |> Seq.map (TileFromDto) |> Table.CreateReadonlyTable (fun (t: TileID) -> t.Key)
    let raceModifiers = dtos.RaceModifiers.Item |> Seq.map (RaceModifierFromDto) |> Table.CreateReadonlyTable (fun (t: RaceModifierID) -> t.Key)
    let races = dtos.Races.Item |> Seq.map (RaceFromDto (fun t -> raceModifiers.Item(t))) |> Table.CreateReadonlyTable (fun (t: RaceID) -> t.Key)
    let items = dtos.Items.Item |> Seq.map (ItemFromDto) |> Table.CreateReadonlyTable (fun (t: ItemID) -> t.Key)
    let monsterGenerationParameters = dtos.MonsterGenerationParameters.Item |> Seq.map (MonsterGenerationParameterFromDto) |> Table.CreateReadonlyTable (fun (t: MonsterGenerationParameterID) -> t.Key)
    
    {
        RawDtoPhase2.Tiles = tiles.Items |> Seq.toArray
        RaceModifiers = raceModifiers.Items |> Seq.toArray
        Races = races.Items |> Seq.toArray
        Items = items.Items |> Seq.toArray
        MonsterGenerationParameters = monsterGenerationParameters.Items |> Seq.toArray
    }

///DTO to Scenario
let TranslateFromDtosToScenario (dtos: RawDtoPhase0): ScenarioData =
    let tiles = dtos.Tiles.Item |> Seq.map (TileFromDto) |> Table.CreateReadonlyTable (fun (t: TileID) -> t.Key)
    let raceModifiers = dtos.RaceModifiers.Item |> Seq.map (RaceModifierFromDto) |> Table.CreateReadonlyTable (fun (t: RaceModifierID) -> t.Key)
    let races = dtos.Races.Item |> Seq.map (RaceFromDto (fun t -> raceModifiers.Item(t))) |> Table.CreateReadonlyTable (fun (t: RaceID) -> t.Key)
    let items = dtos.Items.Item |> Seq.map (ItemFromDto) |> Table.CreateReadonlyTable (fun (t: ItemID) -> t.Key)
    let monsterGenerationParameters = dtos.MonsterGenerationParameters.Item |> Seq.map (MonsterGenerationParameterFromDto) |> Table.CreateReadonlyTable (fun (t: MonsterGenerationParameterID) -> t.Key)
    
    {
        ScenarioData.Tiles = tiles
        RaceModifiers = raceModifiers
        Races = races
        Items = items
        MonsterGenerationParameters = monsterGenerationParameters
    }
    