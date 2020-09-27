namespace Utils


module HandleArray =
    let splitArray (arr: 'a []) (index: int) =
        let head = arr.[..index - 1]
        let tail = arr.[index + 1..]
        (head, tail)

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


    let toCoordinates (index: sbyte) =
        let row = index / 10y
        let column = index % 10y
        Point(row, column)

    let drawCell (board: Board) (point: int * int) =
        let (row, column) = point
        let copied = Array.copy board.[row]

        replaceByIndex board row (replaceByIndex copied column 2)

    let render (board: Board) =
        printfn "Hello"
        board
        |> Array.iter (fun elem -> printfn "%A" elem)
