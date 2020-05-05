using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Escuela
{
    class CarrerasQueries
    {
        EscuelaDatabaseDataContext bdEscuela = new EscuelaDatabaseDataContext();

        // Obtiene las carreras y las inserta en DataGridView
        public void ObtenerCarreras(DataGridView dgvCarreras)
        {
            try
            {
                var Registros = from valor in bdEscuela.ObtenerCarreras()
                                select valor;
                dgvCarreras.DataSource = Registros.ToList();
            }
            catch (Exception)
            {
                MessageBox.Show("Error al obtener datos de carreras", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Obtiene las carreras y las inserta en ComboBox
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

        public void InsertarCarrera(string NombreCarrera)
        {
            tblCarrera objCarrera = new tblCarrera();

            try
            {
                objCarrera.NombreCarrera = NombreCarrera;

                bdEscuela.tblCarreras.InsertOnSubmit(objCarrera);
                bdEscuela.SubmitChanges();

                MessageBox.Show("Carrera guardada con éxito", "Éxito al guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception)
            {
                MessageBox.Show("Error al insertar carrera", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ActualizarCarrera(int CarreraID, string NombreCarrera)
        {
            try
            {
                bdEscuela.ActualizarCarrera(CarreraID, NombreCarrera);
                bdEscuela.SubmitChanges();
                MessageBox.Show("Actualizaste datos de la carrera", "Éxito al guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Error al actualizar datos de la carrera", "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void EliminarCarrera(int CarreraID)
        {
            try
            {
                bdEscuela.EliminarCarrera(CarreraID);
                MessageBox.Show("Eliminaste una carrera", "Éxito al eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Error al eliminar carrera", "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void BuscarCarreraID(string NombreCarrera, TextBox CarreraID)
        {
            var Registros = from valor in bdEscuela.tblMaterias
                            where valor.NombreMateria == NombreCarrera
                            select valor;
            if (Registros.Any())
            {
                foreach (var materia in Registros)
                {
                    CarreraID.Text = materia.MateriaID.ToString();
                }
            }
            else
            {
                MessageBox.Show("Número de carrera no encontrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
