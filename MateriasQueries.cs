using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Escuela
{
    class MateriasQueries
    {
        EscuelaDatabaseDataContext bdEscuela = new EscuelaDatabaseDataContext();

        public void ObtenerMaterias(DataGridView dgvMaterias)
        {
            try
            {
                var Registros = from valor in bdEscuela.ObtenerMaterias()
                                select valor;
                dgvMaterias.DataSource = Registros.ToList();
            }
            catch (Exception)
            {
                MessageBox.Show("Error al obtener datos de materias", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void InsertarMateria(string NombreMateria)
        {
            tblMateria objMateria = new tblMateria();

            try
            {
                objMateria.NombreMateria = NombreMateria;

                bdEscuela.tblMaterias.InsertOnSubmit(objMateria);
                bdEscuela.SubmitChanges();

                MessageBox.Show("Materia guardada con éxito", "Éxito al guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception)
            {
                MessageBox.Show("Error al insertar materia", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ActualizarMateria(int MateriaID, string NombreMateria)
        {
            try
            {
                bdEscuela.ActualizarMateria(MateriaID, NombreMateria);
                bdEscuela.SubmitChanges();
                MessageBox.Show("Actualizaste datos de la materia", "Éxito al guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Error al actualizar datos de la materia", "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void EliminarMateria(int MateriaID)
        {
            try
            {
                bdEscuela.EliminarMateria(MateriaID);
                MessageBox.Show("Eliminaste una materia", "Éxito al eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Error al eliminar materia", "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        public void BuscarMateriaID(string NombreMateria, TextBox MateriaID)
        {
            var Registros = from valor in bdEscuela.tblMaterias
                            where valor.NombreMateria == NombreMateria
                            select valor;
            if (Registros.Any())
            {
                foreach (var materia in Registros)
                {
                    MateriaID.Text = materia.MateriaID.ToString();
                }
            }
            else
            {
                MessageBox.Show("Número de materia no encontrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
