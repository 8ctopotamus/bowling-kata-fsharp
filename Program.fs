open Bowling

[<EntryPoint>]
let main argv: int =

  let interactive = not (argv |> Array.isEmpty)
  printfn "--- F# BOWLING KATA ---"
  printfn "Interactive mode: %b" interactive

  let finalScore = Game.play(interactive)
  printfn "Final Score: %i" finalScore

  0