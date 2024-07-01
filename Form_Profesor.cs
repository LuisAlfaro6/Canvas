﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestionAsignaciones
{
    public partial class Form_Profesor : Form
    {
        public Form_Profesor()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection(@"Data Source=LAPTOP-SANTIV\SQLDEVELOPER;Initial Catalog=Asignaciones;Integrated Security=True");


        private void logInProfesor_button_Click(object sender, EventArgs e)
        {
            String username_Profesor, password_Profesor;

            username_Profesor = usuarioProf_text.Text;
            password_Profesor = contrasenaProf_text.Text;

            try
            {
                String querry = "SELECT * FROM logIn_profesor where username_Profesor= '" + usuarioProf_text.Text + "' AND password_Profesor= '" + contrasenaProf_text.Text + "'";
                SqlDataAdapter sda = new SqlDataAdapter(querry, conn);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    username_Profesor = usuarioProf_text.Text;
                    password_Profesor = contrasenaProf_text.Text;

                    //Metodo para agregar la otra pantalla

                    Canva_Main Canva_Main = new Canva_Main();
                    Canva_Main.Show();
                    this.Hide();

                }
                else
                {

                    MessageBox.Show("Las credenciales ingresadas son invalidad, por favor verifique que los datos ingresados sean correctos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    usuarioProf_text.Clear();
                    contrasenaProf_text.Clear();

                    usuarioProf_text.Focus();

                }
            }
            catch
            {
                MessageBox.Show("Error");
            }
            finally
            {
                conn.Close();
            }
        }

        private void usuarioProf_text_TextChanged(object sender, EventArgs e)
        {

        }

        private void atrasProfesor_text_Click(object sender, EventArgs e)
        {
                        Form_Principal principal = new Form_Principal();
            principal.Show();
            this.Hide();
        }
    }
}