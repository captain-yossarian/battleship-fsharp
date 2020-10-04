namespace Reducer

module Reducers =
    open Types.GameTypes
    open Utils.Board
    open State.Constants

    let buildShips () = BuildShips ships

    let reducer (action) (state) =
        match action with
        | BuildShips ships ->
            let { Board = board; Points = points } = state
            let draw = drawShip state

            printfn "From: %A" (render draw)
            state


// drawCell (drawCell board point) result
