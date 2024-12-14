// delete contact by name
let deleteContactByName name =
    let contactToDelete = contacts |> Map.tryFindKey (fun _ contact -> contact.Name = name)
    match contactToDelete with
    | Some id ->
        contacts <- contacts.Remove(id)
        printfn "Contact with name '%s' deleted successfully!" name
    | None ->
        printfn "No contact found with the name '%s'." name

// delete contact by phone number
let deleteContactByPhone phone =
    let contactToDelete = contacts |> Map.tryFindKey (fun _ contact -> contact.PhoneNumber = phone)
    match contactToDelete with
    | Some id ->
        contacts <- contacts.Remove(id)
        printfn "Contact with phone number '%s' deleted successfully!" phone
    | None ->
        printfn "No contact found with the phone number '%s'." phone