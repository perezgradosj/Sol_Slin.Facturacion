using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Slin.Facturacion.BusinessEntities;
using Slin.Facturacion.DataAccess;

namespace Slin.Facturacion.BusinessLogic
{
    public class UsuarioBusinessLogic
    {

        //public ListaUsuario GetListaUsuario(Usuario Usuario)
        //{
        //    return new UsuarioDataAccess().GetListaUsuario(Usuario);
        //}

        UsuarioDataAccess objUsuarioDataAccess = new UsuarioDataAccess();

        #region SEGURIDAD



        public ListaUsuario GetListaUsuario(Usuario oUsuario)
        {
            return objUsuarioDataAccess.GetListaUsuario(oUsuario);
        }

        public String RegistrarUsuario(Usuario oUsuario)
        {
            return objUsuarioDataAccess.RegistrarUsuario(oUsuario);
        }

        public String ActualizarUsuario(Usuario oUsuario)
        {
            return objUsuarioDataAccess.ActualizarUsuario(oUsuario);
        }

        public ListaUsuario ValidarUsername(String Username)
        {
            return objUsuarioDataAccess.ValidarUsername(Username);
        }

      

        #endregion

        
    }
}
