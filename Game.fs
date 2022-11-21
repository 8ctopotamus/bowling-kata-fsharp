namespace Bowling

module Game = 

  open Bowling.Frame

  let playFrame (frame: Frame) =
    let roll1, _ = frame.roll() // first roll
    printfn "You rolled a %A" roll1
    frame.print()
    let (slot1, _) = frame.Slots // slot1 result
    match slot1 with 
    | EMPTY -> printfn "You got a strike so skipping second roll."
    | _ -> 
      let _, roll2 = frame.roll(); 
      printfn "Second roll was a %A" roll2; frame.print();
    frame

  let calculateScore (frames: Frame[]) =
    let mutable score = 0
    for i in 0..frames.Length - 1 do
      let frame = frames[i]
      let slot1, slot2 = frame.Slots
      let v1 = Frame.getSlotValue slot1 
      let v2 = Frame.getSlotValue slot2
      // TODO: fix spare and strike scoring
      score <- score + v1 + v2
    score

  let play () =
    printfn "Staring frames:\n"
    let frames = [| for i in 1..10 do yield Frame.create() |]
    frames |> Frame.printScoreboard
    
    printfn "Played frames:\n"
    let playedFrames = frames |> Array.map playFrame
    playedFrames |> Frame.printScoreboard

    let finalScore = playedFrames |> calculateScore
    printfn "Final Score: %i" finalScore

