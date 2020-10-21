namespace Game

module Types =
    type Cell =
        | On
        | Off

    type Point = Point of (int * int)
    type Board = Map<Point, Cell>

    type GameState = { Board: Board; Generation: int }

    type Direction =
        | N
        | E
        | S
        | W
        | NE
        | SE
        | SW
        | NW

module Constants =
    open Types

    let ways = [ N; E; S; W; NE; SE; SW; NW ]

module State =
    open Types

    let cellCount = 9999

    let cells = [ 0 .. cellCount ]

    let toCoordinates (index: int) =
        let row = index / 100
        let column = index % 100
        Point(row, column)

    let predicate (board: Board) cell = board.Add(toCoordinates cell, Off)

    let initialState = cells |> List.fold predicate Map.empty

module Utils =
    open Types
    open Constants

    let convertToNum cell =
        match cell with
        | Some On -> 1
        | Some Off
        | None -> 0

    let movePointBy ((rowShift, columnShift): int * int) (Point (row, column): Point) =
        Point(row + rowShift, column + columnShift)

    let movePointTo direction =
        match direction with
        | N -> (-1, 0)
        | E -> (0, 1)
        | S -> (1, 0)
        | W -> (0, -1)
        | NE -> (-1, 1)
        | SE -> (1, 1)
        | SW -> (1, -1)
        | NW -> (-1, -1)

    let getCell point (board: Board) = point |> board.TryFind

    let movePoint = movePointTo >> movePointBy

    let neighborsCount point board =
        let neighbours acc direction =
            let moved = movePoint direction point
            let cell = getCell moved board
            acc + convertToNum cell

        ways |> List.fold neighbours 0

    let nextMove cell neighbors =
        match cell with
        | Some On ->
            match neighbors with
            | _ when neighbors < 2 -> Off
            | _ when neighbors >= 2 && neighbors <= 3 -> On
            | _ when neighbors > 3 -> Off
            | _ -> Off
        | Some Off ->
            match neighbors with
            | _ when neighbors = 3 -> On
            | _ -> Off
        | None -> Off

    let setInitialState (points: Point list) (board: Board) =
        points
        |> List.fold (fun (acc: Board) point -> acc.Add(point, On)) board

    let applyGeneration (board: Board) =
        board
        |> Map.fold (fun (acc: Board) point _ ->
            let cell = getCell point board
            let count = neighborsCount point board
            let nextGeneration = nextMove cell count

            acc.Add(point, nextGeneration)) Map.empty
