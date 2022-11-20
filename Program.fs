open System
open FSharp.Reflection

let rand = Random()

type Roll =
  | Empty of int
  | PINS of int
  | SPARE of int
  | STRIKE of int
  
  // member x.get() =
    // x.GetUnionCases(typeof<'T>)

type Frame = Tuple<Roll, Roll>

// let throwBall (frame: Frame) =
//   PINS 1, PINS 3

[<EntryPoint>]
let main argv: int =
  
  let frames: Frame[] = 
    (Array.create 10 (Empty 0, Empty 0))
    // |> Array.map throwBall


  let recordType = typedefof<Roll>
  let cases = FSharpType.GetUnionCases(recordType)
  printfn "%A" cases 
  // let c = FSharpType.GetUnionCases <Roll>

  // printfn "%A" frames
  
  0