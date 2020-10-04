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

    let cellCount = 99

    let cells = [ 0 .. cellCount ]

    let folder =
        { Points = List.empty
          Board = Map.empty }

    let folderPredicate gameState cell =
        let { Points = prevPoints; Board = prevBoard } = gameState
        let coordinates = toCoordinates cell
        let nextBoard = prevBoard.Add(coordinates, Initial)
        let nextPoints = prevPoints @ [ coordinates ]
        { Points = nextPoints
          Board = nextBoard }

    let initialState =
        cells |> List.fold folderPredicate folder
