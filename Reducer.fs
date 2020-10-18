namespace Reducer

module Reducers =
    open Types.GameTypes
    open Utils.Board

    let buildShip (ship) = MakeShip ship

    let reducer (action) (state) =
        match action with
        | MakeShip { Size = size } ->
            let { Board = board } = state
            { state with
                  Board = drawShip size board }
