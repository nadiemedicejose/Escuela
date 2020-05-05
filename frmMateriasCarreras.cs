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
    public partial class frmMateriasCarreras : Form
    {
        CarrerasQueries objCarreras = new CarrerasQueries();
        MateriasCarrerasQueries objRelMC = new MateriasCarrerasQueries();
        private String acción;
        private int CarreraID_Anterior = 0;

        public frmMateriasCarreras()
        {
            InitializeComponent();
            objCarreras.RellenarCarreras(cmbCarrera);
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
            cmbCarrera.SelectedValue = 0;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            // Botones del menu de opciones:
            // Nuevo, Actualizar, Guardar, Eliminar y Cancelar
            HabilitarBotonesMenu(0, 0, 0, 0, 1);
            btnBuscar.Enabled = true;
            txtMateriaID.Enabled = true;
            txtNombreMateria.Enabled = false;
            cmbCarrera.Enabled = false;
            acción = "nuevo";
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            HabilitarBotonesMenu(0, 0, 1, 0, 1);
            btnBuscar.Enabled = false;
            txtMateriaID.Enabled = false;
            txtNombreMateria.Enabled = false;
            CarreraID_Anterior = Convert.ToInt32(cmbCarrera.SelectedValue);
            cmbCarrera.Enabled = true;
            acción = "actualizar";
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult opcion = MessageBox.Show("¿Está seguro que desea guardar el registro?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (opcion == DialogResult.Yes)
                {
                    int CarreraID = Convert.ToInt32(cmbCarrera.SelectedValue);
                    int MateriaID = Convert.ToInt32(txtMateriaID.Text);

                    if (acción == "nuevo")
                    {
                        objRelMC.InsertarRelacionMateriaCarrera(CarreraID, MateriaID);
                    }
                    else if (acción == "actualizar")
                    {
                        objRelMC.ActualizarRelacionMateriasCarrera(CarreraID_Anterior, CarreraID, MateriaID);
                        CarreraID_Anterior = 0;
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
                    int CarreraID = Convert.ToInt32(cmbCarrera.SelectedValue);
                    int MateriaID = Convert.ToInt32(txtMateriaID.Text);

                    objRelMC.EliminarRelacionMateriasCarrera(CarreraID, MateriaID);
                    ActualizarTabla();
                    HabilitarBotonesMenu(1, 0, 0, 0, 0);
                    LimpiarCampos();
                    HabilitarCampos(false);
                }
                catch (Exception)
                {
                    MessageBox.Show("Selecciona el registro que deseas eliminar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                            cmbCarrera.Enabled = true;
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
                        MessageBox.Show("Número de materia no válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            cmbCarrera.Enabled = estado;
        }

        public void ActualizarTabla()
        {
            objRelMC.ObtenerMateriasCarrera(dgvRelMC);
        }

        private void BuscarSelección()
        {
            HabilitarCampos(false);
            HabilitarBotonesMenu(1, 1, 0, 1, 0);
            acción = "buscar";

            String NombreMateria = dgvRelMC.CurrentRow.Cells[1].Value.ToString();

            objRelMC.BuscarRelacionMateriasCarrera(NombreMateria, txtMateriaID, cmbCarrera);
            txtNombreMateria.Text = NombreMateria;
        }

        private void cmbCarrera_SelectionChangeCommitted(object sender, EventArgs e)
        {
            HabilitarBotonesMenu(0, 0, 1, 0, 1);
        }

        private void dgvRelMC_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            BuscarSelección();
        }
    }
}
