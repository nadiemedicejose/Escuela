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
    public partial class frmMaterias : Form
    {
        MateriasQueries objMaterias = new MateriasQueries();
        private String acción;

        public frmMaterias()
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
            txtMateriaID.Text = null;
            txtNombreMateria.Text = null;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            // Botones del menu de opciones:
            // Nuevo, Actualizar, Guardar, Eliminar y Cancelar
            HabilitarBotonesMenu(0, 0, 0, 0, 1);
            btnBuscar.Enabled = false;
            txtMateriaID.Enabled = false;
            txtNombreMateria.Enabled = true;
            acción = "nuevo";
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            HabilitarBotonesMenu(0, 0, 1, 0, 1);
            btnBuscar.Enabled = false;
            txtMateriaID.Enabled = false;
            txtNombreMateria.Enabled = true;
            acción = "actualizar";
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult opcion = MessageBox.Show("¿Está seguro que desea guardar el registro?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (opcion == DialogResult.Yes)
                {
                    string NombreMateria = txtNombreMateria.Text;

                    if (acción == "nuevo")
                    {
                        objMaterias.InsertarMateria(NombreMateria);
                    }
                    else if (acción == "actualizar")
                    {
                        int MateriaID = Convert.ToInt32(dgvMaterias.Rows[dgvMaterias.CurrentRow.Index].Cells[0].Value);
                        objMaterias.ActualizarMateria(MateriaID, NombreMateria);
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
                    int MateriaID = Convert.ToInt32(dgvMaterias.Rows[dgvMaterias.CurrentRow.Index].Cells[0].Value);

                    objMaterias.EliminarMateria(MateriaID);
                    ActualizarTabla();
                    HabilitarBotonesMenu(1, 0, 0, 0, 0);
                    LimpiarCampos();
                    HabilitarCampos(false);
                }
                catch (Exception)
                {
                    MessageBox.Show("Selecciona la materia que deseas eliminar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        int MateriaID = Convert.ToInt32(txtMateriaID.Text);

                        var Registros = from valor in bdEscuela.tblMaterias
                                        where valor.MateriaID == MateriaID
                                        select valor;
                        if (Registros.Any())
                        {
                            foreach (var materia in Registros)
                            {
                                txtNombreMateria.Text = materia.NombreMateria;
                            }
                        }
                        else
                        {
                            LimpiarCampos();
                            txtMateriaID.Focus();
                            MessageBox.Show("Número de materia no encontrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception)
                    {
                        LimpiarCampos();
                        txtMateriaID.Focus();
                        MessageBox.Show("Error al obtener el número de materia", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ingresa el número de materia", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMateriaID.Focus();
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
            txtMateriaID.Enabled = estado;
            txtNombreMateria.Enabled = estado;
        }

        public void ActualizarTabla()
        {
            objMaterias.ObtenerMaterias(dgvMaterias);
        }

        private void BuscarSelección()
        {
            HabilitarCampos(false);
            HabilitarBotonesMenu(1, 1, 0, 1, 0);
            acción = "buscar";

            int MateriaID = Convert.ToInt32(dgvMaterias.CurrentRow.Cells[0].Value);
            String NombreMateria = dgvMaterias.CurrentRow.Cells[1].Value.ToString();

            txtMateriaID.Text = MateriaID.ToString();
            txtNombreMateria.Text = NombreMateria;
        }

        private void txtNombreMateria_TextChanged(object sender, EventArgs e)
        {
            HabilitarBotonesMenu(0, 0, 1, 0, 1);
        }

        private void dgvMaterias_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            BuscarSelección();
        }
    }
}
