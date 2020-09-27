#load "Types.fsx"

module Constants =
    open Types

    let ships =
        [| Types.Carrier(1y, 4y)
           Types.Cruiser(2y, 3y)
           Types.Submarine(3y, 2y)
           Types.Destroyer(4y, 1y) |]

    let board = Array.create 10 (Array.create 10 0)
