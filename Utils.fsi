// file: ternaryboolean.fsi (signature file, look at the file extension !)


namespace Utils


module Debug =
    open Types.GameTypes

    val render: Board -> int [,]

module Board =
    open Types.GameTypes

    val drawPath: path:Point list -> cell:Cell -> board:Board -> Board
    val drawShip: state:GameState -> ship:Ship -> Board
