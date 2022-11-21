namespace Bowling

module Frame =
  
  open System

  let rand = Random()

  type Slot =
    | EMPTY
    | PINS of int
    | SPARE
    | STRIKE

  type Frame = 
    { mutable Pins: int
      mutable Slots: Tuple<Slot, Slot>
      Interactive: bool }

    static member create (interactive) =
      { Pins = 10 
        Slots = EMPTY, EMPTY
        Interactive = interactive }

    static member getSlotLabel (slot: Slot) =
      match slot with
      | EMPTY -> "*"
      | SPARE -> "/"
      | STRIKE -> "X"
      | PINS n -> n.ToString()

    static member getSlotValue (slot: Slot) =
      match slot with
      | EMPTY -> 0
      | SPARE -> 5
      | STRIKE -> 10
      | PINS n -> n

    static member pluralizePins (numPins: int) = match numPins with | 1 -> "pin" | _ -> "pins"

    member private this.getPinsKnocked () =
      let pins = [|0..this.Pins|]
      printfn "Take a roll! Pins: %A" pins
      let pinsKnocked = 
        match this.Interactive with
        | true -> 
          let randNum = rand.Next(pins.Length)
          let guessed = Console.ReadLine() |> int
          printfn $"Rand: {randNum} | Guessed: {guessed}"
          let diff = abs (randNum - guessed)
          diff
        | false ->
          pins |> Array.item (rand.Next(pins.Length))
      pinsKnocked

    member this.roll () = 
      let pinsKnocked = this.getPinsKnocked()
      this.Pins <- this.Pins - pinsKnocked  
      let (slot1, _) = this.Slots
      let updatedSlots =
        match this.Pins with
        | 0 -> 
          match slot1 with
            | EMPTY -> (STRIKE, EMPTY)
            | _ -> (slot1, SPARE)
        | _ -> 
          match slot1 with 
            | EMPTY -> (PINS pinsKnocked, EMPTY)
            | _ -> (slot1, PINS pinsKnocked) 
      this.Slots <- updatedSlots
      updatedSlots

    member this.getDisplayString () =
      let slot1, slot2 = this.Slots
      let label1 = Frame.getSlotLabel slot1
      let label2 = Frame.getSlotLabel slot2
      $" {label1} : {label2} "

      