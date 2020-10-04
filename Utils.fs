namespace Utils

module HandleArray =
    let splitArray a i = Array.take i a, Array.skip (i + 1) a


    let removeByIndex (arr: 'a []) (index: int) =
        let (head, tail) = splitArray arr index
        Array.concat [| head; tail |]

    let replaceByIndex (arr: 'a []) (index: int) (value: 'a) =
        let (head, tail) = splitArray arr index
        Array.concat [| head
                        [| value |]
                        tail |]

module Board =
    open HandleArray
    open Types.GameTypes
    open State.Constants


    let random = System.Random()

    let randomCell max () = random.Next(max)

    let binaryRandom a b =
        let random = random.Next(2)
        if random = 1 then a else b

    let randomDirection () =
        let index = randomCell 4 ()
        match index with
        | 0 -> N
        | 1 -> E
        | 2 -> S
        | 3 -> W
        | _ -> W

    // let initialCoordinates board point =







    let render (board: Board) =
        let convertToNum cell =
            match cell with
            | Some Initial -> 0
            | Some Float -> 1
            | Some Sinking -> 2
            | Some Bounds -> 3
            | None -> -1

        Array2D.create 10 10 0
        |> Array2D.mapi (fun rowi coli _ -> convertToNum (board.TryFind(Point(rowi, coli))))


    let goVertical (board: Board) (size: sbyte) = 1

    let goHorizontal (board: Board) (size: sbyte) = 1

    let adjustBounds a shift =
        let tmp = a + shift
        if tmp < 0 || tmp > 9 then a else tmp

    let isInRange index = index >= 0 && index <= 9

    let isPointInRange (Point (row, column)) = isInRange row && isInRange column

    let canUseDirection point direction ship =
        let (Point (row, column)) = point
        let { Size = size } = ship

        match direction with
        | N when (row - size) > 0 -> true
        | S when (row + size) < 10 -> true
        | W when (column - size) > 0 -> true
        | E when (column + size) < 10 -> true
        | _ -> false


    let shiftPoint (point: Point) (shift: int * int) =
        let (Point (row, column)) = point
        let (rowShift, columnShift) = shift

        Point(row + rowShift, column + columnShift)


    let scanPath directions point =
        let shifted = shiftPoint point
        match directions with
        | N -> shifted (-1, 0)
        | E -> shifted (0, 1)
        | S -> shifted (1, 0)
        | W -> shifted (0, -1)
        | NE -> shifted (-1, 1)
        | SE -> shifted (1, 1)
        | SW -> shifted (1, -1)
        | NW -> shifted (-1, -1)

    let generatePath point =
        WAYS
        |> List.map (fun way -> scanPath way point)
        |> List.filter isPointInRange

    let isPointEmpty point (board: Board) = fst (board.TryGetValue(point))


    let isPathSuccessful board path =
        path
        |> List.forall (fun elem -> isPointEmpty elem board)


    let drawCell (board: Board) point shipType = board.Add(point, shipType)

    let drawShip state =
        let { Board = board; Points = points } = state
        let index = randomCell points.Length ()
        let direction = randomDirection ()
        let point = points.Item(index)

        let path = generatePath point
        let ok = path |> isPathSuccessful board

        match ok with
        | true ->
            path
            |> List.fold (fun (board: Board) point -> board.Add(point, Bounds)) board
        | false -> board
