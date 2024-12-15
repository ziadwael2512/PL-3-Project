open System
open System.IO
open System.Drawing
open System.Windows.Forms
open Newtonsoft.Json

// Contact record
type Contact = {
    Id: int
    Name: string
    PhoneNumber: string
    Email: string
}

// Mutable state for contacts
let mutable nextId = 1
let mutable contacts: Map<int, Contact> = Map.empty

// Backend functions
let saveContacts filePath =
    let json = JsonConvert.SerializeObject(contacts)
    File.WriteAllText(filePath, json)
    sprintf "Contacts saved to '%s'." filePath
let addContact name phone email =
    match contacts |> Map.exists (fun _ contact -> contact.Name = name),
        contacts |> Map.exists (fun _ contact -> contact.PhoneNumber = phone),
        phone.Length = 11 with
    | true, _, _ -> "A contact with the same name already exists!"
    | _, true, _ -> "A contact with the same phone number already exists!"
    | _, _, false -> "Phone number must be exactly 11 characters long."
    | false, false, true ->
        let newContact = { Id = nextId; Name = name; PhoneNumber = phone; Email = email }
        contacts <- contacts.Add(nextId, newContact)
        saveContacts "contacts.json"|>ignore
        nextId <- nextId + 1
        sprintf "Contact '%s' added successfully!" name

let loadContacts filePath =
    let json = File.ReadAllText(filePath)                
    let contacts = JsonConvert.DeserializeObject<Map<int, Contact>>(json)  
    // Create a string with the contact details
    contacts |> Map.fold (fun acc _ contact -> 
        acc + sprintf "Id: %d, Name: %s, Phone: %s, Email: %s\n" contact.Id contact.Name contact.PhoneNumber contact.Email
    ) ""

let searchContact (query: string) =
    let trimmedQuery = query.Trim().ToLower()
    if System.String.IsNullOrWhiteSpace(trimmedQuery) then
        "Invalid search input."
    else
        let matches =
            contacts
            |> Map.filter (fun _ contact ->
                contact.Name.ToLower().Contains(trimmedQuery) ||
                contact.PhoneNumber.Contains(trimmedQuery))
        if Map.isEmpty matches then
            sprintf "No contact found for '%s'." trimmedQuery
        else
            matches
            |> Map.fold (fun acc _ contact -> acc + sprintf "Id: %d, Name: %s, Phone: %s, Email: %s\n" contact.Id contact.Name contact.PhoneNumber contact.Email) ""

let deleteContactByName name =
    match contacts |> Map.tryFindKey (fun _ contact -> contact.Name = name) with
    | Some id ->
        contacts <- contacts.Remove(id)
        sprintf "Contact with name '%s' deleted successfully!" name
    | None -> sprintf "No contact found with the name '%s'." name

let updateContactById id newName newPhone newEmail =
    if contacts.ContainsKey id then
        let updatedContact = { Id = id; Name = newName; PhoneNumber = newPhone; Email = newEmail }
        contacts <- contacts.Add(id, updatedContact)
        sprintf "Contact with ID %d updated successfully." id
    else
        "Contact not found."

// GUI Forms
let createAddContactForm () =
    let addForm = new Form(Text = "Add New Contact", Width = 500, Height = 500, BackColor = Color.LightBlue)
    let nameLabel = new Label(Text = "Name:", Top = 20, Width = 100)
    let phoneLabel = new Label(Text = "Phone Number:", Top = 60, Width = 100)
    let emailLabel = new Label(Text = "Email:", Top = 100, Width = 100)
    let nameTextBox = new TextBox(Top = 20, Width = 250)
    let phoneTextBox = new TextBox(Top = 60, Width = 250)
    let emailTextBox = new TextBox(Top = 100, Width = 250)
    let saveButton = new Button(Text = "Save Contact", Top = 160, Width = 200, BackColor = Color.LightGreen)
    let resultLabel = new Label(Top = 220, Width = 400, Height = 150, BorderStyle = BorderStyle.FixedSingle, TextAlign = ContentAlignment.MiddleCenter)

    nameLabel.Left <- (addForm.ClientSize.Width - nameLabel.Width - nameTextBox.Width) / 2
    nameTextBox.Left <- nameLabel.Left + nameLabel.Width
    phoneLabel.Left <- nameLabel.Left
    phoneTextBox.Left <- phoneLabel.Left + phoneLabel.Width
    emailLabel.Left <- nameLabel.Left
    emailTextBox.Left <- emailLabel.Left + emailLabel.Width
    saveButton.Left <- (addForm.ClientSize.Width - saveButton.Width) / 2
    resultLabel.Left <- (addForm.ClientSize.Width - resultLabel.Width) / 2

    saveButton.Click.Add(fun _ ->
        resultLabel.Text <- addContact nameTextBox.Text phoneTextBox.Text emailTextBox.Text
    )

    addForm.Controls.AddRange [| nameLabel; phoneLabel; emailLabel; nameTextBox; phoneTextBox; emailTextBox; saveButton; resultLabel |]
    addForm

let createSearchContactForm () =
    let searchForm = new Form(Text = "Search Contact", Width = 500, Height = 500, BackColor = Color.LightBlue)
    let searchLabel = new Label(Text = "Enter Name or Phone:", Top = 20, Width = 150)
    let searchTextBox = new TextBox(Top = 20, Width = 250)
    let searchButton = new Button(Text = "Search", Top = 120, Width = 200, BackColor = Color.LightGreen)
    let resultLabel = new Label(Top = 180, Width = 400, Height = 200, BorderStyle = BorderStyle.FixedSingle, TextAlign = ContentAlignment.MiddleCenter)

    searchLabel.Left <- (searchForm.ClientSize.Width - searchLabel.Width - searchTextBox.Width) / 2
    searchTextBox.Left <- searchLabel.Left + searchLabel.Width
    searchButton.Left <- (searchForm.ClientSize.Width - searchButton.Width) / 2
    resultLabel.Left <- (searchForm.ClientSize.Width - resultLabel.Width) / 2

    searchButton.Click.Add(fun _ ->
        resultLabel.Text <- searchContact searchTextBox.Text
    )

    searchForm.Controls.AddRange [| searchLabel; searchTextBox; searchButton; resultLabel |]
    searchForm

let createUpdateContactForm () =
    let updateForm = new Form(Text = "Update Contact", Width = 500, Height = 500, BackColor = Color.LightYellow)
    let idLabel = new Label(Text = "Contact ID:", Top = 20, Width = 100)
    let nameLabel = new Label(Text = "New Name:", Top = 60, Width = 100)
    let phoneLabel = new Label(Text = "New Phone:", Top = 100, Width = 100)
    let emailLabel = new Label(Text = "New Email:", Top = 140, Width = 100)
    let idTextBox = new TextBox(Top = 20, Width = 250)
    let nameTextBox = new TextBox(Top = 60, Width = 250)
    let phoneTextBox = new TextBox(Top = 100, Width = 250)
    let emailTextBox = new TextBox(Top = 140, Width = 250)
    let updateButton = new Button(Text = "Update Contact", Top = 200, Width = 200, BackColor = Color.Orange)
    let resultLabel = new Label(Top = 260, Width = 400, Height = 150, BorderStyle = BorderStyle.FixedSingle, TextAlign = ContentAlignment.MiddleCenter)

    idLabel.Left <- (updateForm.ClientSize.Width - idLabel.Width - idTextBox.Width) / 2
    idTextBox.Left <- idLabel.Left + idLabel.Width
    nameLabel.Left <- idLabel.Left
    nameTextBox.Left <- nameLabel.Left + nameLabel.Width
    phoneLabel.Left <- idLabel.Left
    phoneTextBox.Left <- phoneLabel.Left + phoneLabel.Width
    emailLabel.Left <- idLabel.Left
    emailTextBox.Left <- emailLabel.Left + emailLabel.Width
    updateButton.Left <- (updateForm.ClientSize.Width - updateButton.Width) / 2
    resultLabel.Left <- (updateForm.ClientSize.Width - resultLabel.Width) / 2

    updateButton.Click.Add(fun _ ->
        let id = idTextBox.Text |> int
        resultLabel.Text <- updateContactById id nameTextBox.Text phoneTextBox.Text emailTextBox.Text
    )

    updateForm.Controls.AddRange [| idLabel; nameLabel; phoneLabel; emailLabel; idTextBox; nameTextBox; phoneTextBox; emailTextBox; updateButton; resultLabel |]
    updateForm

let createDeleteContactForm () =
    let deleteForm = new Form(Text = "Delete Contact", Width = 500, Height = 500, BackColor = Color.LightCoral)
    let nameLabel = new Label(Text = "Contact Name:", Top = 20, Width = 150)
    let nameTextBox = new TextBox(Top = 20, Width = 250)
    let deleteButton = new Button(Text = "Delete Contact", Top = 80, Width = 200, BackColor = Color.Red)
    let resultLabel = new Label(Top = 140, Width = 400, Height = 150, BorderStyle = BorderStyle.FixedSingle, TextAlign = ContentAlignment.MiddleCenter)

    nameLabel.Left <- (deleteForm.ClientSize.Width - nameLabel.Width - nameTextBox.Width) / 2
    nameTextBox.Left <- nameLabel.Left + nameLabel.Width
    deleteButton.Left <- (deleteForm.ClientSize.Width - deleteButton.Width) / 2
    resultLabel.Left <- (deleteForm.ClientSize.Width - resultLabel.Width) / 2

    deleteButton.Click.Add(fun _ ->
        resultLabel.Text <- deleteContactByName nameTextBox.Text
    )

    deleteForm.Controls.AddRange [| nameLabel; nameTextBox; deleteButton; resultLabel |]
    deleteForm

let createViewAllContactsForm () =
    let viewForm = new Form(Text = "All Contacts", Width = 500, Height = 500, BackColor = Color.LightGray)
    let contactsTextBox = new TextBox(Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical, Dock = DockStyle.Fill)
    contactsTextBox.Text <- loadContacts "contacts.json"
    viewForm.Controls.Add(contactsTextBox)
    viewForm

[<EntryPoint>]
let main argv =
    let form = new Form(Text = "Contact Management System", Width = 600, Height = 600, BackColor = Color.WhiteSmoke)
    let addButton = new Button(Text = "Add Contact", Top = 50, Width = 200, BackColor = Color.LightSkyBlue)
    let searchButton = new Button(Text = "Search Contact", Top = 120, Width = 200, BackColor = Color.LightGreen)
    let updateButton = new Button(Text = "Update Contact", Top = 190, Width = 200, BackColor = Color.LightYellow)
    let deleteButton = new Button(Text = "Delete Contact", Top = 260, Width = 200, BackColor = Color.LightCoral)
    let viewButton = new Button(Text = "View All Contacts", Top = 330, Width = 200, BackColor = Color.LightSlateGray)
    let exitButton = new Button(Text = "Exit Application", Top = 400, Width = 200, BackColor = Color.LightSalmon)

    addButton.Left <- (form.ClientSize.Width - addButton.Width) / 2
    searchButton.Left <- (form.ClientSize.Width - searchButton.Width) / 2
    updateButton.Left <- (form.ClientSize.Width - updateButton.Width) / 2
    deleteButton.Left <- (form.ClientSize.Width - deleteButton.Width) / 2
    viewButton.Left <- (form.ClientSize.Width - viewButton.Width) / 2
    exitButton.Left <- (form.ClientSize.Width - exitButton.Width) / 2

    addButton.Click.Add(fun _ -> createAddContactForm().ShowDialog() |> ignore)
    searchButton.Click.Add(fun _ -> createSearchContactForm().ShowDialog() |> ignore)
    updateButton.Click.Add(fun _ -> createUpdateContactForm().ShowDialog() |> ignore)
    deleteButton.Click.Add(fun _ -> createDeleteContactForm().ShowDialog() |> ignore)
    viewButton.Click.Add(fun _ -> createViewAllContactsForm().ShowDialog() |> ignore)
    exitButton.Click.Add(fun _ -> form.Close())

    form.Controls.AddRange [| addButton; searchButton; updateButton; deleteButton; viewButton; exitButton |]
    Application.Run(form)
    0