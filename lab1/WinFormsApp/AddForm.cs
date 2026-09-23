using Model;

namespace WinFormsApp;

public class AddForm : Form
{
    private readonly TextBox name = new();
    private readonly TextBox spec = new();
    private readonly TextBox group = new();
    public Student? Student { get; private set; }

    public AddForm()
    {
        Text = "Добавить студента";
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        ClientSize = new Size(420, 210);

        TableLayoutPanel grid = new();
        grid.Dock = DockStyle.Fill;
        grid.Padding = new Padding(12);
        grid.ColumnCount = 2;
        grid.RowCount = 4;
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145));
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        grid.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
        grid.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
        grid.RowStyles.Add(new RowStyle(SizeType.Percent, 34));
        grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
        Controls.Add(grid);

        AddRow(grid, "ФИО", name, 0);
        AddRow(grid, "Направление подготовки", spec, 1);
        AddRow(grid, "Группа", group, 2);

        FlowLayoutPanel buttons = new() { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
        Button save = new() { Text = "Сохранить", AutoSize = true };
        Button cancel = new() { Text = "Отмена", AutoSize = true, DialogResult = DialogResult.Cancel };
        save.Click += SaveClick;
        buttons.Controls.Add(cancel);
        buttons.Controls.Add(save);
        grid.Controls.Add(buttons, 0, 3);
        grid.SetColumnSpan(buttons, 2);
        AcceptButton = save;
        CancelButton = cancel;
    }

    private static void AddRow(TableLayoutPanel grid, string title, TextBox box, int row)
    {
        Label label = new() { Text = title, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill };
        box.Dock = DockStyle.Fill;
        grid.Controls.Add(label, 0, row);
        grid.Controls.Add(box, 1, row);
    }

    private void SaveClick(object? sender, EventArgs e)
    {
        Student = new Student { Name = name.Text, Speciality = spec.Text, Group = group.Text };
        DialogResult = DialogResult.OK;
        Close();
    }
}
