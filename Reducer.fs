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
            let index = randomCell points.Length ()
            let direction = randomDirection ()
            let point = points.Item(index)
            let result = scan board point direction
            printfn "From: %A, to: %A, direction: %A" point result direction

            drawCell (drawCell board point) result
