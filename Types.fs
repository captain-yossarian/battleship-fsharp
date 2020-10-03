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
        | Initial
        | Float
        | Sinking
        | Empty

    type Point = Point of (int * int)

    type Board = int [] []

    type GameState = { Points: Point list; Board: Board }

    type Actions = BuildShips of Ship []
