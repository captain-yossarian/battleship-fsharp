namespace Types
//https://www.reddit.com/r/fsharp/comments/j3g2gn/please_review_my_code/
module GameTypes =
    type Ship = { Count: int; Size: int }

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
        | Bounds

    type Point = Point of (int * int)

    type Board = Map<Point, Cell>

    type GameState = { Board: Board }

    type Actions = BuildShip of Ship
