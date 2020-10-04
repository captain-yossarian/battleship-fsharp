open Types.GameTypes
open State.Constants
open Reducer.Reducers
open Utils.Board

[<EntryPoint>]
let main argv =
    let action = buildShips ()
    let result = reducer action initialState
    printfn "hello %A" 2
    0 // return an integer exit code
