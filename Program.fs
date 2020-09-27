open Types.GameTypes
open State.Constants
open State.Reducer

[<EntryPoint>]
let main argv =
    let action = buildShip Carrier { Count = 1y; Size = 4y }
    let result = reducer action initialState
    printfn "hello %A" result
    0 // return an integer exit code
