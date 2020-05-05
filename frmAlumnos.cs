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
    public partial class frmAlumnos : Form
    {
        AlumnosQueries objAlumno = new AlumnosQueries();
        private String acción;

        public frmAlumnos()
        {
            InitializeComponent();
            objAlumno.RellenarCarreras(cmbCarrera);
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
            txtAlumnoID.Text = null;
            txtNombreAlumno.Text = null;
            cmbCarrera.Text = null;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            // Botones del menu de opciones:
            // Nuevo, Actualizar, Guardar, Eliminar y Cancelar
            HabilitarBotonesMenu(0, 0, 0, 0, 1);
            btnBuscar.Enabled = false;
            txtAlumnoID.Enabled = false;
            txtNombreAlumno.Enabled = true;
            cmbCarrera.Enabled = true;
            acción = "nuevo";
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            HabilitarBotonesMenu(0, 0, 1, 0, 1);
            btnBuscar.Enabled = false;
            txtAlumnoID.Enabled = false;
            txtNombreAlumno.Enabled = true;
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
                    string NombreAlumno = txtNombreAlumno.Text;
                    int CarreraID = Convert.ToInt32(cmbCarrera.SelectedValue);

                    if (acción == "nuevo")
                    {
                        objAlumno.InsertarAlumno(NombreAlumno, CarreraID);
                    }
                    else if (acción == "actualizar")
                    {
                        int AlumnoID = Convert.ToInt32(dgvAlumnos.Rows[dgvAlumnos.CurrentRow.Index].Cells[0].Value);
                        objAlumno.ActualizarAlumno(AlumnoID, NombreAlumno, CarreraID);
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
                    int AlumnoID = Convert.ToInt32(dgvAlumnos.Rows[dgvAlumnos.CurrentRow.Index].Cells[0].Value);

                    objAlumno.EliminarAlumno(AlumnoID);
                    ActualizarTabla();
                    HabilitarBotonesMenu(1, 0, 0, 0, 0);
                    LimpiarCampos();
                    HabilitarCampos(false);
                }
                catch (Exception)
                {
                    MessageBox.Show("Selecciona al alumno deseas eliminar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        int AlumnoID = Convert.ToInt32(txtAlumnoID.Text);
                        bdEscuela.BuscarAlumno(AlumnoID);

                        var Registros = from valor in bdEscuela.tblAlumnos
                                        where valor.AlumnoID == AlumnoID
                                        select valor;
                        if (Registros.Any())
                        {
                            foreach (var alumno in Registros)
                            {
                                txtNombreAlumno.Text = alumno.NombreAlumno;

                                if (acción == "nuevo")
                                {
                                    cmbCarrera.Enabled = true;
                                }
                            }
                        }
                        else
                        {
                            LimpiarCampos();
                            cmbCarrera.Enabled = false;
                            txtAlumnoID.Focus();
                            MessageBox.Show("Número de alumno no encontrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception)
                    {
                        LimpiarCampos();
                        txtAlumnoID.Focus();
                        MessageBox.Show("Error al obtener el número de alumno", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ingresa el número de Alumno", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAlumnoID.Focus();
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
            txtAlumnoID.Enabled = estado;
            txtNombreAlumno.Enabled = estado;
            cmbCarrera.Enabled = estado;
        }

        private void CargarMaterias_SelectedValueChanged(object sender, EventArgs e)
        {
            if (acción == "nuevo" || acción == "actualizar")
            {
                try
                {
                    int CarreraID = Convert.ToInt32(cmbCarrera.SelectedValue);
                }
                catch (Exception)
                {
                    MessageBox.Show("Error al cargar materias", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void ActualizarTabla()
        {
            objAlumno.ObtenerAlumnos(dgvAlumnos);
        }

        private void BuscarSelección()
        {
            HabilitarCampos(false);
            HabilitarBotonesMenu(1, 1, 0, 1, 0);
            acción = "buscar";

            String NombreAlumno = dgvAlumnos.CurrentRow.Cells[1].Value.ToString();
            String NombreCarrera = dgvAlumnos.CurrentRow.Cells[2].Value.ToString();

            txtNombreAlumno.Text = NombreAlumno;
            objAlumno.BuscarAlumnoID(NombreAlumno, txtAlumnoID);
            objAlumno.BuscarCarreraID(NombreCarrera, cmbCarrera);
        }

        private void cmbCarrera_SelectionChangeCommitted(object sender, EventArgs e)
        {
            HabilitarBotonesMenu(0, 0, 1, 0, 1);
        }

        private void dgvAlumnos_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            BuscarSelección();
        }
    }
}
