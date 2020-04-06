using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Escuela
{
    class MaestrosQueries
    {
        EscuelaDatabaseDataContext bdEscuela = new EscuelaDatabaseDataContext();

        public void ObtenerMaestros(DataGridView dgvMaestros)
        {
            try
            {
                var Registros = from valor in bdEscuela.ObtenerMaestros()
                                select valor;
                dgvMaestros.DataSource = Registros.ToList();
            }
            catch (Exception)
            {
                MessageBox.Show("Error al obtener datos de maestros", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void InsertarMaestro(string NombreMaestro, string Dirección)
        {
            tblMaestro objMaestro = new tblMaestro();

            try
            {
                objMaestro.NombreMaestro = NombreMaestro;
                objMaestro.Dirección = Dirección;

                bdEscuela.tblMaestros.InsertOnSubmit(objMaestro);
                bdEscuela.SubmitChanges();

                MessageBox.Show("Maestro guardado con éxito", "Éxito al guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception)
            {
                MessageBox.Show("Error al insertar maestro", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ActualizarMaestro(int MaestroID, string NombreMaestro, string Dirección)
        {
            try
            {
                bdEscuela.ActualizarMaestro(MaestroID, NombreMaestro, Dirección);
                bdEscuela.SubmitChanges();
                MessageBox.Show("Actualizaste datos del maestro", "Éxito al guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Error al actualizar datos del maestro", "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void EliminarMaestro(int MaestroID)
        {
            try
            {
                bdEscuela.EliminarMaestro(MaestroID);
                MessageBox.Show("Eliminaste los datos del maestro", "Éxito al eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Error al eliminar datos del maestro", "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        

        public void BuscarMaestroID(string NombreMaestro, TextBox MaestroID)
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
    }
}
