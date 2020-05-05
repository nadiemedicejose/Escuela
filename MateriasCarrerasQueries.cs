using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Escuela
{
    class MateriasCarrerasQueries
    {
        EscuelaDatabaseDataContext bdEscuela = new EscuelaDatabaseDataContext();

        public void ObtenerMateriasCarrera(DataGridView dgvMateriasCarrera)
        {
            try
            {
                var Registros = from valor in bdEscuela.ObtenerRelacionMateriasCarrera()
                                select valor;
                dgvMateriasCarrera.DataSource = Registros.ToList();
            }
            catch (Exception)
            {
                MessageBox.Show("Error al obtener registros", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void InsertarRelacionMateriaCarrera(int CarreraID, int MateriaID)
        {
            tblMateriasCarrera registro = new tblMateriasCarrera();

            try
            {
                registro.MateriaID = MateriaID;
                registro.CarreraID = CarreraID;

                bdEscuela.tblMateriasCarreras.InsertOnSubmit(registro);
                bdEscuela.SubmitChanges();

                MessageBox.Show("Relación guardada con éxito", "Éxito al guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception)
            {
                MessageBox.Show("Error al insertar relación", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ActualizarRelacionMateriasCarrera(int CarreraID_Anterior, int CarreraID, int MateriaID)
        {
            try
            {
                bdEscuela.ActualizarRelacionMateriasCarrera(CarreraID_Anterior, CarreraID, MateriaID);
                bdEscuela.SubmitChanges();
                MessageBox.Show("Actualizaste esta relación", "Éxito al guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Error al actualizar datos de esta relación", "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void EliminarRelacionMateriasCarrera(int CarreraID, int MateriaID)
        {
            try
            {
                bdEscuela.EliminarRelacionMateriasCarrera(CarreraID, MateriaID);
                MessageBox.Show("Eliminaste la relación", "Éxito al eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Error al eliminar relación", "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void BuscarRelacionMateriasCarrera(String NombreMateria, TextBox txtMateriaID, ComboBox cmbCarrera)
        {
            var Registros = from valor in bdEscuela.BuscarRelacionMateriasCarrera(NombreMateria).ToList()
                            select valor;
            if (Registros.Any())
            {
                foreach (var registro in Registros)
                {
                    txtMateriaID.Text = registro.MateriaID.ToString();
                    cmbCarrera.SelectedValue = registro.CarreraID;
                }
            }
            else
            {
                MessageBox.Show("Registro no encontrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
