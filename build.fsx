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

    let nuget = Path.Combine(Environment.GetFolderPath Environment.SpecialFolder.UserProfile, ".nuget", "packages")
    let fsharpCore =
        Path.Combine(nuget, "fsharp.core", "3.1.2.5", "lib", "net40", "FSharp.Core.dll")

    File.Copy(fsharpCore, Path.Combine("pack", "typeproviders", "fsharp41", "net45", "FSharp.Core.dll"), true)
)


#if DEBUG
do System.Diagnostics.Debugger.Launch() |> ignore
#endif


"Compile" ==> "CopyFSharpCore" ==> "CreatePackage"


entry()