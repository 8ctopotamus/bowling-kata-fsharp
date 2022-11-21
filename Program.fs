﻿open System

let rand = Random()

type RollResult =
  | EMPTY
  | PINS of int
  | SPARE
  | STRIKE

type Frame = 
  { mutable Pins: int
    mutable Slots: Tuple<RollResult, RollResult> }

  static member create () =
    { Pins = 10 
      Slots = EMPTY, EMPTY }

  static member getSlotLabel (slot: RollResult) =
    match slot with
    | EMPTY -> "*"
    | SPARE -> "/"
    | STRIKE -> "X"
    | PINS n -> n.ToString()

  static member getSlotValue (slot: RollResult) =
    match slot with
    | EMPTY -> 0
    | SPARE -> 5
    | STRIKE -> 10
    | PINS n -> n

  static member pluralizePins (numPins: int) = match numPins with | 1 -> "" | _ -> "s"

  static member printScoreboard (frames: Frame[]) =
    frames |> Array.iter (fun frame -> frame.print())
    ()

  member this.roll () = 
    printfn "Roll 1..."
    let pins = [|0..this.Pins|]
    let pinsKnocked = pins |> Array.item (rand.Next(pins.Length))
    let pinsKnockedMsg = Frame.pluralizePins pinsKnocked
    let pinsLeftMsg = Frame.pluralizePins this.Pins
    printfn $"{pinsKnocked} {pinsKnockedMsg} knocked! {this.Pins} {pinsLeftMsg} remaining."
    let (slot1, _) = this.Slots
    let updatedSlots =
      match this.Pins with
      | 0 -> 
        match slot1 with
          | EMPTY -> printfn "STRIKE!"; (STRIKE, EMPTY)
          | _ -> printfn "SPARE!"; (slot1, SPARE)
      | _ -> 
        match slot1 with 
          | EMPTY -> (PINS pinsKnocked, EMPTY)
          | _ -> (slot1, PINS pinsKnocked)
    this.Pins <- this.Pins - pinsKnocked  
    this.Slots <- updatedSlots
    updatedSlots

  member this.print () =
    let slot1, slot2 = this.Slots
    let label1 = Frame.getSlotLabel slot1
    let label2 = Frame.getSlotLabel slot2
    printfn $"| {label1} | {label2} |"

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
    score <- score + v1 + v2
  score

[<EntryPoint>]
let main argv: int =
  
  printfn "Staring frames:\n"
  let frames = [| for i in 1..10 do yield Frame.create() |]
  frames |> Frame.printScoreboard
  
  printfn "Played frames:\n"
  let playedFrames = frames |> Array.map playFrame
  playedFrames |> Frame.printScoreboard

  let finalScore = playedFrames |> calculateScore
  printfn "Final Score: %i" finalScore
  0