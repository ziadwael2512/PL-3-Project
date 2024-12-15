//declartion fun 
let searchContact (enterd: string) =
    let enterdTrimmed = enterd.Trim().ToLower()
    //null spaces
    if System.String.IsNullOrWhiteSpace(enterdTrimmed) then
        printfn "invalid search"
    else
    //search contact by name or number 
        let findMatches (getField: Contact -> string) =
            contacts
            |> Map.filter (fun _ contact -> (getField contact).ToLower().Contains(enterdTrimmed))
            |> Map.toList

        let phone = findMatches (fun c -> c.PhoneNumber)
        let name = findMatches (fun c -> c.Name)
        //combine matches
        let allMatches = phone @ name

        if List.isEmpty allMatches then
            printfn "No contact found with '%s'" enterdTrimmed
        else
            allMatches |> List.iter (fun (_, contact) -> printfn "Found: %A" contact)
