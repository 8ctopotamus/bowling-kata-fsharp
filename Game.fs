namespace Bowling

module Game = 

  open Bowling.Frame

  let playFrame (frame: Frame) =
    printfn "Roll 1"
    let roll1, _ = frame.roll() // first roll
    printfn "You rolled a %s" (Frame.getSlotLabel roll1)
    let (slot1, _) = frame.Slots // slot1 result
    match slot1 with 
    | EMPTY -> printfn "You got a strike so skipping second roll.\n"
    | _ -> 
      printfn "Roll 2"
      let _, roll2 = frame.roll(); 
      printfn "Second roll was a %s" (Frame.getSlotLabel roll2);
    printfn "----------------\n"
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

  let printScoreboard (frames: Frame[]) =
      let yBorder = "-----------------------------------------------------------------------------------"
      printfn "SCOREBOARD:"
      let framesDisplay =
        frames 
        |> Array.map((fun frame -> frame.getDisplayString())) 
        |> String.concat "|"
      let score = frames |> calculateScore
      printfn "%s" yBorder
      printfn "| %s | %i" framesDisplay score
      printfn "%s" yBorder

  let play (interactive: bool) =
    let frames = [| for i in 1..10 do yield Frame.create(interactive) |]

    printfn "Played frames:\n"
    let playedFrames = frames |> Array.map playFrame
    playedFrames |> printScoreboard
    
    let finalScore = playedFrames |> calculateScore
    finalScore