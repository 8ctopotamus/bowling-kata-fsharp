type Roll =
  | Empty of int
  | PINS of int
  | SPARE of string
  | STRIKE of string

  static member ToString () =
    "test"

let setUpFrames () = 
  (Empty 0, Empty 0)

[<EntryPoint>]
let main argv =
  
  let frames = 
    (Array.create 10 (PINS 0, PINS 0))
    |> Array.map (fun f -> Roll.ToString())


  printfn "%A" frames

  0