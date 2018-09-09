using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;

namespace MyTask
{
    public partial class TaskForm : Form
    {
        TaskContext db;
        public TaskForm()
        {
            InitializeComponent();

            db = new TaskContext();
            db.Tasks.Load();
            dataGridView1.DataSource = db.Tasks.Local.ToBindingList();
        }

        private void TaskForm_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "myTaskDBDataSet.Task". При необходимости она может быть перемещена или удалена.
            this.taskTableAdapter.Fill(this.myTaskDBDataSet.Task);

        }

        private void button1_Click(object sender, EventArgs e) //добавляем задачу
        {
            TaskEditing tForm = new TaskEditing();

            DialogResult result = tForm.ShowDialog(this);

            if (result == DialogResult.Cancel)
                return;

            Task task = new Task();
            task.Name = tForm.textBox1.Text;

            db.Tasks.Add(task);
            db.SaveChanges();

            MessageBox.Show("Новая задача добавлена.");
        }

        private void button2_Click(object sender, EventArgs e) //редактируем задачу
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int index = dataGridView1.SelectedRows[0].Index;
                int id = 0;
                bool converted = Int32.TryParse(dataGridView1[0, index].Value.ToString(), out id);
                if (converted == false)
                    return;

                Task task = db.Tasks.Find(id);

                TaskEditing tForm = new TaskEditing();
                tForm.textBox1.Text = task.Name;

                DialogResult result = tForm.ShowDialog(this);

                if (result == DialogResult.Cancel)
                    return;

                task.Name = tForm.textBox1.Text;

                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();

                MessageBox.Show("Задача обновлена.");
            }
        }

        private void button3_Click(object sender, EventArgs e) //удаляем задачу
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int index = dataGridView1.SelectedRows[0].Index;
                int id = 0;
                bool converted = Int32.TryParse(dataGridView1[0, index].Value.ToString(), out id);
                if (converted == false)
                    return;

                Task task = db.Tasks.Find(id);
                db.Tasks.Remove(task);
                db.SaveChanges();

                MessageBox.Show("Задача удалена.");
            }
        }
    }
}
