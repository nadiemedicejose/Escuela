using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Escuela
{
    class CargasDocentesQueries
    {
        EscuelaDatabaseDataContext bdEscuela = new EscuelaDatabaseDataContext();

        public void ObtenerCargasDocentes(DataGridView dgvCargasDocentes)
        {
            try
            {
                var Registros = from valor in bdEscuela.ObtenerCargasDocentes()
                                select valor;
                dgvCargasDocentes.DataSource = Registros.ToList();
            }
            catch (Exception)
            {
                MessageBox.Show("Error al obtener las cargas docentes", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void InsertarCargaDocente(int MaestroID, int CarreraID, int MateriaID)
        {
            tblCargasDocente objCargaDocente = new tblCargasDocente();

            try
            {
                objCargaDocente.MaestroID = MaestroID;
                objCargaDocente.CarreraID = CarreraID;
                objCargaDocente.MateriaID = MateriaID;

bdEscuela.tblCargasDocentes.InsertOnSubmit(objCargaDocente);
                bdEscuela.SubmitChanges();

                MessageBox.Show("Carga docente guardada con éxito", "Éxito al guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception)
            {
                MessageBox.Show("Error al insertar la carga del docente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ActualizarCargaDocente(int CargaID, int MaestroID, int CarreraID, int MateriaID)
        {
            try
            {
                bdEscuela.ActualizarCargaDocente(CargaID, MaestroID, CarreraID, MateriaID);
                bdEscuela.SubmitChanges();
                MessageBox.Show("Actualizaste la carga del docente", "Éxito al guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Error al actualizar carga del docente", "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void EliminarCargaDocente(int CargaID)
        {
            try
            {
                bdEscuela.EliminarCargaDocente(CargaID);
                MessageBox.Show("Eliminaste la carga del maestro", "Éxito al eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Error al eliminar carga del maestro", "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void BuscarMaestroID(string NombreMaestro, TextBox MaestroID)
        {
            try
            {
                var Registros = from valor in bdEscuela.tblMaestros
                                where valor.NombreMaestro == NombreMaestro
                                select valor;
                if (Registros.Any())
                {
                    foreach (var maestro in Registros)
                    {
                        MaestroID.Text = maestro.MaestroID.ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Número de maestro no encontrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error al obtener el número de maestro", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        public void BuscarMateriaID(int MaestroID, int CarreraID, string NombreMateria, ComboBox cmbMateria)
        {
            ObtenerMateriasPorCarrera(MaestroID, CarreraID, cmbMateria);

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
            }
            catch (Exception)
            {

            }
        }

        public void ObtenerMateriasPorCarrera(int MaestroID, int CarreraID, ComboBox cmbMaterias)
        {
            cmbMaterias.DataSource = null;
            cmbMaterias.Items.Clear();

            var Registros = from valor in bdEscuela.BuscarMateriasNoAsignadasCD(MaestroID, CarreraID)
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
    }
}
