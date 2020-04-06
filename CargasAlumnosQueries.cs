using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Escuela
{
    class CargasAlumnosQueries
    {
        EscuelaDatabaseDataContext bdEscuela = new EscuelaDatabaseDataContext();

        public void ObtenerCargasAlumnos(DataGridView dgvCargasAlumnos)
        {
            try
            {
                var Registros = from valor in bdEscuela.ObtenerCargasAlumnos()
                                select valor;
                dgvCargasAlumnos.DataSource = Registros.ToList();
            } catch (Exception)
            {
                MessageBox.Show("Error al obtener las cargas académicas de los alumnos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void InsertarCargaAlumno(int AlumnoID, int CarreraID, int MateriaID, int MaestroID)
        {
            tblCargasAlumno objCargaAlumno = new tblCargasAlumno();

            try
            {
                objCargaAlumno.AlumnoID = AlumnoID;
                objCargaAlumno.CarreraID = CarreraID;
                objCargaAlumno.MateriaID = MateriaID;
                objCargaAlumno.MaestroID = MaestroID;

                bdEscuela.tblCargasAlumnos.InsertOnSubmit(objCargaAlumno);
                bdEscuela.SubmitChanges();

                MessageBox.Show("Carga de alumno guardada con éxito", "Éxito al guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception error)
            {
                MessageBox.Show("Error al insertar una carga de alumno: " + error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ActualizarCargaAlumno(int CargaID, int AlumnoID, int CarreraID, int MateriaID, int MaestroID)
        {
            try
            {
                bdEscuela.ActualizarCargaAlumno(CargaID, AlumnoID, CarreraID, MateriaID, MaestroID);
                bdEscuela.SubmitChanges();
                MessageBox.Show("Actualizaste la carga académica del estudiante", "Éxito al guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Error al actualizar carga académica del estudiante", "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void EliminarCargaAlumno(int CargaID)
        {
            try
            {
                bdEscuela.EliminarCargaAlumno(CargaID);
                MessageBox.Show("Eliminaste la carga académica del alumno", "Éxito al eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Error al eliminar carga académica del alumno", "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void BuscarAlumnoID(string NombreAlumno, TextBox AlumnoID)
        {
            try
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
            } catch (Exception)
            {
                MessageBox.Show("Error al obtener el número de alumno", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        public void BuscarMateriaID(int CarreraID, string NombreMateria, ComboBox cmbMateria)
        {
            ObtenerMateriasPorCarrera(CarreraID, cmbMateria);

            var Registros = from valor in bdEscuela.tblMaterias
                            where valor.NombreMateria == NombreMateria
                            select valor;
            if (Registros.Any())
            {

                foreach (var materia in Registros)
                {
                    cmbMateria.SelectedValue = materia.MateriaID;
                }
            }
            else
            {
                MessageBox.Show("Número de materia no encontrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void BuscarMaestroID(int MateriaID, string NombreMaestro, ComboBox cmbMaestro)
        {
            ObtenerMaestrosPorMateriaID(MateriaID, cmbMaestro);

            var Registros = from valor in bdEscuela.tblMaestros
                            where valor.NombreMaestro == NombreMaestro
                            select valor;
            if (Registros.Any())
            {

                foreach (var maestro in Registros)
                {
                    cmbMaestro.SelectedValue = maestro.MaestroID;
                }
            }
            else
            {
                MessageBox.Show("Número de maestro no encontrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            } catch (Exception)
            {

            }
        }

        public void ObtenerMateriasPorCarrera(int CarreraID, ComboBox cmbMaterias)
        {
            cmbMaterias.DataSource = null;
            cmbMaterias.Items.Clear();

            var Registros = from valor in bdEscuela.BuscarMateriasPorCarreraID(CarreraID)
                            select valor;
            cmbMaterias.DataSource = Registros.ToList();
            cmbMaterias.DisplayMember = "Materia";
            cmbMaterias.ValueMember = "Id";
            cmbMaterias.SelectedItem = null;
        }

        public void ObtenerMaestrosPorMateriaID(int MateriaID, ComboBox cmbMaestros)
        {
            cmbMaestros.DataSource = null;
            cmbMaestros.Items.Clear();

            var Registros = from valor in bdEscuela.BuscarMaestrosPorMateriaID(MateriaID)
                            select valor;
            cmbMaestros.DataSource = Registros.ToList();
            cmbMaestros.DisplayMember = "Maestro";
            cmbMaestros.ValueMember = "Id";
            cmbMaestros.SelectedItem = null;
        }

        public void BuscarAlumno(int AlumnoID, TextBox NombreAlumno, ComboBox cmbCarrera)
        {
            try
            {
                var Registros = from valor in bdEscuela.tblAlumnos
                                where valor.AlumnoID == AlumnoID
                                select valor;
                if (Registros.Any())
                {
                    foreach (var alumno in Registros)
                    {
                        NombreAlumno.Text = alumno.NombreAlumno;
                        cmbCarrera.SelectedValue = alumno.CarreraID;
                    }
                }
                else
                {
                    MessageBox.Show("Número de alumno no encontrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error al obtener el número de alumno", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
