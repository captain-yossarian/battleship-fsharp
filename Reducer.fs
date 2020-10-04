namespace Reducer

module Reducers =
    open Types.GameTypes
    open Utils.Board

    let buildShips (ship) = BuildShip ship

    let reducer (action) (state) =
        match action with
        | BuildShip ship ->
            { state with
                  Board = drawShip state ship }
