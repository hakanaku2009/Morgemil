namespace Morgemil.Core

open Morgemil.Models

type Loop(characters: CharacterTable, tileMap: TileMap, scenarioData: ScenarioData) =

    member this.ProcessRequest(event: ActionRequest): Step list =
        use builder = new EventHistoryBuilder(characters)

        builder {
            match event with
            | ActionRequest.Move actionRequestMove ->
                match actionRequestMove.CharacterID |> Table.TryGetRowByKey characters with
                | None -> ()
                | Some moveCharacter ->
                    let oldPosition = moveCharacter.Position
                    let newPosition = oldPosition + actionRequestMove.Direction
                    let blocksMovement = tileMap.Item(newPosition) |> TileMap.blocksMovement
                    if blocksMovement then
                        yield
                            {
                                CharacterID = moveCharacter.ID
                                OldPosition = oldPosition
                                RequestedPosition = newPosition
                            }
                            |> ActionEvent.RefusedMove
                    else
                        Table.AddRow characters {
                            moveCharacter with
                                Position = newPosition
                        }
                        yield
                            {
                                CharacterID = moveCharacter.ID
                                OldPosition = oldPosition
                                NewPosition = newPosition
                            }
                            |> ActionEvent.AfterMove
            | ActionRequest.GoToNextLevel (characterID) ->
                match characterID |> Table.TryGetRowByKey characters with
                | None -> ()
                | Some moveCharacter ->
                    if tileMap.[moveCharacter.Position] |> TileMap.isExitPoint then
                        let items = characters
                                    |> Table.Items
                                    |> Seq.toArray
                        items
                        |> Seq.map(fun t -> {
                                t with
                                    Position = tileMap.EntryPoints |> Seq.head
                            })
                        |> Seq.iter (Table.AddRow characters)

                        yield
                            {
                                Characters =
                                    characters
                                    |> Table.Items
                                    |> Seq.map(fun t -> {
                                            t with
                                                Position = tileMap.EntryPoints |> Seq.head
                                        })
                                    |> Array.ofSeq
                                TileMapData = tileMap.TileMapData
                            }
                            |> ActionEvent.MapChange

            yield ActionEvent.Empty 0
        }
