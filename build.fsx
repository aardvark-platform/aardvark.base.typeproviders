#r "paket: groupref Build //"
#load ".fake/build.fsx/intellisense.fsx"
#load @"paket-files/build/aardvark-platform/aardvark.fake/DefaultSetup.fsx"

open System
open System.IO
open System.Diagnostics
open Aardvark.Fake
open Fake.Core
open Fake.Core.Operators
open Fake.Core.TargetOperators

do Environment.CurrentDirectory <- __SOURCE_DIRECTORY__

DefaultSetup.install ["src/Aardvark.Base.TypeProviders.sln"]


Target.create "CopyFSharpCore" (fun _ ->
    let core =
        if config.debug then Path.Combine("bin", "Debug", "net45", "FSharp.Core.dll")
        else Path.Combine("bin", "Release", "net45", "FSharp.Core.dll")

    File.Copy(core, Path.Combine("pack", "typeproviders", "fsharp41", "net45", "FSharp.Core.dll"), true)
)


#if DEBUG
do System.Diagnostics.Debugger.Launch() |> ignore
#endif


"Compile" ==> "CopyFSharpCore" ==> "CreatePackage"


entry()