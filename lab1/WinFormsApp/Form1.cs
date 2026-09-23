using BusinessLogic;
using Model;

namespace WinFormsApp;

public partial class Form1 : Form
{
    private readonly Logic log = new();
    private readonly DataGridView tab = new();
    private readonly Panel bar = new();
    private readonly Label msg = new();

    public Form1()
    {
        InitializeComponent();
        MakeUi();
        Fill();
    }

    private void MakeUi()
    {
        Text = "DecanatPRO";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(700, 500);
        ClientSize = new Size(900, 650);

        TableLayoutPanel root = new();
        root.Dock = DockStyle.Fill;
        root.Padding = new Padding(12);
        root.RowCount = 2;
        root.ColumnCount = 1;
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 58));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 42));
        Controls.Add(root);

        TableLayoutPanel top = new();
        top.Dock = DockStyle.Fill;
        top.RowCount = 2;
        top.ColumnCount = 1;
        top.RowStyles.Add(new RowStyle(SizeType.Absolute, 46));
        top.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.Controls.Add(top, 0, 0);

        FlowLayoutPanel tools = new();
        tools.Dock = DockStyle.Fill;
        tools.FlowDirection = FlowDirection.LeftToRight;
        tools.WrapContents = false;
        top.Controls.Add(tools, 0, 0);

        Button add = new() { Text = "Добавить", AutoSize = true, Height = 32 };
        Button del = new() { Text = "Удалить", AutoSize = true, Height = 32 };
        Button refBtn = new() { Text = "Обновить", AutoSize = true, Height = 32 };
        add.Click += AddClick;
        del.Click += DelClick;
        refBtn.Click += (s, e) => Fill();
        tools.Controls.Add(add);
        tools.Controls.Add(del);
        tools.Controls.Add(refBtn);

        tab.Dock = DockStyle.Fill;
        tab.AllowUserToAddRows = false;
        tab.AllowUserToDeleteRows = false;
        tab.AllowUserToResizeRows = false;
        tab.ReadOnly = true;
        tab.MultiSelect = false;
        tab.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        tab.RowHeadersVisible = false;
        tab.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        tab.Columns.Add("name", "ФИО");
        tab.Columns.Add("spec", "Направление");
        tab.Columns.Add("group", "Группа");
        top.Controls.Add(tab, 0, 1);

        GroupBox box = new() { Text = "Студенты по направлениям", Dock = DockStyle.Fill, Padding = new Padding(8) };
        root.Controls.Add(box, 0, 1);
        bar.Dock = DockStyle.Fill;
        bar.Paint += BarPaint;
        box.Controls.Add(bar);
        msg.Text = "Нет данных для гистограммы";
        msg.TextAlign = ContentAlignment.MiddleCenter;
        msg.Dock = DockStyle.Fill;
        msg.Visible = false;
        bar.Controls.Add(msg);
    }

    private void Fill()
    {
        List<Student> all = log.GetAll();
        tab.Rows.Clear();
        foreach (Student st in all)
        {
            tab.Rows.Add(st.Name, st.Speciality, st.Group);
        }

        msg.Visible = all.Count == 0;
        bar.Invalidate();
    }

    private void AddClick(object? sender, EventArgs e)
    {
        using AddForm form = new();
        if (form.ShowDialog(this) == DialogResult.OK && form.Student is not null)
        {
            if (log.Add(form.Student))
            {
                Fill();
            }
            else
            {
                MessageBox.Show("Заполните ФИО, направление и группу.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    private void DelClick(object? sender, EventArgs e)
    {
        if (tab.CurrentRow is null || tab.CurrentRow.Index < 0)
        {
            MessageBox.Show("Выберите студента для удаления.", "Нет выбора", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (log.Remove(tab.CurrentRow.Index))
        {
            Fill();
        }
    }

    private void BarPaint(object? sender, PaintEventArgs e)
    {
        Dictionary<string, int> data = log.GetCounts();
        if (data.Count == 0)
        {
            return;
        }

        Graphics g = e.Graphics;
        int left = 48;
        int top = 22;
        int baseY = bar.ClientSize.Height - 42;
        int areaH = baseY - top;
        int areaW = bar.ClientSize.Width - left - 20;
        int max = data.Values.Max();
        int step = areaW / data.Count;
        int wide = Math.Max(12, Math.Min(55, step / 2));
        int i = 0;

        g.DrawLine(Pens.Gray, left, top, left, baseY);
        g.DrawLine(Pens.Gray, left, baseY, left + areaW, baseY);
        foreach (KeyValuePair<string, int> item in data.OrderBy(x => x.Key))
        {
            int h = Math.Max(1, areaH * item.Value / max);
            int x = left + i * step + (step - wide) / 2;
            int y = baseY - h;
            g.FillRectangle(Brushes.SteelBlue, x, y, wide, h);
            g.DrawString(item.Value.ToString(), Font, Brushes.Black, x, Math.Max(top, y - 18));
            string name = item.Key;
            SizeF size = g.MeasureString(name, Font);
            g.DrawString(name, Font, Brushes.Black, x + wide / 2 - size.Width / 2, baseY + 5);
            i++;
        }
    }
}
