namespace State

module Constants =
    open Types.GameTypes

    let toCoordinates (index: int) =
        let row = index / 10
        let column = index % 10
        Point(row, column)

    let ships =
        [| { Count = 1y; Size = 4y }
           { Count = 2y; Size = 3y }
           { Count = 3y; Size = 2y }
           { Count = 4y; Size = 1y } |]

    let WAYS = [| N; E; S; W; NE; SE; SW; NW |]

    let board: Board = Array.create 10 (Array.create 10 0)

    let points = [| 0 .. 99 |] |> Array.map toCoordinates

    let initialState = { Points = points; Board = board }
