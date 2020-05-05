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

        public void BuscarCargaDocenteCargaID(int CargaID, TextBox txtMaestroID, TextBox txtNombreMaestro, ComboBox cmbCarrera)
        {
            try
            {
                var Registros = from valor in bdEscuela.BuscarCargaDocenteCargaID(CargaID).ToList()
                                select valor;
                if (Registros.Any())
                {
                    foreach (var registro in Registros)
                    {
                        txtMaestroID.Text = registro.MaestroID.ToString();
                        txtNombreMaestro.Text = registro.NombreMaestro.ToString();
                        cmbCarrera.SelectedValue = registro.CarreraID;
                    }
                }
                else
                {
                    MessageBox.Show("Carga docente no encontrada", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error al obtener el número de maestro", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void BuscarMateriaID(int CarreraID, ComboBox cmbMateria)
        {
            var Registros = from valor in bdEscuela.tblMateriasCarreras
                            where valor.CarreraID == CarreraID
                            select valor;
            if (Registros.Any())
            {
                foreach (var registro in Registros)
                {
                    cmbMateria.SelectedValue = registro.MateriaID;
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
