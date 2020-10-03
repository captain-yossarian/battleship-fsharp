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

    let randomDirection () =
        let index = randomCell 1
        if index = 0 then Vertical else Horizontal


    let drawCell (board: Board) (point: Point) =
        let (row, column) = point
        let copied = Array.copy board.[row]

        replaceByIndex board row (replaceByIndex copied column 1)

    let goVertical (board: Board) (size: sbyte) = 1
    let goHorizontal (board: Board) (size: sbyte) = 1

    let isEmpty (board: Board) (point: Point) =
        let (row, column) = point
        board.[row].[column] = 0

    let adjustBounds a shift =
        let tmp = a + shift
        if tmp < 0 || tmp > 9 then a else tmp

    let shiftPoint (point: Point) (shift: int * int) =
        let (row, column) = point
        let (rowShift, columnShift) = shift

        Point(adjustBounds row rowShift, adjustBounds column columnShift)


    let scan (board: Board) (point: Point) (ship: Ship) =
        let { Size = size } = ship

        let shifted = shiftPoint point

        let callback side =
            match side with
            | N -> shifted (-1, 0)
            | E -> shifted (0, 1)
            | S -> shifted (1, 0)
            | W -> shifted (0, -1)
            | NE -> shifted (-1, 1)
            | SE -> shifted (1, 1)
            | SW -> shifted (1, -1)
            | NW -> shifted (-1, 1)

        WAYS
        |> Array.map callback
        |> Array.forall (fun pnt -> isEmpty board pnt)


    let makePath (board: Board) (point: Point) (ship: Ship) =
        let direction = randomDirection ()
        let { Size = size } = ship
        match direction with
        | Vertical -> goVertical
        | Horizontal -> goHorizontal

    let render (board: Board) =
        printfn "Hello"
        board
        |> Array.iter (fun elem -> printfn "%A" elem)
