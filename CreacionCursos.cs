using GestionAsignaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace Cursos
{
    public partial class CreacionCursos : Form
    {
        private string connectionString = "Data Source=LAPTOP-SANTIV\\SQLDEVELOPER;Initial Catalog=Asignaciones;Integrated Security=True;";
        private bool isModifyMode = false;
        private bool isDeleteMode = false;
        private string codigoCursoSeleccionado = "";

        public CreacionCursos()
        {
   InitializeComponent();
    panel1.Visible = false;
    panel2.Visible = false;
    CargarCursos();
    dataGridView1.CellClick += DataGridView1_CellClick;
}

private void label1_Click(object sender, EventArgs e)
{

}

private void button1_Click(object sender, EventArgs e)
{

    panel1.Visible = true;
    panel2.Visible = false;
    isModifyMode = false;
    isDeleteMode = false;
    LimpiarControles();
    QuitarColumnaCheckbox();
}


private void button2_Click(object sender, EventArgs e)
{
    panel1.Visible = true;
    isModifyMode = true;
    panel2.Visible = false;
    isDeleteMode = false;
    QuitarColumnaCheckbox();

    if (dataGridView1.SelectedRows.Count > 0)
    {
        DataGridViewRow row = dataGridView1.SelectedRows[0];
        codigoCursoSeleccionado = row.Cells["codigo"].Value.ToString();
        textBox1.Visible = false; 
        label2.Visible = false; 
    }
    else
    {
        MessageBox.Show("Seleccione un curso de la lista para modificar.");
    }
}


private void DELETE_Click(object sender, EventArgs e)
{
    panel1.Visible = false;
    isDeleteMode = true;

    if (!dataGridView1.Columns.Contains("checkBoxColumn"))
    {
        DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
        checkBoxColumn.HeaderText = "Seleccionar";
        checkBoxColumn.Name = "checkBoxColumn";
        dataGridView1.Columns.Insert(0, checkBoxColumn);
        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
    }

    MessageBox.Show("Seleccione los cursos que desea eliminar marcando los checkboxes en la primera columna.", "Instrucciones", MessageBoxButtons.OK, MessageBoxIcon.Information);
    button3.Visible = true;
    panel2.Visible = true;
}

private void QuitarColumnaCheckbox()
{
    if (dataGridView1.Columns.Contains("checkBoxColumn"))
    {
        dataGridView1.Columns.Remove("checkBoxColumn");
    }
}




private void panelcursos(object sender, PaintEventArgs e)
{

}

private void label2_Click(object sender, EventArgs e)
{

}

private void label3_Click(object sender, EventArgs e)
{

}

private void label5_Click(object sender, EventArgs e)
{

}

private void txtCodigo(object sender, EventArgs e)
{

}

private void txtNombreCurso(object sender, EventArgs e)
{

}

private void txtGrupo(object sender, EventArgs e)
{

}

private void limiteEstudiantes(object sender, EventArgs e)
{

}

private void btnGuardar_Click(object sender, EventArgs e)
{
    string codigo = textBox1.Text;
    string nombreCurso = textBox2.Text;
    string grupo = textBox3.Text;
    int limiteEstudiantes = (int)numericUpDown1.Value;
    

    if (string.IsNullOrEmpty(codigo) || string.IsNullOrEmpty(nombreCurso) || string.IsNullOrEmpty(grupo) || limiteEstudiantes == 0)
    {
        MessageBox.Show("Todos los campos son obligatorios. Por favor, complete todos los campos.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
    }
    int numeroDeCursos = ContarCursosEnBaseDeDatos();
    if (numeroDeCursos >= 10)
    {
        MessageBox.Show("Llegó al limite de cursos creados", "Límite Alcanzado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
    }



    if (limiteEstudiantes < 10 || limiteEstudiantes > 30)
    {
        MessageBox.Show("El rango de estudiantes es entre 10 y 30", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
    }


    if (CodigoCursoEnUso(codigo))
    {
        MessageBox.Show("Codigo ya en uso");
        return;
    }

    GuardarCursoEnBaseDeDatos(codigo, nombreCurso, grupo, limiteEstudiantes);

    LimpiarControles();
    panel1.Visible = false;
    isModifyMode = false;

    Abrir Canva_Main y cargar los datos
    Canva_Main canvaMain = new Canva_Main();
    canvaMain.Show();
    canvaMain.CargarDatos();
}

private bool CodigoCursoEnUso(string codigo)
{
    string query = isModifyMode ?
        "SELECT COUNT(*) FROM Cursos WHERE codigo = @Codigo AND codigo <> @CodigoCursoSeleccionado" :
        "SELECT COUNT(*) FROM Cursos WHERE codigo = @Codigo";

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Codigo", codigo);

        if (isModifyMode)
        {
            command.Parameters.AddWithValue("@CodigoCursoSeleccionado", codigoCursoSeleccionado);
        }

        try
        {
            connection.Open();
            int count = (int)command.ExecuteScalar();
            return count > 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al verificar el código del curso: " + ex.Message);
            return false;
        }
    }
}
private int ContarCursosEnBaseDeDatos()
{
    string query = "SELECT COUNT(*) FROM Cursos";

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        SqlCommand command = new SqlCommand(query, connection);

        try
        {
            connection.Open();
            int count = (int)command.ExecuteScalar();
            return count;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al contar los cursos en la base de datos: " + ex.Message);
            return 0; 
        }
    }
}



private void btnCancelar_Click(object sender, EventArgs e)
{

    LimpiarControles();
    panel1.Visible = false;
    isModifyMode = false;
}

private void LimpiarControles()
{
    textBox1.Text = "";
    textBox2.Text = "";
    textBox3.Text = "";
    numericUpDown1.Value = 0;
}
private void GuardarCursoEnBaseDeDatos(string codigo, string nombreCurso, string grupo, int limiteEstudiantes)
{
    
    if (limiteEstudiantes < 10 || limiteEstudiantes > 30)
    {
        MessageBox.Show("El rango de estudiantes es entre 10 y 30");
        return;
    }

  
    if (CodigoCursoEnUso(codigo))
    {
        MessageBox.Show("Código ya en uso.");
        return;
    }

    
    string query = isModifyMode ?
        "UPDATE Cursos SET nombre_del_curso = @NombreCurso, grupo = @Grupo, limite_de_estudiantes = @LimiteEstudiantes WHERE codigo = @Codigo" :
        "INSERT INTO Cursos (codigo, nombre_del_curso, grupo, limite_de_estudiantes) VALUES (@Codigo, @NombreCurso, @Grupo, @LimiteEstudiantes)";

   
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Codigo", codigo);
        command.Parameters.AddWithValue("@NombreCurso", nombreCurso);
        command.Parameters.AddWithValue("@Grupo", grupo);
        command.Parameters.AddWithValue("@LimiteEstudiantes", limiteEstudiantes);

        try
        {
            connection.Open();
            int recordsAffected = command.ExecuteNonQuery();

           
            string mensaje = isModifyMode ? "El curso se ha modificado correctamente." : "El curso se ha creado correctamente.";
            MessageBox.Show(mensaje, "Operación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);

            
            CargarCursos();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al {(isModifyMode ? "modificar" : "guardar")} el curso en la base de datos: " + ex.Message);
        }
    }
}


private void CargarCursos()
{
    string query = "SELECT codigo, nombre_del_curso AS NombreCurso, grupo, limite_de_estudiantes AS LimiteEstudiantes FROM Cursos";

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
        DataTable dataTable = new DataTable();

        try
        {
            connection.Open();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
            dataGridView1.Refresh();
        }
        catch (Exception)
        {
            MessageBox.Show("Error al cargar cursos: ");
        }
    }
}


private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
{
    if (isModifyMode && e.RowIndex >= 0)
    {
        DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
        codigoCursoSeleccionado = row.Cells["codigo"].Value.ToString();

        string query = "SELECT codigo, nombre_del_curso, grupo, limite_de_estudiantes FROM Cursos WHERE codigo = @Codigo";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Codigo", codigoCursoSeleccionado);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    textBox1.Text = reader["codigo"].ToString();
                    textBox2.Text = reader["nombre_del_curso"].ToString();
                    textBox3.Text = reader["grupo"].ToString();
                    numericUpDown1.Value = Convert.ToInt32(reader["limite_de_estudiantes"]);

                    panel1.Visible = true;
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos del curso: " + ex.Message);
            }
        }
    }
}
private void EliminarCursosSeleccionados()
{
    List<string> cursosAEliminar = new List<string>();

    foreach (DataGridViewRow row in dataGridView1.Rows)
    {
        if (row.Cells["checkBoxColumn"] is DataGridViewCheckBoxCell chkCell && Convert.ToBoolean(chkCell.Value))
        {
            string codigoCurso = row.Cells["codigo"].Value.ToString();
            cursosAEliminar.Add(codigoCurso);
        }
    }

    foreach (string codigoCurso in cursosAEliminar)
    {
        EliminarCursoEnBaseDeDatos(codigoCurso);
    }

    CargarCursos();
}



private void EliminarCursoEnBaseDeDatos(string codigoCurso)
{
    string query = "DELETE FROM Cursos WHERE codigo = @Codigo";

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Codigo", codigoCurso);

        try
        {
            connection.Open();
            int recordsAffected = command.ExecuteNonQuery();
            MessageBox.Show($"Curso con código {codigoCurso} eliminado ");
        }
        catch (Exception)
        {
            MessageBox.Show($"Error al eliminar curso con código {codigoCurso} ");
        }
    }
}
private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
{
    if (e.ColumnIndex == 0 && e.RowIndex >= 0)
    {
        DataGridViewCheckBoxCell checkBoxCell = (DataGridViewCheckBoxCell)dataGridView1[e.ColumnIndex, e.RowIndex];
        bool isChecked = (bool)checkBoxCell.Value;

        string codigoCurso = dataGridView1.Rows[e.RowIndex].Cells["codigo"].Value.ToString();
        string nombreCurso = dataGridView1.Rows[e.RowIndex].Cells["NombreCurso"].Value.ToString();

        DialogResult result = MessageBox.Show($"¿Estás seguro que deseas eliminar el curso '{nombreCurso}'?", "Confirmar Eliminación", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

        if (result == DialogResult.OK)
        {
            EliminarCurso(codigoCurso);
            CargarCursos();
        }
        else
        {
            checkBoxCell.Value = !isChecked;
        }
    }
}

private void EliminarCurso(string codigoCurso)
{
    string query = "DELETE FROM Cursos WHERE codigo = @Codigo";

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Codigo", codigoCurso);

        try
        {
            connection.Open();
            int recordsAffected = command.ExecuteNonQuery();
            MessageBox.Show($"Curso eliminado");
        }
        catch (Exception)
        {
            MessageBox.Show("Error al eliminar curso ");
        }
    }
}


private void button3_Click(object sender, EventArgs e)
{
    if (isDeleteMode)
    {
        EliminarCursosSeleccionados();
        isDeleteMode = false;
        QuitarColumnaCheckbox();
        panel2.Visible = false;
        button3.Visible = false;
    }
}

private void panel2_Paint(object sender, PaintEventArgs e)
{

}

private void CreacionCursos_Load(object sender, EventArgs e)
{
    Canva_Main cm = new Canva_Main();
    cm.Show();
    this.Hide();
}
