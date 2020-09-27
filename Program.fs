open Types.GameTypes
open Utils.Board
open Constants.Constants

[<EntryPoint>]
let main argv =
    printfn " %A" (drawCell board (3, 7))
    printfn "hello"
    0 // return an integer exit code
