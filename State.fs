namespace State

module Constants =
    open Types.GameTypes
    open Utils.Board

    let ships =
        [| Carrier { Count = 1y; Size = 4y }
           Cruiser { Count = 2y; Size = 3y }
           Submarine { Count = 3y; Size = 2y }
           Destroyer { Count = 4y; Size = 1y } |]

    let board: Board = Array.create 10 (Array.create 10 0)

    let points = [| 0 .. 99 |] |> Array.map toCoordinates

    let initialState = { Points = points; Board = board }

module Reducer =
    open Types.GameTypes
    open Utils.Board
    open Constants

    let buildShips () = BuildShips ships

    let reducer (action: Actions) state =
        match action with
        | BuildShips ships ->
            let { Board = board } = state
            let { Count = count; Size = size } = ships

            state
