namespace Aardvark.Base

open ProviderImplementation.ProvidedTypes
open Microsoft.FSharp.Core.CompilerServices
open System.Reflection

// This type defines the type provider. When compiled to a DLL, it can be added
// as a reference to an F# command-line compilation, script, or project.
[<TypeProvider>]
type PeanoTypeProvider(config: TypeProviderConfig) as this = 
//TypeProviderConfig * namespaceName:string * types: ProvidedTypeDefinition list
    inherit TypeProviderForNamespaces(config, assemblyReplacementMap=[("Aardvark.Base.TypeProviders.DesignTime", "Aardvark.Base.TypeProviders")])

    let namespaceName = "Aardvark.Base"
    let thisAssembly = Assembly.GetExecutingAssembly()

    let natType = ProvidedTypeDefinition(thisAssembly, namespaceName, "N", None)


    let rec getPeanoType (v : int) =
        if v < 0 then 
            failwith "cannot represent negative numbers"
        elif v = 0 then 
            typeof<Z>
        elif v >= 8192 then
            let inner = getPeanoType (v - 8192)
            typedefof<S8192<_>>.MakeGenericType [| inner |]
        elif v >= 4096 then
            let inner = getPeanoType (v - 4096)
            typedefof<S4096<_>>.MakeGenericType [| inner |]
        elif v >= 2048 then
            let inner = getPeanoType (v - 2048)
            typedefof<S2048<_>>.MakeGenericType [| inner |]
        elif v >= 1024 then
            let inner = getPeanoType (v - 1024)
            typedefof<S1024<_>>.MakeGenericType [| inner |]
        elif v >= 512 then
            let inner = getPeanoType (v - 512)
            typedefof<S512<_>>.MakeGenericType [| inner |]
        elif v >= 256 then
            let inner = getPeanoType (v - 256)
            typedefof<S256<_>>.MakeGenericType [| inner |]
        elif v >= 128 then
            let inner = getPeanoType (v - 128)
            typedefof<S128<_>>.MakeGenericType [| inner |]
        elif v >= 64 then
            let inner = getPeanoType (v - 64)
            typedefof<S64<_>>.MakeGenericType [| inner |]
        elif v >= 32 then
            let inner = getPeanoType (v - 32)
            typedefof<S32<_>>.MakeGenericType [| inner |]
        elif v >= 16 then
            let inner = getPeanoType (v - 16)
            typedefof<S16<_>>.MakeGenericType [| inner |]
        elif v >= 8 then
            let inner = getPeanoType (v - 8)
            typedefof<S8<_>>.MakeGenericType [| inner |]
        elif v >= 4 then
            let inner = getPeanoType (v - 4)
            typedefof<S4<_>>.MakeGenericType [| inner |]
        elif v >= 2 then
            let inner = getPeanoType (v - 2)
            typedefof<S2<_>>.MakeGenericType [| inner |]
        else
            typeof<S<Z>>


    do natType.DefineStaticParameters(
        [ProvidedStaticParameter("xy", typeof<int>)],
        fun typeName parameterValues ->
            match parameterValues with
                | [|:? int as number|] ->

                    if number < 0 then
                        failwith "cannot represent negative numbers"

                    let baseType = getPeanoType number
                    let ty = ProvidedTypeDefinition(thisAssembly, namespaceName, typeName, Some baseType)

                    ty

                | _ -> 
                    failwith "invalid args"
        )


    // And add them to the namespace
    do this.AddNamespace(namespaceName, [natType])
