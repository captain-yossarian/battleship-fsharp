namespace State

module Constants =
    open Types.GameTypes

    let toCoordinates (index: int) =
        let row = index / 10
        let column = index % 10
        Point(row, column)

    let ships =
        [| { Count = 1; Size = 4 }
           { Count = 2; Size = 3 }
           { Count = 3; Size = 2 }
           { Count = 4; Size = 1 } |]

    let WAYS = [ N; E; S; W; NE; SE; SW; NW ]

    let cellCount = 99

    let cells = [ 0 .. cellCount ]

    let predicate gameState cell =
        let { Board = prevBoard } = gameState
        let coordinates = toCoordinates cell
        let nextBoard = prevBoard.Add(coordinates, Initial)
        { Board = nextBoard }

    let initialState =
        cells |> List.fold predicate { Board = Map.empty }
