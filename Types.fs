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
        | Initial = 0
        | Float = 1
        | Sinking = 2
        | Empty = 3

    type Point = (int * int)

    type Board = int [] []

    type GameState = { Points: Point []; Board: Board }

    type Actions = BuildShips of Ships []
