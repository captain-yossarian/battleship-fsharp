open Types.GameTypes
open State.Constants
open Reducer.Reducers
open Utils.Board
open Utils.Debug

[<EntryPoint>]
let main argv = 
    let ships =
         [|  { Count = 1; Size = 4 } 
             { Count = 2; Size = 3 } 
             { Count = 3; Size = 2 } 
             { Count = 4; Size = 1 } |]   

    let dispatchShip state ship =
        let {Count=count} = ship
        let arr = Array.create count 0
        arr |> Array.fold (fun acc _->reducer (buildShip ship) acc) state       
      

    let buildAllShips = ships |> Array.fold dispatchShip initialState 
    let {Board=board} = buildAllShips
    let result = render board    

    printfn "%A" result
    printfn "%A" (success board)
    0 // return an integer exit code
