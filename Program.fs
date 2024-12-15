open System
open System.IO
open Newtonsoft.Json


// contact record
type Contact = {
    Id: int
    Name : string
    PhoneNumber: string
    Email: string
}
// id aute incremented
let mutable nextId = 1 

// contacts map storage
let mutable contacts: Map<int, Contact> = Map.empty



// add contact
let addContact name phone email =
    match contacts |> Map.exists (fun _ contact -> contact.Name = name),
        contacts |> Map.exists (fun _ contact -> contact.PhoneNumber = phone),
        phone.Length = 11 with
    | true, _, _ -> printfn "A contact with the same name '%s' already exists!" name
    | _, true, _ -> printfn "A contact with the same phone number '%s' already exists!" phone
    | _, _, false -> printfn "Phone number must be exactly 11 characters long."
    | false, false, true ->
        let newContact = { Id = nextId; Name = name; PhoneNumber = phone; Email = email }
        contacts <- contacts.Add(nextId, newContact)
        printfn "Contact '%s' added successfully!" name
        nextId <- nextId + 1 


// display all contacts
let displayAllContacts () =
    match Map.isEmpty contacts with
    | true -> printfn "No contacts available."
    | false ->
        contacts |> Map.iter (fun _ contact ->
            printfn "Id: %d, Name: %s, Phone: %s, Email: %s" contact.Id contact.Name contact.PhoneNumber contact.Email
        )     

// json save
let saveContacts filePath =
    let json = JsonConvert.SerializeObject(contacts)
    File.WriteAllText(filePath, json)
    printfn "Contacts saved to '%s'." filePath
    

// json load
let loadContacts filePath =
    let json = File.ReadAllText(filePath)                
    contacts <- JsonConvert.DeserializeObject<Map<int, Contact>>(json)  
    printfn "Contacts loaded from '%s'." filePath


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
        printfn "No contact found with the phone number'%s'."phone

let updateContactById (id: int) (newName: string) (newPhone: string) (newEmail: string) =
    if contacts.ContainsKey id then
        let updatedContact = { Id = id; Name = newName; PhoneNumber = newPhone; Email = newEmail }
        contacts <- contacts |> Map.add id updatedContact
        sprintf "Contact with ID %d updated successfully." id
    else
        "Contact not found."


[<EntryPoint>]
let main argv =
    displayAllContacts()
    0 // Return an integer exit code

