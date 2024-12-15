delete_contact
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


        


//test
addContact "shahd farag" "98765432111" "shahdfarag@gmail.com"
addContact "shahd sharaf" "11123456789" "shahdsharaf@gamil.com"
addContact "fady nabil" "12345678900" "fadynabil@gamil.com"
addContact "zeyad wael" "00123456789" "zeyadwael@gamil.com"
addContact "menna abdelnasser" "22334567891" "menna@gmail.com"
addContact "shahd farag" "88888888888" "shahhhd@gmail.com" // check an exists name
addContact "shahd " "12223456789" "shahd@gamil.com"   // check an exists phoneNo.
addContact "nour salah" "123456" "nour@gmail.com" //check phoneNo. less than 11
addContact "nour salah" "11223344556677" "nour@gmail.com" // check phoneNo. more than 11
//saveContacts "contacts.json"
printfn"-----------------------------------------------------\n"
displayAllContacts()
printfn"-----------------------------------------------------\n"
printfn"-----------------------------------------------------\n"
saveContacts "contacts.json"

 main
