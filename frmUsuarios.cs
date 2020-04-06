using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Escuela
{
    public partial class frmUsuarios : Form
    {
        UsuariosQueries objUsuario = new UsuariosQueries();
        private String acción;

        public frmUsuarios()
        {
            InitializeComponent();
            ActualizarTabla();
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            if (MenuOpciones.Visible)
            {
                btnMenu.Image = Properties.Resources.more;
                MenuOpciones.Visible = false;
            }
            else
            {
                btnMenu.Image = Properties.Resources.more2;
                MenuOpciones.Visible = true;
            }
        }

        public void LimpiarCampos()
        {
            txtUsuarioID.Text = null;
            txtNombreUsuario.Text = null;
            txtContraseña.Text = null;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            // Botones del menu de opciones:
            // Nuevo, Actualizar, Guardar, Eliminar y Cancelar
            HabilitarBotonesMenu(0, 0, 0, 0, 1);
            btnBuscar.Enabled = false;
            txtUsuarioID.Enabled = false;
            txtNombreUsuario.Enabled = true;
            txtContraseña.Enabled = true;
            acción = "nuevo";
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            HabilitarBotonesMenu(0, 0, 1, 0, 1);
            btnBuscar.Enabled = false;
            txtUsuarioID.Enabled = false;
            txtNombreUsuario.Enabled = true;
            txtContraseña.Enabled = true;
            acción = "actualizar";
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult opcion = MessageBox.Show("¿Está seguro que desea guardar el registro?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (opcion == DialogResult.Yes)
                {
                    string NombreUsuario = txtNombreUsuario.Text;
                    string Contraseña = txtContraseña.Text;

                    if (acción == "nuevo")
                    {
                        objUsuario.InsertarUsuario(NombreUsuario, Contraseña);
                    }
                    else if (acción == "actualizar")
                    {
                        int UsuarioID = Convert.ToInt32(dgvUsuarios.Rows[dgvUsuarios.CurrentRow.Index].Cells[0].Value);
                        objUsuario.ActualizarUsuario(UsuarioID, NombreUsuario, Contraseña);
                    }

                    ActualizarTabla();
                    HabilitarBotonesMenu(1, 0, 0, 0, 0);
                    LimpiarCampos();
                    HabilitarCampos(false);
                    acción = "guardar";
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Rellena todos los campos" + Environment.NewLine + "Error: " + error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            DialogResult opcion = MessageBox.Show("¿Está seguro que desea eliminar el registro?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (opcion == DialogResult.Yes)
            {
                try
                {
                    int UsuarioID = Convert.ToInt32(dgvUsuarios.Rows[dgvUsuarios.CurrentRow.Index].Cells[0].Value);

                    objUsuario.EliminarUsuario(UsuarioID);
                    ActualizarTabla();
                    HabilitarBotonesMenu(1, 0, 0, 0, 0);
                    LimpiarCampos();
                    HabilitarCampos(false);
                }
                catch (Exception)
                {
                    MessageBox.Show("Selecciona al usuario que deseas eliminar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                using (EscuelaDatabaseDataContext bdEscuela = new EscuelaDatabaseDataContext())
                {
                    try
                    {
                        int UsuarioID = Convert.ToInt32(txtUsuarioID.Text);
                        bdEscuela.BuscarUsuario(UsuarioID);

                        var Registros = from valor in bdEscuela.tblUsuarios
                                        where valor.UsuarioID == UsuarioID
                                        select valor;
                        if (Registros.Any())
                        {
                            foreach (var usuario in Registros)
                            {
                                txtNombreUsuario.Text = usuario.NombreUsuario;

                                if (acción == "nuevo")
                                {
                                    txtContraseña.Enabled = true;
                                }
                            }
                        }
                        else
                        {
                            LimpiarCampos();
                            txtContraseña.Enabled = false;
                            txtUsuarioID.Focus();
                            MessageBox.Show("Número de usuario no encontrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception)
                    {
                        LimpiarCampos();
                        txtUsuarioID.Focus();
                        MessageBox.Show("Error al obtener el número de usuario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ingresa el número de usuario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUsuarioID.Focus();
            }
        }

        private void btnInicio_Click(object sender, EventArgs e)
        {
            Menu ventana = new Menu();
            ventana.Show();
            this.Hide();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            switch (acción)
            {
                case "nuevo":
                    {
                        LimpiarCampos();
                        HabilitarBotonesMenu(1, 0, 0, 0, 0);
                        break;
                    }
                case "buscar":
                    {
                        HabilitarBotonesMenu(1, 1, 0, 1, 0);
                        break;
                    }
                case "actualizar":
                    {
                        BuscarSelección();
                        break;
                    }
            }
            HabilitarCampos(false);
            acción = "cancelar";
        }

        public void HabilitarBotonesMenu(int Nuevo, int Actualizar, int Guardar, int Eliminar, int Cancelar)
        {
            btnNuevo.Enabled = Convert.ToBoolean(Nuevo);
            btnActualizar.Enabled = Convert.ToBoolean(Actualizar);
            btnGuardar.Enabled = Convert.ToBoolean(Guardar);
            btnEliminar.Enabled = Convert.ToBoolean(Eliminar);
            btnCancelar.Enabled = Convert.ToBoolean(Cancelar);
        }

        public void HabilitarCampos(bool estado)
        {
            btnBuscar.Enabled = estado;
            txtUsuarioID.Enabled = estado;
            txtNombreUsuario.Enabled = estado;
            txtContraseña.Enabled = estado;
        }

        public void ActualizarTabla()
        {
            objUsuario.ObtenerUsuarios(dgvUsuarios);
        }

        private void dgvCargasAlumnos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            BuscarSelección();
        }

        private void BuscarSelección()
        {
            HabilitarCampos(false);
            HabilitarBotonesMenu(1, 1, 0, 1, 0);
            acción = "buscar";

            int MaestroID = Convert.ToInt32(dgvUsuarios.CurrentRow.Cells[0].Value);
            String NombreMaestro = dgvUsuarios.CurrentRow.Cells[1].Value.ToString();
            String Dirección = dgvUsuarios.CurrentRow.Cells[2].Value.ToString();

            txtUsuarioID.Text = MaestroID.ToString();
            txtNombreUsuario.Text = NombreMaestro;
            txtContraseña.Text = Dirección;
        }

        private void DirecciónIngresada(object sender, EventArgs e)
        {
            HabilitarBotonesMenu(0, 0, 1, 0, 1);
        }
    }
}
