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
    public partial class frmMaestros : Form
    {
        MaestrosQueries objMaestro = new MaestrosQueries();
        private String acción;

        public frmMaestros()
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
            txtMaestroID.Text = null;
            txtNombreMaestro.Text = null;
            txtDirección.Text = null;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            // Botones del menu de opciones:
            // Nuevo, Actualizar, Guardar, Eliminar y Cancelar
            HabilitarBotonesMenu(0, 0, 0, 0, 1);
            btnBuscar.Enabled = false;
            txtMaestroID.Enabled = false;
            txtNombreMaestro.Enabled = true;
            txtDirección.Enabled = true;
            acción = "nuevo";
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            HabilitarBotonesMenu(0, 0, 1, 0, 1);
            btnBuscar.Enabled = false;
            txtMaestroID.Enabled = false;
            txtNombreMaestro.Enabled = true;
            txtDirección.Enabled = true;
            acción = "actualizar";
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult opcion = MessageBox.Show("¿Está seguro que desea guardar el registro?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (opcion == DialogResult.Yes)
                {
                    string NombreMaestro = txtNombreMaestro.Text;
                    string Dirección = txtDirección.Text;

                    if (acción == "nuevo")
                    {
                        objMaestro.InsertarMaestro(NombreMaestro, Dirección);
                    }
                    else if (acción == "actualizar")
                    {
                        int MaestroID = Convert.ToInt32(dgvMaestros.Rows[dgvMaestros.CurrentRow.Index].Cells[0].Value);
                        objMaestro.ActualizarMaestro(MaestroID, NombreMaestro, Dirección);
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
                    int MaestroID = Convert.ToInt32(dgvMaestros.Rows[dgvMaestros.CurrentRow.Index].Cells[0].Value);

                    objMaestro.EliminarMaestro(MaestroID);
                    ActualizarTabla();
                    HabilitarBotonesMenu(1, 0, 0, 0, 0);
                    LimpiarCampos();
                    HabilitarCampos(false);
                }
                catch (Exception)
                {
                    MessageBox.Show("Selecciona al maestro que deseas eliminar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        int MaestroID = Convert.ToInt32(txtMaestroID.Text);
                        bdEscuela.BuscarMaestro(MaestroID);

                        var Registros = from valor in bdEscuela.tblMaestros
                                        where valor.MaestroID == MaestroID
                                        select valor;
                        if (Registros.Any())
                        {
                            foreach (var maestro in Registros)
                            {
                                txtNombreMaestro.Text = maestro.NombreMaestro;

                                if (acción == "nuevo")
                                {
                                    txtDirección.Enabled = true;
                                }
                            }
                        }
                        else
                        {
                            LimpiarCampos();
                            txtDirección.Enabled = false;
                            txtMaestroID.Focus();
                            MessageBox.Show("Número de maestro no encontrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception)
                    {
                        LimpiarCampos();
                        txtMaestroID.Focus();
                        MessageBox.Show("Error al obtener el número de maestro", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ingresa el número de maestro", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMaestroID.Focus();
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
            txtMaestroID.Enabled = estado;
            txtNombreMaestro.Enabled = estado;
            txtDirección.Enabled = estado;
        }

        public void ActualizarTabla()
        {
            objMaestro.ObtenerMaestros(dgvMaestros);
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

            int MaestroID = Convert.ToInt32(dgvMaestros.CurrentRow.Cells[0].Value);
            String NombreMaestro = dgvMaestros.CurrentRow.Cells[1].Value.ToString();
            String Dirección = dgvMaestros.CurrentRow.Cells[2].Value.ToString();

            txtMaestroID.Text = MaestroID.ToString();
            txtNombreMaestro.Text = NombreMaestro;
            txtDirección.Text = Dirección;
        }

        private void DirecciónIngresada(object sender, EventArgs e)
        {
            HabilitarBotonesMenu(0, 0, 1, 0, 1);
        }
    }
}
