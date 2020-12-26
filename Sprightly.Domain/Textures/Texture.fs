namespace Sprightly.Domain.Textures

open Sprightly.Common

/// <summary>
/// <see cref="Texture"/> defines the <see cref="Texture.T"/> and related
/// functions.
/// </summary>
module public Texture = 
    /// <summary>
    /// <see cref="Id"/> defines a texture id.
    /// </summary>
    type public Id = | Id of string * uint

    /// <summary>
    /// Convert the specified <paramref name="id"/> to its corresponding key
    /// string value.
    /// </summary>
    /// <param name="id">The <see cref='Id"/> to convert.</param>
    /// <returns>
    /// The corresponding key string.
    /// </returns>
    let public toKeyString (id: Id): string = 
        match id with | Id (idVal, idInt) -> idVal + "#" + idInt.ToString()

    let private getIdString (id: Id) : string = match id with Id (s, _) -> s
    let private getIdIndex (id: Id) : uint = match id with Id (_, i) -> i

    /// <summary>
    /// <see cref="Name"/> defines a texture name.
    /// </summary>
    type public Name = | Name of string

    /// <summary>
    /// The data of a single texture.
    /// </summary>
    type public Data = {
        Name: Name
        Path: Path.T
        MetaData: MetaData.T
    }

    /// <summary>
    /// <see cref="T"/> defines the texture type.
    /// </summary>
    type public T = {
        Id: Id
        Data: Data
    }

    /// <summary>
    /// Construct a new <see cref="T"/>
    /// </summary>
    /// <param name="id">The id of the texture. </param>
    /// <param name="name">The human-readable name of this texture. </param>
    /// <returns>
    /// A new texture.
    /// </returns>
    let construct (id: Id) (name: string) (path: Path.T) (metaData: MetaData.T) : T =
        { Id = id
          Data = { Name = Name name
                   Path = path
                   MetaData = metaData
                 }
        }

    /// <summary>
    /// <see cref="Store"/> defines a store of textures.
    /// </summary>
    type public Store = T list

    /// <summary>
    /// Create a new empty <see cref="Store"/>.
    /// </summary>
    /// <returns>
    /// A new empty <see cref="Store"/>.
    /// </returns>
    let public emptyStore () : Store = []

    let public getUniqueId (store: Store) (idString: string) : Id = 
        let usedIndices: uint Set = List.filter (fun (e: T) -> getIdString e.Id = idString) store
                                    |> List.map (fun (e: T) -> getIdIndex e.Id)
                                    |> Set.ofList

        let rec generateId (id: uint) =
            if Set.contains id usedIndices then 
                generateId (id + uint 1)
            else 
                id

        Id (idString, generateId <| uint 0)
