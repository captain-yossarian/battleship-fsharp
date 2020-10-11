module Tests

open System
open Xunit
open Utils.Board
open Types.GameTypes

[<Fact>]
let First () =
    let result = movePoint (Point(2, 2)) (1, 1)
    Assert.Equal(result, Point(3, 3))

[<Fact>]
let Second () =
    let result = movePoint (Point(2, 2)) (2, 2)
    Assert.Equal(result, Point(4, 3))
