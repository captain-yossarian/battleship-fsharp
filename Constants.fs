namespace Constants

module Constants =
    open Types.GameTypes

    let ships =
        [| Carrier { Count = 1y; Size = 4y }
           Cruiser { Count = 2y; Size = 3y }
           Submarine { Count = 3y; Size = 2y }
           Destroyer { Count = 4y; Size = 1y } |]

    let board = Array.create 10 (Array.create 10 0)
