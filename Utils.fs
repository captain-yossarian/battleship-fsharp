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
    open Debug

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

    let makeShipPath ship direction point =
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
        | (Bounds, (true, Bounds))
        | (Bounds, _) -> true
        | _ -> false

    let canBuildPath path cell board =
        path
        |> List.forall (fun elem -> allowToDraw elem board cell)

    let drawCell cell (board: Board) point = board.Add(point, cell)

    let drawPath path cell board = path |> List.fold (drawCell cell) board

    let getRandomElement (points: Point list) =
        let index = randomNumber points.Length ()
        let point = points.Item(index)
        point

    let getEmptyPoints board =
        board
        |> Map.fold (fun (acc: Point list) key value ->

            match value with
            | Initial -> key :: acc
            | _ -> acc) List.empty

    let isEqualPoints (Point (rowx, columnx)) (Point (rowy, columny)) = rowx = rowy && columnx = columny

    let removePoint points point =
        points |> List.filter (fun pnt -> pnt <> point)

    let rec getShipPath ship (point: Point) (board: Board) (directions: Directions list) =
        match directions with
        | [ x ] -> makeShipPath ship x point
        | x :: xs ->
            let path = makeShipPath ship x point

            match canBuildPath path Float board with
            | true -> path
            | false -> getShipPath ship point board xs
        | [] -> makeShipPath ship N point

    let getWholePath board ship =
        let point =
            (getEmptyPoints >> getRandomElement) board

        let shipPath = getShipPath ship point board WAYS.[..3]
        let boundsPath = getBoundsPath shipPath

        (shipPath, boundsPath)

    let canProceed (shipPath, boundsPath) board =
        let shipPathApprove = canBuildPath shipPath Float board
        let boundsPathApprove = canBuildPath boundsPath Bounds board
        (shipPathApprove, boundsPathApprove)


    let rec approvedPath board ship =
        let wholePath = getWholePath board ship
        let wholePathApprove = canProceed wholePath board

        match wholePathApprove with
        | (true, true) -> wholePath
        | _ -> approvedPath board ship

    let drawShip state ship =
        let { Board = board } = state
        let (shipPath, boundsPath) = approvedPath board ship

        board
        |> drawPath boundsPath Bounds
        |> drawPath shipPath Float
