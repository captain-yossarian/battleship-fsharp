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
        |> Array2D.mapi (fun rowi coli _ -> Point(rowi, coli) |> board.TryFind |> convertToNum)

    let summary board =
        board
        |> Map.fold (fun acc _ value ->
            match value with
            | Float -> acc + 1
            | _ -> acc) 0

    let success board = (summary board) = 20


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

    let movePoint (Point (row, column): Point) ((rowShift, columnShift): int * int) =
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
        let step = 1

        let predicate =
            fun index -> movePointByIndex direction point (index + step)

        point :: List.init (size - step) predicate

    let isInRange index =
        let lowestBound = 0
        let highestBound = 9
        index >= lowestBound && index <= highestBound

    let isPointInRange (Point (row, column)) = isInRange row && isInRange column

    let getCellBound point =
        WAYS
        |> List.choose (fun way ->
            match isPointInRange point with
            | true -> Some(movePointByIndex way point 1)
            | false -> None)

    let getBoundsPath shipPath =
        shipPath
        |> List.fold (fun acc point -> acc @ getCellBound point) List.empty

    let allowToDraw point (board: Board) cell =
        let value = board.TryGetValue(point)
        match (cell, value) with
        | (Float, (true, Initial)) -> true
        | (Bounds, (true, Initial))
        | (Bounds, (true, Bounds)) -> true
        | _ -> false

    let canBuildPath path cell board =
        path
        |> List.forall (fun elem -> allowToDraw elem board cell)

    let drawCell cell (board: Board) point = board.Add(point, cell)

    let drawPath path cell board = path |> List.fold (drawCell cell) board

    let getRandomData (points: Point list) =
        let index = randomNumber points.Length ()
        printfn "POINT INDEX %A" index
        let direction = randomDirection ()
        let point = points.Item(index)
        printfn "POINT ITSELF %A" points.Length


        (point, direction)

    let getEmptyPoints board =
        board
        |> Map.fold (fun acc key value ->
            match value with
            | Initial -> key :: acc
            | _ -> acc) List.empty

    let isEqualPoints (Point (rowx, columnx)) (Point (rowy, columny)) = rowx = rowy && columnx = columny

    let removePoint points point =
        points |> List.filter (fun pnt -> pnt <> point)
    // because of infinity recursion
    // @TODO add cache mechanism with bit mask of used directions

    let rec approvePath board ship =
        let emptyPoints = getEmptyPoints board

        let (point, direction) = getRandomData emptyPoints
        let shipPath = getShipPath ship direction point
        let boundsPath = getBoundsPath shipPath

        let shipPathApprove = canBuildPath shipPath Float board
        let boundsPathApprove = canBuildPath boundsPath Bounds board

        match (shipPathApprove, boundsPathApprove) with
        | (true, true) -> (shipPath, boundsPath)
        | _ -> approvePath board ship

    let drawShip state ship =
        let { Board = board } = state
        // let (point, direction) = getRandomData (getEmptyPoints board)
        let (shipPath, boundsPath) = approvePath board ship
        board
        |> drawPath boundsPath Bounds
        |> drawPath shipPath Float
