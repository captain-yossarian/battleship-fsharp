module Tests

open System
open Xunit
open Utils.Board
open Types.GameTypes

[<Fact>]
let ``Move point by (1,1)`` () =
    let result = movePoint (Point(2, 2)) (1, 1)
    Assert.Equal(result, Point(3, 3))

[<Fact>]
let ``Don't move the point`` () =
    let point = Point(2, 2)
    let result = movePoint point (0, 0)
    Assert.Equal(result, point)

[<Fact>]
let ``Move point in specified direction`` () =
    let point = Point(2, 2)
    let result = movePointByIndex N point 0
    Assert.Equal(result, Point(1, 2))
