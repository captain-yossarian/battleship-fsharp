namespace Types

module GameTypes =
    type Ship = { Count: sbyte; Size: sbyte }

    type Plane =
        | Vertical
        | Horizontal

    type Cardinals =
        | N
        | E
        | S
        | W

    type Directions =
        | N
        | E
        | S
        | W
        | NE
        | SE
        | SW
        | NW

    type Cell =
        | Initial = 0
        | Float = 1
        | Sinking = 2
        | Empty = 3

    type Point = (int * int)

    type Board = int [] []

    type GameState = { Points: Point []; Board: Board }

    type Actions = BuildShips of Ship []
