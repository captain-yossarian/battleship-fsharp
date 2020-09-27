
module Types =
    type Ships =
        | Carrier of sbyte * sbyte //q: 1, s: 4
        | Cruiser of sbyte * sbyte //q: 2, s: 3
        | Submarine of sbyte * sbyte //q: 3, s: 2
        | Destroyer of sbyte * sbyte //q: 4, s: 1

    type Direction =
        | Vertical
        | Horizontal

    type Cell =
        | Empty = 1
        | Float = 2
        | Sinking = 3

    type Point = (sbyte * sbyte)

    type Board = int [] []
