﻿open System
open System.Drawing
open System.Windows.Forms

// Form for updating a contact
let createUpdateContactForm () =
    let updateForm = new Form(Text = "Update Contact", Width = 500, Height = 500, BackColor = Color.LightBlue)

    // Labels and textboxes
    let idLabel = new Label(Text = "Contact ID:", Top = 20, Left = 30, Font = new Font("Arial", 10.0f, FontStyle.Bold))
    let idTextBox = new TextBox(Top = 20, Left = 150, Width = 250)

    let nameLabel = new Label(Text = "New Name:", Top = 60, Left = 30, Font = new Font("Arial", 10.0f, FontStyle.Bold))
    let nameTextBox = new TextBox(Top = 60, Left = 150, Width = 250)

    let phoneLabel = new Label(Text = "New Phone:", Top = 100, Left = 30, Font = new Font("Arial", 10.0f, FontStyle.Bold))
    let phoneTextBox = new TextBox(Top = 100, Left = 150, Width = 250)

    let emailLabel = new Label(Text = "New Email:", Top = 140, Left = 30, Font = new Font("Arial", 10.0f, FontStyle.Bold))
    let emailTextBox = new TextBox(Top = 140, Left = 150, Width = 250)

    let updateButton = new Button(Text = "Update Contact", Top = 200, Left = 150, Width = 200, BackColor = Color.LightGreen, Font = new Font("Arial", 10.0f, FontStyle.Bold))
    let resultLabel = new Label(Top = 260, Left = 30, Width = 400, Height = 150, BorderStyle = BorderStyle.FixedSingle, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 12.0f, FontStyle.Bold))

    // Add controls
    updateForm.Controls.AddRange [| idLabel; idTextBox; nameLabel; nameTextBox; phoneLabel; phoneTextBox; emailLabel; emailTextBox; updateButton; resultLabel |]

    // Event for update button (Logic for updating a contact will go here)
    updateButton.Click.Add(fun _ -> 
        // Call your update contact logic here
        // Example: updateContact(idTextBox.Text, nameTextBox.Text, phoneTextBox.Text, emailTextBox.Text)
        resultLabel.Text <- "Contact Updated!" // Modify based on your logic
    )

    updateForm

// Form for deleting a contact
let createDeleteContactForm () =
    let deleteForm = new Form(Text = "Delete Contact", Width = 500, Height = 500, BackColor = Color.LightBlue)

    // Labels and textboxes
    let nameLabel = new Label(Text = "Name:", Top = 20, Left = 60, Font = new Font("Arial", 10.0f, FontStyle.Bold))
    let nameTextBox = new TextBox(Top = 20, Left = 150, Width = 250)

    let phoneLabel = new Label(Text = "Phone:", Top = 80, Left = 60, Font = new Font("Arial", 10.0f, FontStyle.Bold))
    let phoneTextBox = new TextBox(Top = 80, Left = 150, Width = 250)

    let deleteButton = new Button(Text = "Delete Contact", Top = 150, Left = 160, Width = 200, BackColor = Color.LightCoral, Font = new Font("Arial", 10.0f, FontStyle.Bold))
    let resultLabel = new Label(Top = 200, Left = 80, Width = 350, Height = 180, BorderStyle = BorderStyle.FixedSingle, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 12.0f, FontStyle.Bold))

    // Add controls
    deleteForm.Controls.AddRange [| nameLabel; nameTextBox; phoneLabel; phoneTextBox;  deleteButton; resultLabel |]

    // Event for delete button (Logic for deleting a contact will go here)
    deleteButton.Click.Add(fun _ -> 
        // Call your delete contact logic here
        // Example: deleteContact(idTextBox.Text)
        resultLabel.Text <- "Contact Deleted!" // Modify based on your logic
    )

    deleteForm

// Main Form
[<EntryPoint>]
let main argv =
    let form = new Form(Text = "Contact Management System", Width = 600, Height = 600, BackColor = Color.WhiteSmoke)

    // Function to center buttons
    let centerButton (button: Button) = button.Left <- (form.ClientSize.Width - button.Width) / 2

    // Buttons
    let addButton = new Button(Text = "Add Contact", Top = 50, Width = 200, BackColor = Color.LightSkyBlue, Font = new Font("Arial", 10.0f, FontStyle.Bold))
    let updateButton = new Button(Text = "Update Contact", Top = 120, Width = 200, BackColor = Color.Khaki, Font = new Font("Arial", 10.0f, FontStyle.Bold))
    let deleteButton = new Button(Text = "Delete Contact", Top = 190, Width = 200, BackColor = Color.Salmon, Font = new Font("Arial", 10.0f, FontStyle.Bold))
    let searchButton = new Button(Text = "Search Contact", Top = 260, Width = 200, BackColor = Color.LightGreen, Font = new Font("Arial", 10.0f, FontStyle.Bold))
    let displayButton = new Button(Text = "Display Contacts", Top = 330, Width = 200, BackColor = Color.LightSkyBlue, Font = new Font("Arial", 10.0f, FontStyle.Bold))
    let exitButton = new Button(Text = "Exit", Top = 400, Width = 200, BackColor = Color.LightCoral, Font = new Font("Arial", 10.0f, FontStyle.Bold))

    // Center the buttons
    centerButton addButton
    centerButton updateButton
    centerButton deleteButton
    centerButton searchButton
    centerButton displayButton
    centerButton exitButton

    // Add buttons to form
    form.Controls.AddRange [| addButton; updateButton; deleteButton; searchButton; displayButton; exitButton |]

    // Button events (Logic for button clicks)
    updateButton.Click.Add(fun _ -> createUpdateContactForm().ShowDialog() |> ignore)
    deleteButton.Click.Add(fun _ -> createDeleteContactForm().ShowDialog() |> ignore)
    displayButton.Click.Add(fun _ -> 
        // Call your display contacts logic here
        // Example: displayAllContacts()
        MessageBox.Show("Display Contacts clicked!", "Info") |> ignore
    )
    exitButton.Click.Add(fun _ -> form.Close())

    // Keep buttons centered on resize
    form.Resize.Add(fun _ -> 
        centerButton addButton
        centerButton updateButton
        centerButton deleteButton
        centerButton searchButton
        centerButton displayButton
        centerButton exitButton
    )

    // Run the app
    Application.Run(form)
    0
