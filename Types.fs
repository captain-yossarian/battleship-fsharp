namespace Types

module GameTypes =
    type Ship = { Count: sbyte; Size: sbyte }

    type Ships =
        | Carrier of Ship
        | Cruiser of Ship
        | Submarine of Ship
        | Destroyer of Ship


    type Direction =
        | Vertical
        | Horizontal

    type Cell =
        | Empty = 1
        | Float = 2
        | Sinking = 3

    type Point = (sbyte * sbyte)

    type Board = int [] []
