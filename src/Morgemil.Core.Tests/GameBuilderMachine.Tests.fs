module Morgemil.Core.Tests.GameBuilderMachine

open Xunit
open Morgemil.Core
open Morgemil.Math
open Morgemil.Models


let defaultTile: Tile = {
        ID = TileID 3L
        Name = "Dungeon Wall"
        TileType = TileType.Solid
        Description = "Dungeon floors are rock, paved cobblestone, and very slipper when bloody."
        BlocksMovement = true
        BlocksSight = true
        Representation = {
            AnsiCharacter = '#'
            ForegroundColor = Some <| Color.From(200, 200, 200)
            BackGroundColor = Some <| Color.From(0, 0, 0)
        }
    }

let tile2 = {
    defaultTile with
        Name = "Dungeon Floor"
        BlocksMovement = false
        BlocksSight = false
        ID = TileID 4L
}

let floorParameters: FloorGenerationParameter = {
    Strategy = FloorGenerationStrategy.OpenFloor
    Tiles = [
        defaultTile
        tile2
    ]
    SizeRange = Rectangle.create(10, 10, 15, 15)
    DefaultTile = defaultTile
    ID = FloorGenerationParameterID 5L
}

let stairTileFeature: TileFeature = {
        ID = TileFeatureID 2L
        Name = "Stairs down"
        Description = "Stairs down"
        BlocksMovement = false
        BlocksSight = false
        Representation = {
            AnsiCharacter = char(242)
            ForegroundColor = Some <| Color.From(30, 30, 255)
            BackGroundColor = Some <| Color.From(0, 240, 0, 50)
        }
        PossibleTiles = [
            tile2
        ]
        ExitPoint = true
        EntryPoint = false
    }

let startingPointFeature: TileFeature = {
        ID = TileFeatureID 1L
        Name = "Starting point"
        Description = "Starting point"
        BlocksMovement = false
        BlocksSight = false
        Representation = {
            AnsiCharacter = '@'
            ForegroundColor = Some <| Color.From(0)
            BackGroundColor = None
        }
        PossibleTiles = [
            tile2
        ]
        ExitPoint = false
        EntryPoint = true
    }

let race1: Race = {
    Race.Adjective = "Adjective"
    Race.Description = "Description"
    Race.ID = RaceID 1L
    Race.Noun = "Noun"
}

[<Fact>]
let ``Can transition states``() =
    let scenarioData = {
        ScenarioData.Items = Table.CreateReadonlyTable (fun (ItemID id) -> id) []
        ScenarioData.Races = Table.CreateReadonlyTable (fun (RaceID id) -> id) [
            race1
        ]
        ScenarioData.Tiles = Table.CreateReadonlyTable (fun (TileID id) -> id) [
            defaultTile
            tile2
        ]
        ScenarioData.TileFeatures = Table.CreateReadonlyTable (fun (TileFeatureID id) -> id) [
            startingPointFeature
            stairTileFeature
        ]
        ScenarioData.RaceModifiers = Table.CreateReadonlyTable (fun (RaceModifierID id) -> id) []
        ScenarioData.FloorGenerationParameters = Table.CreateReadonlyTable (fun (FloorGenerationParameterID id) -> id) [
            floorParameters
        ]
        ScenarioData.MonsterGenerationParameters = Table.CreateReadonlyTable (fun (MonsterGenerationParameterID id) -> id) []
    }
    
    let loadScenarioData (callback: ScenarioData -> unit) =
        callback scenarioData
    
    let machine: IGameBuilder = SimpleGameBuilderMachine(loadScenarioData) :> IGameBuilder
    Assert.Equal(GameBuilderStateType.SelectScenario, machine.CurrentState.GameBuilderStateType)
    
    match machine.CurrentState with
    | GameBuilderState.SelectScenario (scenarioList, scenarioCallback) ->
        scenarioCallback (scenarioList.Head)
    | _ -> Assert.False(true)           

    while machine.CurrentState.GameBuilderStateType = GameBuilderStateType.LoadedScenarioData do
        System.Threading.Thread.Sleep 200
    Assert.Equal(GameBuilderStateType.WaitingForCurrentPlayer, machine.CurrentState.GameBuilderStateType)
    
    match machine.CurrentState with
    | GameBuilderState.WaitingForCurrentPlayer (addPlayer) ->
        addPlayer (RaceID 1L)
    | _ -> Assert.False(true)
    
    Assert.Equal(GameBuilderStateType.LoadingGameProgress, machine.CurrentState.GameBuilderStateType)
    
    while machine.CurrentState.GameBuilderStateType = GameBuilderStateType.LoadingGameProgress do
        System.Threading.Thread.Sleep 200
    Assert.Equal(GameBuilderStateType.GameBuilt, machine.CurrentState.GameBuilderStateType)
    
    match machine.CurrentState with
    | GameBuilderState.GameBuilt (gameState, initialGameData) ->
        Assert.Equal(1, initialGameData.Characters.Length)
        Assert.Equal(PlayerID 1L, initialGameData.CurrentPlayerID)
    | _ -> Assert.False(true)    
    ()
    
    