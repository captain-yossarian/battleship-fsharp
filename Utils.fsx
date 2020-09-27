#load "Types.fsx"
open Types

module Utils =
    let toCoordinates (index: sbyte) =
        let row = index / 10y
        let column = index % 10y
        Types.Point(row, column)

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

    let drawCell (board: Types.Board) (point: int * int) =
        let (row, column) = point
        let copied = Array.copy board.[row]

        replaceByIndex board row (replaceByIndex copied column 2)
