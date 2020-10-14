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

    let movePointByIndex directions point index =
        let shift = 1 + index
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

    let isInRange index =
        let lowestBound = 0
        let highestBound = 9
        index >= lowestBound && index <= highestBound

    let isPointInRange (Point (row, column)) = isInRange row && isInRange column

    let getCellBound point =
        WAYS
        |> List.choose (fun way ->
            match isPointInRange point with
            | true -> Some(((movePointByIndex way point 0), Bounds))
            | false -> None)

    let concat list1 list2 = list1 @ list2

    let makeBoundsPath (shipPath: (Point * Cell) list) =
        shipPath
        |> List.fold (fun acc (point, _) ->
            let bounds =
                getCellBound point
                |> List.filter (fun (pnt, _) -> not (List.contains (pnt, Float) shipPath))

            acc @ bounds) List.empty
        |> concat shipPath

    let makeShipPath shipSize point direction =
        let result =
            point
            :: List.init (shipSize - 1) (fun index -> (movePointByIndex direction (fst point) index, Float))

        makeBoundsPath result

    let allowToDraw (board: Board) (point, cell) =
        let value = board.TryGetValue(point)
        match (cell, value) with
        | (Float, (true, Initial)) -> true
        | (Bounds, (true, Initial))
        | (Bounds, (true, Bounds))
        | (Bounds, _) -> true
        | _ -> false

    let canBuildPath path board = path |> List.forall (allowToDraw board)

    let drawCell (board: Board) (point, cell) = board.Add(point, cell)

    let getRandomElement (points: Point list) =
        points.Item(randomNumber points.Length ())

    let getEmptyPoints board =
        board
        |> Map.fold (fun (acc: Point list) key value ->
            match value with
            | Initial -> key :: acc
            | _ -> acc) List.empty

    let isEqualPoints (Point (rowx, columnx)) (Point (rowy, columny)) = rowx = rowy && columnx = columny

    let removePoint points point =
        points |> List.filter (fun pnt -> pnt <> point)

    let availablePoint board =
        (getEmptyPoints >> getRandomElement) board

    let chooseDirection board applyDirection direction =
        canBuildPath (applyDirection direction) board

    let getWholePath shipSize board =
        let emptyPoints = getEmptyPoints board

        let rec isAllowed point =
            let applyDirection = makeShipPath shipSize (point, Float)
            let isDirectionOk = chooseDirection board applyDirection

            let chosenDirection = WAYS.[..3] |> List.tryFind isDirectionOk

            match chosenDirection with
            | (Some direction) -> applyDirection direction
            | None ->
                ((removePoint emptyPoints)
                 >> getRandomElement
                 >> isAllowed) point

        board |> availablePoint |> isAllowed

    let drawPath path board = path |> List.fold drawCell board

    let drawShip shipSize board =
        board |> (getWholePath shipSize >> drawPath)
