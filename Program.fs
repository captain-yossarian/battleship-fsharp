open Types.GameTypes
open State.Constants
open Reducer.Reducers
open Utils.Board
open Utils.Debug

[<EntryPoint>]
let main argv = 
    let ships =
         [| { Count = 1; Size = 4 }
            { Count = 2; Size = 3 }
            { Count = 3; Size = 2 }
            { Count = 4; Size = 1 } |]   

    let dispatchShip state ship =
        reducer (buildShip ship) state

    let buildAllShips = ships |> Array.fold dispatchShip initialState 
    let {Board=board} = buildAllShips
    let result = render board

    printfn "%A" result
    0 // return an integer exit code
