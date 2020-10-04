open Types.GameTypes
open State.Constants
open Reducer.Reducers
open Utils.Board
open Utils.Debug

[<EntryPoint>]
let main argv =
    let action = buildShips ()
    let result = reducer action initialState
    let ship = {Count=1; Size=1}
    let r = render (drawShip initialState ship)

    printfn "%A" r
    0 // return an integer exit code
