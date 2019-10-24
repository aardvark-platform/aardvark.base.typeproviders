// Learn more about F# at http://fsharp.org

open System
open Aardvark.Base

type A = N<8191>

[<EntryPoint>]
let main argv =
    printfn "%A" typeof<A>
    0 // return an integer exit code
