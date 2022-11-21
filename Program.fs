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
    let (slot1, slot2) = this.Scores

    // printfn "Pins knocked: %A" this.Pins
    // printfn "Pins knocked: %A" pinsKnocked

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

    // printfn "Reult: %A" result
    this.Scores <- result
    ()
  


[<EntryPoint>]
let main argv: int =
  
  // let frames: Frame[] = 
  //   (Array.create 10 (EMPTY 0, EMPTY 0))
    // |> Array.map throwBall
  // printfn "%A" frames

  let f1 = Frame.Default
  f1.roll()
  printfn "%A" (f1)
  f1.roll()
  printfn "%A" (f1)

  let f2 = Frame.Default
  f2.roll()
  printfn "%A" (f2)
  f2.roll()
  printfn "%A" (f2)

  0