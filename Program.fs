open System

let rand = Random()

type RollResult =
  | EMPTY
  | PINS of int
  | SPARE
  | STRIKE

type Frame = 
  { mutable Pins: int
    mutable Scores: Tuple<RollResult, RollResult> }

  static member Default =
    { Pins = 10 
      Scores = (EMPTY, EMPTY)}

  member this.roll () = 
    let pins = [|0..this.Pins|]
    let pinsKnocked = pins |> Array.item (rand.Next(pins.Length))
    this.Pins <- this.Pins - pinsKnocked
    let (slot1, _) = this.Scores
    let result =
      match this.Pins with
      | 0 -> 
        match slot1 with
          | EMPTY -> (STRIKE, EMPTY)
          | _ -> (slot1, SPARE)
      | _ -> 
        match slot1 with 
          | EMPTY -> (PINS pinsKnocked, EMPTY)
          | _ -> (slot1, PINS pinsKnocked)      
    this.Scores <- result
    ()

let playFrames (frame: Frame) =
  frame.roll()
  frame.roll()
  frame

[<EntryPoint>]
let main argv: int =
  
  let frames = 
    (Array.create 10 Frame.Default)
    |> Array.map playFrames

  printfn "%A" frames

  0