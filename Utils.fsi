// file: ternaryboolean.fsi (signature file, look at the file extension !)


namespace Utils


module Debug =
    open Types.GameTypes

    val render: Board -> int [,]

module Board =
    open Types.GameTypes

    val drawShip: GameState -> Ship -> Board
