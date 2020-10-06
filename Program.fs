open Types.GameTypes
open State.Constants
open Reducer.Reducers
open Utils.Board
open Utils.Debug

[<EntryPoint>]
let main argv =
    let ship = {Count=1; Size=3}
    let action = buildShips (ship)
    let state = reducer action initialState   


    let sndAction = buildShips {Count=1; Size=2}

    let {Board=board} = reducer sndAction state   
    let result = render board

    printfn "%A" result 
    0 // return an integer exit code
