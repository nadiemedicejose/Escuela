using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Escuela
{
    class UsuariosQueries
    {
        EscuelaDatabaseDataContext bdEscuela = new EscuelaDatabaseDataContext();

        public void ObtenerUsuarios(DataGridView dgvUsuarios)
        {
            try
            {
                var Registros = from valor in bdEscuela.ObtenerUsuarios()
                                select valor;
                dgvUsuarios.DataSource = Registros.ToList();
            }
            catch (Exception)
            {
                MessageBox.Show("Error al obtener datos de usuarios", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void InsertarUsuario(string NombreUsuario, string Contraseña)
        {
            tblUsuario objUsuario = new tblUsuario();

            try
            {
                objUsuario.NombreUsuario = NombreUsuario;
                objUsuario.Contraseña = Contraseña;

                bdEscuela.tblUsuarios.InsertOnSubmit(objUsuario);
                bdEscuela.SubmitChanges();

                MessageBox.Show("Usuario guardado con éxito", "Éxito al guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception)
            {
                MessageBox.Show("Error al insertar usuario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ActualizarUsuario(int UsuarioID, string NombreUsuario, string Contraseña)
        {
            try
            {
                bdEscuela.ActualizarUsuario(UsuarioID, NombreUsuario, Contraseña);
                bdEscuela.SubmitChanges();
                MessageBox.Show("Actualizaste datos del usuario", "Éxito al guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Error al actualizar datos del usuario", "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void EliminarUsuario(int UsuarioID)
        {
            try
            {
                bdEscuela.EliminarUsuario(UsuarioID);
                MessageBox.Show("Eliminaste los datos del usuario", "Éxito al eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Error al eliminar datos del usuario", "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        public void BuscarUsuarioID(string NombreUsuario, TextBox UsuarioID)
        {
            var Registros = from valor in bdEscuela.tblUsuarios
                            where valor.NombreUsuario == NombreUsuario
                            select valor;
            if (Registros.Any())
            {

                foreach (var usuario in Registros)
                {
                    UsuarioID.Text = usuario.UsuarioID.ToString();
                }
            }
            else
            {
                MessageBox.Show("Número de usuario no encontrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
