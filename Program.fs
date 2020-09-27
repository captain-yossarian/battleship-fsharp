open Types.GameTypes
open State.Constants
open State.Reducer

[<EntryPoint>]
let main argv =
    let action = buildShip (Point(2,3))
    let result = reducer action initialState
    printfn "hello %A" result
    0 // return an integer exit code
