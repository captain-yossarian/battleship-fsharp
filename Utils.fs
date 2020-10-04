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


    let scanPath directions point shift =
        let shifted = shiftPoint point
        match directions with
        | N -> shifted (-shift, 0)
        | E -> shifted (0, shift)
        | S -> shifted (shift, 0)
        | W -> shifted (0, -shift)
        | NE -> shifted (-shift, shift)
        | SE -> shifted (shift, shift)
        | SW -> shifted (shift, -shift)
        | NW -> shifted (-shift, -shift)


    let makeShipPath ship direction point =
        let { Size = size } = ship
        let bound = 1

        let predicate =
            fun index -> scanPath direction point (index + bound)

        [ point ] @ List.init (size - bound) predicate


    let generateBounds point =
        WAYS
        |> List.map (fun way -> scanPath way point 1)
        |> List.filter isPointInRange


    let isPointEmpty point (board: Board) = fst (board.TryGetValue(point))


    let isPathSuccessful board path =
        path
        |> List.forall (fun elem -> isPointEmpty elem board)


    let drawCell (board: Board) point cell = board.Add(point, cell)

    let drawShip state ship =
        let { Board = board; Points = points } = state
        let index = randomCell points.Length ()
        let direction = randomDirection ()
        let point = points.Item(index)



        let drawShipCore = makeShipPath ship direction point

        let boundsPath =
            drawShipCore
            |> List.fold (fun acc elem -> acc @ generateBounds elem) List.empty

        let ok = boundsPath |> isPathSuccessful board

        printfn "Point %A, direction %A" point direction



        let part1 =
            boundsPath
            |> List.fold (fun (board: Board) point -> board.Add(point, Bounds)) board

        let part2 =
            drawShipCore
            |> List.fold (fun (board: Board) point -> board.Add(point, Float)) part1

        part2
