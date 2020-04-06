using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Escuela
{
    class AlumnosQueries
    {
        EscuelaDatabaseDataContext bdEscuela = new EscuelaDatabaseDataContext();

        public void ObtenerAlumnos(DataGridView dgvAlumnos)
        {
            try
            {
                var Registros = from valor in bdEscuela.ObtenerAlumnos()
                                select valor;
                dgvAlumnos.DataSource = Registros.ToList();
            }
            catch (Exception)
            {
                MessageBox.Show("Error al obtener datos de alumnos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void InsertarAlumno(string NombreAlumno, int CarreraID)
        {
            tblAlumno objAlumno = new tblAlumno();

            try
            {
                objAlumno.NombreAlumno = NombreAlumno;
                objAlumno.CarreraID = CarreraID;

                bdEscuela.tblAlumnos.InsertOnSubmit(objAlumno);
                bdEscuela.SubmitChanges();

                MessageBox.Show("Alumno guardado con éxito", "Éxito al guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception)
            {
                MessageBox.Show("Error al insertar alumno", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ActualizarAlumno(int AlumnoID, string NombreAlumno, int CarreraID)
        {
            try
            {
                bdEscuela.ActualizarAlumno(AlumnoID, NombreAlumno, CarreraID);
                bdEscuela.SubmitChanges();
                MessageBox.Show("Actualizaste datos del alumno", "Éxito al guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Error al actualizar datos del alumno", "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void EliminarAlumno(int AlumnoID)
        {
            try
            {
                bdEscuela.EliminarAlumno(AlumnoID);
                MessageBox.Show("Eliminaste los datos del alumno", "Éxito al eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Error al eliminar datos del alumno", "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void BuscarCarreraID(string NombreCarrera, ComboBox cmbCarrera)
        {
            var Registros = from valor in bdEscuela.ObtenerCarreras().ToList()
                            where valor.Carrera == NombreCarrera
                            select valor;
            if (Registros.Any())
            {
                foreach (var carrera in Registros)
                {
                    cmbCarrera.SelectedValue = carrera.Id;
                }
            }
            else
            {
                MessageBox.Show("Número de carrera no encontrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void RellenarCarreras(ComboBox cmbCarreras)
        {
            try
            {
                cmbCarreras.Items.Clear();
                var Registros = from valor in bdEscuela.ObtenerCarreras()
                                select valor;
                cmbCarreras.DataSource = Registros.ToList();
                cmbCarreras.DisplayMember = "Carrera";
                cmbCarreras.ValueMember = "Id";
                cmbCarreras.SelectedItem = null;
            }
            catch (Exception)
            {

            }
        }

        public void BuscarAlumnoID(string NombreAlumno, TextBox AlumnoID)
        {
            var Registros = from valor in bdEscuela.tblAlumnos
                            where valor.NombreAlumno == NombreAlumno
                            select valor;
            if (Registros.Any())
            {

                foreach (var alumno in Registros)
                {
                    AlumnoID.Text = alumno.AlumnoID.ToString();
                }
            }
            else
            {
                MessageBox.Show("Número de alumno no encontrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
