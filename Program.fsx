#load "Utils.fsx"
#load "Types.fsx"
#load "Constants.fsx"

open Utils
open Types
open Constants

let main =
    printfn " %A" (Utils.drawCell Constants.board (3, 7))
    0 // return an integer exit code
