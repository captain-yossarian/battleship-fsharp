namespace Reducer

module Reducers =
    open Types.GameTypes
    open Utils.Board

    let buildShip (ship) = MakeShip ship

    let reducer (action) (state) =
        match action with
        | MakeShip ship ->
            { state with
                  Board = drawShip state ship }
