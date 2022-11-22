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

    member private this.humanRoll () =
      let pins = [|0..this.Pins|]
      let randNum = rand.Next(pins.Length)
      printfn "Take a roll! Pins: %s (%i)" (pins |> string) this.Pins
      printfn $"Rand: {randNum}"
      let guessed = Console.ReadLine() |> int
      printfn $"Guessed: {guessed}"
      let pinsKnocked = abs (randNum - guessed)
      pinsKnocked

    member private this.autoRoll () =
      let pins = [|0..this.Pins|]
      pins |> Array.item (rand.Next(pins.Length))

    member this.roll () = 
      let pinsKnocked = 
        match this.Interactive with
        | true -> this.humanRoll()
        | false -> this.autoRoll()
      this.Pins <- this.Pins - pinsKnocked
      let (slot1, _) = this.Slots
      let updatedSlots =
        match this.Pins with
        | 0 -> // 0 pins left standing
          match slot1 with
            | EMPTY -> (STRIKE, EMPTY) // first roll
            | _ -> (slot1, SPARE) // second roll
        | _ -> // at least 1 pin left standing
          match slot1 with 
            | EMPTY -> (PINS pinsKnocked, EMPTY) // first roll
            | _ -> (slot1, PINS pinsKnocked) // second roll
      this.Slots <- updatedSlots
      updatedSlots

    member this.bonusRoll (numRolls: int) =
      this.Pins <- 10
      let mutable bonusPoints = 0
      for i in 1..numRolls do
        let pinsKnocked = 
          match this.Interactive with
          | true -> this.humanRoll()
          | false -> this.autoRoll()
        printfn $"Bonus roll {i}. Knocked {pinsKnocked} {(Frame.pluralizePins pinsKnocked)}"
        this.Pins <- this.Pins - pinsKnocked
        bonusPoints <- bonusPoints + pinsKnocked
        

    member this.getDisplayString () =
      let slot1, slot2 = this.Slots
      let label1 = Frame.getSlotLabel slot1
      let label2 = Frame.getSlotLabel slot2
      $" {label1}:{label2} "