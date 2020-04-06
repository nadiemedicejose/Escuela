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
    public partial class frmCargasDocentes : Form
    {
        CargasDocentesQueries objCargaDocente = new CargasDocentesQueries();
        private String acción;

        public frmCargasDocentes()
        {
            InitializeComponent();
            objCargaDocente.RellenarCarreras(cmbCarrera);
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
            cmbCarrera.Text = null;
            cmbMateria.Text = null;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            // Botones del menu de opciones:
            // Nuevo, Actualizar, Guardar, Eliminar y Cancelar
            HabilitarBotonesMenu(0, 0, 0, 0, 1);
            btnBuscar.Enabled = true;
            txtMaestroID.Enabled = true;
            acción = "nuevo";
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            HabilitarBotonesMenu(0, 0, 1, 0, 1);
            HabilitarCampos(true);
            acción = "actualizar";
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult opcion = MessageBox.Show("¿Está seguro que desea guardar el registro?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (opcion == DialogResult.Yes)
                {
                    int MaestroID = Convert.ToInt32(txtMaestroID.Text);
                    int CarreraID = Convert.ToInt32(cmbCarrera.SelectedValue);
                    int MateriaID = Convert.ToInt32(cmbMateria.SelectedValue);

                    if (acción == "nuevo")
                    {
                        objCargaDocente.InsertarCargaDocente(MaestroID, CarreraID, MateriaID);
                    }
                    else if (acción == "actualizar")
                    {
                        int CargaID = Convert.ToInt32(dgvCargasDocentes.Rows[dgvCargasDocentes.CurrentRow.Index].Cells[0].Value);
                        objCargaDocente.ActualizarCargaDocente(CargaID, MaestroID, CarreraID, MateriaID);
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
                    int CargaID = Convert.ToInt32(dgvCargasDocentes.Rows[dgvCargasDocentes.CurrentRow.Index].Cells[0].Value);

                    objCargaDocente.EliminarCargaDocente(CargaID);
                    ActualizarTabla();
                    HabilitarBotonesMenu(1, 0, 0, 0, 0);
                    LimpiarCampos();
                    HabilitarCampos(false);
                }
                catch (Exception)
                {
                    MessageBox.Show("Selecciona la carga del docente que deseas eliminar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                                    HabilitarCampos(true);
                                }
                            }
                        }
                        else
                        {
                            LimpiarCampos();
                            cmbMateria.Enabled = false;
                            txtMaestroID.Focus();
                            MessageBox.Show("Número de maestro no encontrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception)
                    {
                        LimpiarCampos();
                        cmbMateria.Enabled = false;
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
            cmbCarrera.Enabled = estado;
            cmbMateria.Enabled = estado;
        }

        private void CargarMaterias_SelectedValueChanged(object sender, EventArgs e)
        {
            if (acción == "nuevo" || acción == "actualizar")
            {
                try
                {
                    int CarreraID = Convert.ToInt32(cmbCarrera.SelectedValue);
                    objCargaDocente.ObtenerMateriasPorCarrera(CarreraID, cmbMateria);
                    cmbMateria.Enabled = true;
                }
                catch (Exception)
                {
                    MessageBox.Show("Error al cargar materias", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void ActualizarTabla()
        {
            objCargaDocente.ObtenerCargasDocentes(dgvCargasDocentes);
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

            String NombreMaestro = dgvCargasDocentes.CurrentRow.Cells[1].Value.ToString();
            String NombreCarrera = dgvCargasDocentes.CurrentRow.Cells[2].Value.ToString();
            String NombreMateria = dgvCargasDocentes.CurrentRow.Cells[3].Value.ToString();

            txtNombreMaestro.Text = NombreMaestro;
            objCargaDocente.BuscarMaestroID(NombreMaestro, txtMaestroID);
            objCargaDocente.BuscarCarreraID(NombreCarrera, cmbCarrera);
            int CarreraID = Convert.ToInt32(cmbCarrera.SelectedValue);
            objCargaDocente.BuscarMateriaID(CarreraID, NombreMateria, cmbMateria);
        }

        private void cmbMateria_SelectionChangeCommitted_1(object sender, EventArgs e)
        {
            HabilitarBotonesMenu(0, 0, 1, 0, 1);
        }
    }
}
