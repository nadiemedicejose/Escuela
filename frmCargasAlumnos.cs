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
    public partial class frmCargasAlumnos : Form
    {
        CargasAlumnosQueries objCargaAlumno = new CargasAlumnosQueries();
        private String acción;
        public frmCargasAlumnos()
        {
            InitializeComponent();
            objCargaAlumno.RellenarCarreras(cmbCarrera);
            ActualizarTabla();
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            if(MenuOpciones.Visible)
            {
                btnMenu.Image = Properties.Resources.more;
                MenuOpciones.Visible = false;
            } else
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
            cmbMateria.Text = null;
            cmbMaestro.Text = null;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            // Botones del menu de opciones:
            // Nuevo, Actualizar, Guardar, Eliminar y Cancelar
            HabilitarBotonesMenu(0, 0, 0, 0, 1);
            btnBuscar.Enabled = true;
            txtAlumnoID.Enabled = true;
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
                    int AlumnoID = Convert.ToInt32(txtAlumnoID.Text);
                    int CarreraID = Convert.ToInt32(cmbCarrera.SelectedValue);
                    int MateriaID = Convert.ToInt32(cmbMateria.SelectedValue);
                    int MaestroID = Convert.ToInt32(cmbMaestro.SelectedValue);

                    if (acción == "nuevo")
                    {
                        objCargaAlumno.InsertarCargaAlumno(AlumnoID, CarreraID, MateriaID, MaestroID);
                    }
                    else if (acción == "actualizar")
                    {
                        int CargaID = Convert.ToInt32(dgvCargasAlumnos.Rows[dgvCargasAlumnos.CurrentRow.Index].Cells[0].Value);
                        objCargaAlumno.ActualizarCargaAlumno(CargaID, AlumnoID, CarreraID, MateriaID, MaestroID);
                    }

                    ActualizarTabla();
                    HabilitarBotonesMenu(1, 0, 0, 0, 0);
                    LimpiarCampos();
                    HabilitarCampos(false);
                    acción = "guardar";
                }

                
            } catch (Exception error)
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
                    int CargaID = Convert.ToInt32(dgvCargasAlumnos.Rows[dgvCargasAlumnos.CurrentRow.Index].Cells[0].Value);

                    objCargaAlumno.EliminarCargaAlumno(CargaID);
                    ActualizarTabla();
                    HabilitarBotonesMenu(1, 0, 0, 0, 0);
                    LimpiarCampos();
                    HabilitarCampos(false);
                } catch (Exception)
                {
                    MessageBox.Show("Selecciona la carga del alumno que deseas eliminar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                                cmbCarrera.SelectedValue = alumno.CarreraID;

                                if (acción == "nuevo")
                                {
                                    HabilitarCombos(1, 0);
                                } else
                                {
                                    HabilitarCombos(1, 1);
                                }
                            }
                        }
                        else
                        {
                            LimpiarCampos();
                            HabilitarCombos(0, 0);
                            txtAlumnoID.Focus();
                            MessageBox.Show("Número de alumno no encontrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception)
                    {
                        LimpiarCampos();
                        HabilitarCombos(0, 0);
                        txtAlumnoID.Focus();
                        MessageBox.Show("Error al obtener el número de alumno", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            } catch (Exception)
            {
                MessageBox.Show("Ingresa el número de Alumno", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAlumnoID.Focus();
            }
        }

        private void btnInicio_Click(object sender, EventArgs e)
        {
            Form1 inicio = new Form1();
            inicio.Show();
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
            cmbMateria.Enabled = estado;
            cmbMaestro.Enabled = estado;
        }

        private void cmbMateria_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int MateriaID = Convert.ToInt32(cmbMateria.SelectedValue);
            objCargaAlumno.ObtenerMaestrosPorMateriaID(MateriaID, cmbMaestro);
            cmbMaestro.Enabled = true;
        }

        private void CargarMaterias_SelectedValueChanged(object sender, EventArgs e)
        {
            if (acción == "nuevo" || acción == "actualizar")
            {
                try
                {
                    int CarreraID = Convert.ToInt32(cmbCarrera.SelectedValue);
                    objCargaAlumno.ObtenerMateriasPorCarrera(CarreraID, cmbMateria);
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
            objCargaAlumno.ObtenerCargasAlumnos(dgvCargasAlumnos);
        }

        private void cmbMaestro_SelectionChangeCommitted(object sender, EventArgs e)
        {
            HabilitarBotonesMenu(0, 0, 1, 0, 1);
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

            String NombreAlumno = dgvCargasAlumnos.CurrentRow.Cells[1].Value.ToString();
            String NombreCarrera = dgvCargasAlumnos.CurrentRow.Cells[2].Value.ToString();
            String NombreMateria = dgvCargasAlumnos.CurrentRow.Cells[3].Value.ToString();
            String NombreMaestro = dgvCargasAlumnos.CurrentRow.Cells[4].Value.ToString();

            txtNombreAlumno.Text = NombreAlumno;
            objCargaAlumno.BuscarAlumnoID(NombreAlumno, txtAlumnoID);
            objCargaAlumno.BuscarCarreraID(NombreCarrera, cmbCarrera);
            int CarreraID = Convert.ToInt32(cmbCarrera.SelectedValue);
            objCargaAlumno.BuscarMateriaID(CarreraID, NombreMateria, cmbMateria);
            int MateriaID = Convert.ToInt32(cmbMateria.SelectedValue);
            objCargaAlumno.BuscarMaestroID(MateriaID, NombreMaestro, cmbMaestro);
        }

        public void HabilitarCombos (int Materia, int Maestro)
        {
            cmbMateria.Enabled = Convert.ToBoolean(Materia);
            cmbMaestro.Enabled = Convert.ToBoolean(Maestro);
        }
    }
}
