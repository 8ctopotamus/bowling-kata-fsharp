namespace Bowling.Frame

module Frame =
  open System

  let rand = Random()

  type RollResult =
    | EMPTY
    | PINS of int
    | SPARE
    | STRIKE