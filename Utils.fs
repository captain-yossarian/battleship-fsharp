namespace Utils

module Debug =
    open Types.GameTypes

    let convertToNum cell =
        match cell with
        | Some Initial -> 0
        | Some Float -> 1
        | Some Sinking -> 2
        | Some Bounds -> 3
        | None -> -1

    let render (board: Board) =
        Array2D.create 10 10 0
        |> Array2D.mapi (fun rowi coli _ -> convertToNum (board.TryFind(Point(rowi, coli))))

module Random =
    open Types.GameTypes

    let random = System.Random()

    let randomNumber max () = random.Next(max)

    let randomDirection () =
        let index = randomNumber 4 ()
        match index with
        | 0 -> N
        | 1 -> E
        | 2 -> S
        | 3 -> W
        | _ -> W

module Board =
    open Types.GameTypes
    open State.Constants
    open Random

    let movePoint (point: Point) (shift: int * int) =
        let (Point (row, column)) = point
        let (rowShift, columnShift) = shift
        Point(row + rowShift, column + columnShift)

    let movePointByIndex directions point shift =
        let movePointBy = movePoint point
        match directions with
        | N -> movePointBy (-shift, 0)
        | E -> movePointBy (0, shift)
        | S -> movePointBy (shift, 0)
        | W -> movePointBy (0, -shift)
        | NE -> movePointBy (-shift, shift)
        | SE -> movePointBy (shift, shift)
        | SW -> movePointBy (shift, -shift)
        | NW -> movePointBy (-shift, -shift)

    let getShipPath ship direction point =
        let { Size = size } = ship
        let bound = 1

        let predicate =
            fun index -> movePointByIndex direction point (index + bound)

        [ point ] @ List.init (size - bound) predicate

    let isInRange index = index >= 0 && index <= 9

    let isPointInRange (Point (row, column)) = isInRange row && isInRange column

    let getCellBound point =
        WAYS
        |> List.fold (fun acc way ->
            match isPointInRange point with
            | true -> acc @ [ movePointByIndex way point 1 ]
            | false -> acc) List.empty

    let getBoundsPath shipPath =
        shipPath
        |> List.fold (fun acc point -> acc @ getCellBound point) List.empty

    let isCellEmpty point (board: Board) = fst (board.TryGetValue(point))

    let isPathSuccessful board path =
        path
        |> List.forall (fun elem -> isCellEmpty elem board)

    let drawCell cell (board: Board) point = board.Add(point, cell)

    let drawPath path cell board = path |> List.fold (drawCell cell) board

    let getRandomData (points: Point list) =
        let index = randomNumber points.Length ()
        let direction = randomDirection ()
        let point = points.Item(index)
        (point, direction)

    let drawShip state ship =
        let { Board = board; Points = points } = state
        let (point, direction) = getRandomData points
        let shipPath = getShipPath ship direction point
        let boundsPath = getBoundsPath shipPath

        board
        |> drawPath boundsPath Bounds
        |> drawPath shipPath Float
