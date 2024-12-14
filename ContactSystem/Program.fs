module program.fs
open System
open System.Windows.Forms

// Form for adding a new contact
let createAddContactForm() =
    // Create a new form for adding contact
    let addForm = new Form(Text = "Add New Contact", Width = 500, Height = 500)

    // Labels for the input fields
    let nameLabel = new Label(Text = "Name:", Top = 20, Left = 30)
    let phoneLabel = new Label(Text = "Phone Number:", Top = 60, Left = 30)
    let emailLabel = new Label(Text = "Email:", Top = 100, Left = 30)

    // Text boxes for the user to input data
    let nameTextBox = new TextBox(Top = 20, Left = 150, Width = 200)
    let phoneTextBox = new TextBox(Top = 60, Left = 150, Width = 200)
    let emailTextBox = new TextBox(Top = 100, Left = 150, Width = 200)

    // Button to save the contact
    let saveButton = new Button(Text = "Save Contact", Top = 140, Left = 150, Width = 200)

    // Label for displaying the result (optional)
    let resultLabel = new Label(Top = 180, Left = 30, Width = 340, Height = 60, BorderStyle = BorderStyle.FixedSingle)

    // Add controls to the form
    addForm.Controls.Add(nameLabel)
    addForm.Controls.Add(phoneLabel)
    addForm.Controls.Add(emailLabel)
    addForm.Controls.Add(nameTextBox)
    addForm.Controls.Add(phoneTextBox)
    addForm.Controls.Add(emailTextBox)
    addForm.Controls.Add(saveButton)
    addForm.Controls.Add(resultLabel)

    // Event handler for the save button
    saveButton.Click.Add(fun _ ->
        // Get the entered values
        let name = nameTextBox.Text
        let phone = phoneTextBox.Text
        let email = emailTextBox.Text

        // Display a message with the contact details
        resultLabel.Text <- $"Contact Saved: Name = {name}, Phone = {phone}, Email = {email}"

        // You can add logic here to save the contact (e.g., in a list or database)
    )

    // Return the form to be displayed
    addForm

// Main Form
[<EntryPoint>]
let main argv =
    // Main form
    let form = new Form(Text = "Contact Management System", Width = 600, Height = 600)

    // Function to center a button horizontally
    let centerButton (button: Button) =
        button.Left <- (form.ClientSize.Width - button.Width) / 2

    // Buttons for actions
    let addButton = new Button(Text = "Add Contact", Top = 50, Width = 200)
    let updateButton = new Button(Text = "Update Contact", Top = 110, Width = 200)
    let deleteButton = new Button(Text = "Delete Contact", Top = 170, Width = 200)
    let searchButton = new Button(Text = "Search Contact", Top = 230, Width = 200)

    // Center the buttons horizontally
    centerButton addButton
    centerButton updateButton
    centerButton deleteButton
    centerButton searchButton

    // Add buttons to form
    form.Controls.Add(addButton)
    form.Controls.Add(updateButton)
    form.Controls.Add(deleteButton)
    form.Controls.Add(searchButton)

    // Event handler for "Add Contact" button
    addButton.Click.Add(fun _ ->
        // Show the "Add Contact" form when clicked
        let addForm = createAddContactForm()
        addForm.ShowDialog() |> ignore // Corrected to ignore the DialogResult
    )

    // Placeholder event handlers for the other buttons
    updateButton.Click.Add(fun _ -> MessageBox.Show("Update Contact clicked!") |> ignore)
    deleteButton.Click.Add(fun _ -> MessageBox.Show("Delete Contact clicked!") |> ignore)
    searchButton.Click.Add(fun _ -> MessageBox.Show("Search Contact clicked!") |> ignore)

    // Ensure buttons stay centered when resizing the form
    form.Resize.Add(fun _ ->
        centerButton addButton
        centerButton updateButton
        centerButton deleteButton
        centerButton searchButton
    )

    // Run the main application
    Application.Run(form)
    0
