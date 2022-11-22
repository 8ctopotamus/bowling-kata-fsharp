namespace Bowling

module Game = 

  open Bowling.Frame

  let playFrame (index: int) (frame: Frame) =
    printfn "Frame: %i" index
    printfn "Roll 1"
    let roll1, _ = frame.roll() // first roll
    printfn "You rolled a %s" (Frame.getSlotLabel roll1)
    let slot1, _ = frame.Slots // slot1 result
    match slot1 with 
    | EMPTY -> printfn "You got a strike so skipping second roll.\n"
    | _ -> 
      printfn "Roll 2"
      let _, roll2 = frame.roll(); 
      printfn "Second roll was a %s" (Frame.getSlotLabel roll2);
    printfn "----------------\n"
    
    // if on last frame
    if index = 10 then
      match roll1 with
      | STRIKE -> 
        printfn "Nice strike on the last frame!"
        frame.bonusRoll(2)
      | _ -> ()
      match roll1 with
      | SPARE -> 
        printfn "Nice spare on the last frame! Take another roll."
        frame.bonusRoll(2)
      | _ -> ()
    frame

  let calculateScore (frames: Frame[]) =
    let mutable score = 0
    for i in 0..frames.Length - 1 do
      let frame = frames[i]
      let slot1, slot2 = frame.Slots
      let v1 = Frame.getSlotValue slot1 
      let v2 = Frame.getSlotValue slot2
      
      // let nextFrame = frames[i+1]
      // let nextNextFrame = frames[i+2] 
        
      // TODO: fix spare and strike scoring

      // “spare” and his score for the frame is ten plus the number of pins knocked down on his next throw (in his next turn).

      // If on his first try in the frame he knocks down all the pins, this is called a “strike”. His turn is over, and his score for the frame is ten plus the simple total of the pins knocked down in his next two rolls.

      // If he gets a spare or strike in the last (tenth) frame, the bowler gets to throw one or two more bonus balls, respectively. These bonus throws are taken as part of the same turn. If the bonus throws knock down all the pins, the process does not repeat: the bonus throws are only used to calculate the score of the final frame.

      score <- score + v1 + v2
    score

  let printScoreboard (frames: Frame[]) =
      let yBorder = "-------------------------------------------------------------"
      printfn "SCOREBOARD:"
      let framesDisplay =
        frames 
        |> Array.map((fun frame -> frame.getDisplayString())) 
        |> String.concat "|"
      let score = frames |> calculateScore
      printfn "%s" yBorder
      printfn "|%s | Score: %i" framesDisplay score
      printfn "%s" yBorder

  let play (interactive: bool) =
    let frames = [| for i in 0..10 do yield Frame.create(interactive) |]

    printfn "Played frames:\n"
    // let playedFrames = frames |> Array.map (playFrame) // how to get index in map?
    let playedFrames = [| for i in 0..frames.Length - 1 do yield playFrame i frames[i] |]
    playedFrames |> printScoreboard
    
    let finalScore = playedFrames |> calculateScore
    finalScore