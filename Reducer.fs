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

            state


// drawCell (drawCell board point) result
