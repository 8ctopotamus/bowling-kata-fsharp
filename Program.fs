open Bowling

[<EntryPoint>]  
let main argv: int =
  
  printfn "--- F# BOWLING KATA ---"
  let finalScore = Game.play()
  printfn "Final Score: %i" finalScore
  
  0