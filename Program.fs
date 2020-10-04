open Types.GameTypes
open State.Constants
open Reducer.Reducers
open Utils.Board
open Utils.Debug

[<EntryPoint>]
let main argv =
    let ship = {Count=1; Size=3}
    let action = buildShips (ship)
    let {Board=board} = reducer action initialState   
    let result = render board

    printfn "%A" result
    0 // return an integer exit code
